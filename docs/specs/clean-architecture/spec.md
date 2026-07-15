# clean-architecture — 仕様

## 1. 目的と背景

C#/WPF 製テトリスの改修計画 第 2 段(PR2)。現状はロジック層 `TetrisLogic` にドメイン(Block・Field・enum)とゲーム進行(GameManager)が混在し、GameManager にタイミング(FPS/FrameRate/DownRate/フレームカウンタ)とゲームルール(スコア・REN・B2B・レベル・ゴースト・ゲームオーバー)が同居している。座標に `System.Drawing.Point` を用い、`IUserAction` は `ref` 渡しでインスタンスを差し替える。

本作業では、層を Domain / Application / Infrastructure に分離し、座標を新しい値型 `Position` に置換し、タイミングをゲームルールから切り離す(`GameSession.Advance(TimeSpan)`)。SRS 壁蹴り・T-Spin 判定・Hold のインスタンス差し替えは**式を書き換えず移植のみ**とし、既存 114 テストの挙動を保つ。WPF(`TetrisWindow`)は新 API へ接続し直すが、挙動は不変に保つ。

## 2. スコープ

### 対象(やること)

- 新規プロジェクト 3 つ(いずれも `net10.0`)への層分離: `Tetris.Domain` / `Tetris.Application` / `Tetris.Infrastructure`。既存 `TetrisLogic` を解体してこの 3 層へ移設する。
- 新しい値型 `readonly record struct Position(int X, int Y)` を Domain に定義し、`System.Drawing.Point` を全面置換する(Block/Field/GameSession/コマンド/WPF/テスト)。
- スコア規則を Domain(純粋計算)へ、実行時の累積を Application `ScoreManager` へ分離する。
- `IUserAction` の `ref` 渡しを廃し、`GameSession` が可変状態(CurrentBlock/HoldBlock/Field)を保持し、各コマンドが `GameSession` を操作する形へ変換する。UserAction Factory は Application 内部に隠蔽する。
- `GameManager` からルール部を抽出した `GameSession`(タイミング非依存の進行ルール)を Application に新設する。
- タイミング分離: `GameSession.Advance(TimeSpan delta)`(重力アキュムレータ)+ コマンド適用に再設計する。レベル→落下間隔の対応をルールとして保持し、ms/セル換算を現状と等価に保つ。
- 表示用の読み取りモデル `GameStateSnapshot` と、旧 `GameManager` 公開プロパティ群に相当する互換ファサード(`Tick`/`Snapshot` を含む)を Application に用意する。
- `BlocksPoolManager`(7-bag)実装を Infrastructure へ移し、`IBlocksPoolManager` は Application に置く。
- 既存 114 テストを新構成・新 API・`Position` へ移行し、挙動(壁蹴り回帰・T-Spin・スコア・座標・Hold)を不変に保つ。テストプロジェクトを層に対応させて分割する。
- `Tetris.sln` へ新規プロジェクトを追加し、参照方向を守る。中央パッケージ管理(`Directory.Packages.props`)と共通 `Directory.Build.props` を導入する。
- WPF(`TetrisWindow`)を新 Application API へ接続し直し、動作を不変に保つ。

### 対象外(やらないこと)

- WPF UI の見た目・操作・レイアウト・演出の変更 — 理由: 本段は構造改修であり挙動不変が要件。
- UI 側の 16.67ms ティック機構の再設計 — 理由: 次段(UI 側)の責務。本段は `Advance(TimeSpan)`/`Tick` の受け口を用意するのみ。
- SRS 壁蹴り補正表・T-Spin 判定・スコア係数・7-bag 乱択・Hold 挙動の**アルゴリズム変更** — 理由: 移植のみ(挙動不変)が厳守事項。
- 新機能(設定画面・スコア保存・レベル上限変更等)の追加 — 理由: スコープ外。
- Linux コンテナ上での WPF ビルド/実行 — 理由: `net10.0-windows` は Linux でビルド不可。WPF の健全性は GitHub Windows CI が担保する。

## 3. 前提(未検証の賭け)

- 「既存 114 テストを壊さない」= **テストの挙動(アサーション)を不変に保つ**の意。名前空間・型(`Point`→`Position`)・API 形状(`ref` 廃止・コマンド化)の変更に伴いテストコード自体は機械的に移行する(依頼文が「Block/Field/GameManager/UserAction/**テスト**」の置換を明示)。 — 検証方法: 移行後 `dotnet test` で全件 pass、アサーション値・期待文字列は原文どおり。 / 状態: 未検証
- ルーティング承認: 依頼文の「まずルーティングと spec まで進め」を経路 C(単一 unit)承認と解釈する。 — 検証方法: spec 承認ゲートでユーザーが是正しなければ承認とみなす。 / 状態: 未検証
- 互換ファサードは旧 `GameManager` の公開プロパティ・メソッド(`Update(ActionTypes)`・`Start(int)`・`FrameRate`・`GameLevel`・各表示プロパティ)を**同名・同挙動**で維持する。これにより GameManagerTests と WPF の差分を名前空間・`Position` 置換に限定する。 — 検証方法: GameManagerTests が原文アサーションのまま pass。WPF は Windows CI でビルド成功。 / 状態: 未検証
- レベル→落下間隔の等価換算: 旧 `DownRate = GameLevel==0 ? 0 : max(1, 60 - GameLevel*5)`(フレーム数)× `FrameRate(=1000/60 ms)` を落下間隔とする。レベル 0 は自然落下なし。`GameSession.Advance` はこの間隔で重力を発火する。 — 検証方法: 換算式をコード内定数として保持し、facade `Update` 1 回=1 フレーム相当で GameManagerTests の落下挙動が一致。 / 状態: 未検証
- UA_* 系のコマンド単体テストは、コマンドを `GameSession` の可変状態に対して実行し、回転結果(`DrawBlock` 文字列)・壁蹴りオフセット・CanExecute 可否を原文どおり検証する形へ移行する。 — 検証方法: 壁蹴り回帰テストが原文の期待値で pass。 / 状態: 未検証
- コマンド型(`RotateRightCommand` 等)と `GameSession` の可変状態は、テストが直接駆動できる可視性(`public`)で公開する。Factory のみ内部に隠蔽する。 — 検証方法: テストがコマンド型を `new` して実行できる。 / 状態: 未検証
- 本タスク規模は decompose の要件目安(≤10)に収める。単位分割は PR2 の原子性(中間状態でビルド不能)に反するため単一 unit を維持する。 — 検証方法: 分解フェーズで要件 ≤10・メインタスク ≤8 に収まる。 / 状態: 未検証

## 4. 用語定義

| 用語 | 定義 |
| ---- | ---- |
| Domain 層 | フレームワーク非依存・時間概念なしの純粋ドメイン(Block/Field/enum/Position/スコア規則)。 |
| Application 層 | ルールと進行(GameSession/ScoreManager/コマンド/IBlocksPoolManager/Snapshot/互換ファサード)。 |
| Infrastructure 層 | 外部依存を持つ実装(7-bag の `BlocksPoolManager`)。 |
| コマンド(GameCommand) | ユーザー操作 1 種を表す指令。`GameSession` の可変状態を読み書きする(旧 UserAction 相当)。 |
| GameSession | タイミング非依存のゲーム進行ルール本体。可変状態(Field/CurrentBlock/HoldBlock)と重力アキュムレータを保持する。 |
| 互換ファサード | 旧 `GameManager` の公開契約を保つ薄いラッパ。`GameSession` へ委譲し、`Tick`/`Snapshot` も公開する。 |
| ms/セル換算 | レベルに応じた 1 セル自然落下あたりの実時間(ms)。旧実装のフレーム換算と等価に保つ。 |

## 5. 公開インターフェース(API)

> 記法は C# 擬似コード。名前空間は `Tetris.Domain` / `Tetris.Application` / `Tetris.Infrastructure`。移植対象(SRS/T-Spin/Hold/スコア係数/7-bag)は**式・数値を変更しない**。

### 5.1 Position(Tetris.Domain)

```csharp
public readonly record struct Position(int X, int Y);
```

- **事前/事後条件**: 不変。`X`/`Y` は任意の `int`(フィールド外・負値も表現可、旧 `Point` と同一意味)。
- **エラー**: なし(値型・例外を投げない)。

### 5.2 Block / Field / enum 群(Tetris.Domain)

- `enum BlockTypes { nothing, T, I, J, L, S, Z, O }`、`enum DirectionTypes { north=0, east, south, west }`、`enum TSpinTypes { notTSpin, tMini, tSpin }`、`enum FieldTypes { empty, block, fixedBlock, outOfField }` を Domain へ移設(定義変更なし)。
- `Block`: 公開メンバは現行と同一(`BlockType`・`Location`(型は `Position`)・`Direction`・`CanSwap`・`TSpinType`・`Block(BlockTypes,bool)`・`Block(Block)`・`ResetLocation()`・`MoveLocation(int,int)`・`RotateRight()`・`RotateLeft()`・`GetTSpinPoints()`・`GetBlockPoints()`・`GetBlockBottomPoints()`・`GetBlockLeftPoints()`・`GetBlockRightPoints()`・`GetBlockRotatePoints()`・`DrawBlock()`)。座標を返す箇所は `List<Position>` を返す。
- `Field`: 公開メンバは現行と同一(`Width`・`Height`・`InitField()`・`InitField(FieldTypes[,])`・`GetFieldBlockPointAndTypePairs()`→`List<(Position,BlockTypes)>`・`GetFieldBlockPoints()`→`List<Position>`・`GetFixedBlockPoints()`→`List<Position>`・`GetFieldType(int,int)`・`ExistsCollisionPoint(Block)`・`CountCollisionPoint(List<Position>)`・`UpdateField(Block,bool)`・`DrawField()`)。
- **事前/事後条件・エラー**: 現行と同一(挙動不変)。フィールドは 10×20 固定。

### 5.3 スコア: ScoreRule(Domain)/ ScoreManager(Application)

```csharp
// Tetris.Domain — 純粋なスコア規則(旧 ScoreManager の係数表を式変更なしで移植)
public static class ScoreRule
{
    // 消去ライン・T-Spin・REN・B2B・全消しから加点を計算する
    public static int Calculate(int line, TSpinTypes tSpin, int ren, bool btb, bool allClear);
}

// Tetris.Application — 実行時の累積
public sealed class ScoreManager
{
    public int Score { get; }
    public void Reset();
    public void Add(int line, TSpinTypes tSpin, int ren, bool btb, bool allClear); // ScoreRule で計算し累積
}
```

- **事後条件**: `ScoreManager.Add` 後、`Score` は `ScoreRule.Calculate` の戻り値ぶん増加する。係数表(base/additional の分岐)は旧実装と同値。
- **エラー**: なし。

### 5.4 コマンド(Tetris.Application)

```csharp
public enum GameCommand { nothing=0, moveLeft, moveRight, moveDown, rotateLeft, rotateRight, hold, hardDrop }

// ref 渡しを廃し、GameSession の可変状態を操作する
public interface IGameCommand
{
    bool CanExecute(GameSession session);
    void Execute(GameSession session);
}

// 具体コマンド(public、テストが直接駆動可能): MoveLeftCommand / MoveRightCommand /
// MoveDownCommand / RotateLeftCommand / RotateRightCommand / HoldCommand / HardDropCommand / NothingCommand
```

- **事前条件**: `session` は開始済み(`CurrentBlock` が有効)。
- **事後条件**: `CanExecute` が真のとき `Execute` は `session.CurrentBlock`/`HoldBlock`/`Field` を旧 UserAction と同一の規則で更新する。SRS 壁蹴り補正(通常 4 状態・I 専用 4 状態)、T-Spin 判定(`GetTSpinPoints` の衝突数 ≥3、`count==3 && MoveY==0` → tMini、他 → tSpin)、Hold のインスタンス差し替え(`holdBlock` が nothing なら退避のみ、そうでなければ交換、`CanSwap=false` 付与)を**式変更なしで移植**する。
- **エラー**: なし(`CanExecute` false のとき `Execute` を呼ばない前提。呼び出し側=GameSession が保証)。
- Factory(`GameCommand` → `IGameCommand`)は Application 内部(非公開)に隠蔽する。

### 5.5 IBlocksPoolManager(Application)/ BlocksPoolManager(Infrastructure)

```csharp
// Tetris.Application
public interface IBlocksPoolManager
{
    void Reset();
    Block TakeNextBlock();
    List<Block> GetNextBlocksPool();
}

// Tetris.Infrastructure — 7-bag(旧実装を式変更なしで移植)
public sealed class BlocksPoolManager : IBlocksPoolManager { /* ... */ }
```

- **事後条件**: 7 種を 1 バッグ単位で乱択供給する挙動は旧実装と同値(プールが 6 個以下=7 個未満になったら次バッグを補充。旧 `Count > 6` で return と等価)。

### 5.6 GameSession(Tetris.Application)

```csharp
public sealed class GameSession
{
    public GameSession(Field field, IBlocksPoolManager pool);

    // 可変状態(コマンド・テストがアクセスする)
    public Field Field { get; }
    public Block CurrentBlock { get; set; }
    public Block HoldBlock { get; set; }

    // 進行状態
    public bool IsGameOver { get; }
    public int Score { get; }
    public int Level { get; }                 // 旧 GameLevel(0–99)
    public TSpinTypes LastTSpin { get; }       // 旧 TSpinType
    public int LastClearedLines { get; }       // 旧 Line

    public void Start(int level = 1);          // level を 0–99 にクランプ

    // タイミング非依存のコマンド適用(旧 Act + UpdateGameState 相当の後処理を含む)
    // 戻り値: そのコマンドで下位アクションが成立したか(旧 canAction 相当)
    public bool Apply(GameCommand command);

    // 経過時間ぶん重力を進める(重力アキュムレータ)。level==0 は自然落下なし
    public void Advance(TimeSpan delta);

    public GameStateSnapshot Snapshot();
}
```

- **事前条件**: `Apply`/`Advance`/`Snapshot` は `Start` 後に呼ぶ。`IsGameOver` が真のとき `Apply`/`Advance` は状態を変えない(旧 `Update` の early-return と同値)。
- **事後条件(Apply)**: 旧 `GameManager.UpdateGameState` と同一。固定判定(`moveDown` 不成立 or `hardDrop` で固定)、ライン消去、REN/B2B、スコア加算、次ブロック生成、スポーン不可で `IsGameOver=true`、`Hold` 時の `CanSwap` 復帰・差し替え、レベルアップ(スコア差分 500 ごと)を保持する。
- **事後条件(Advance)**: 累積時間が現在レベルの落下間隔以上になるごとに 1 セルぶんの重力落下(`moveDown` 成立なら移動、不成立なら固定)を、旧フレーム換算と等価に発火する。`level==0` は落下しない。
- **エラー**: なし。異常入力(未開始での呼び出し)は事前条件違反(呼び出し側責任)。

### 5.7 GameStateSnapshot(Tetris.Application)

```csharp
public readonly record struct GameStateSnapshot(
    int FieldWidth,
    int FieldHeight,
    bool IsGameOver,
    int Score,
    int Level,
    TSpinTypes TSpinType,
    int Line,
    BlockTypes CurrentBlockType,
    IReadOnlyList<Position> CurrentBlockPoints,
    IReadOnlyList<Position> GhostBlockPoints,
    IReadOnlyList<(Position, BlockTypes)> FieldPointAndTypePairs,
    IReadOnlyList<Position> FieldBlockPoints,
    IReadOnlyList<Position> FixedBlockPoints,
    BlockTypes HoldBlockType,
    IReadOnlyList<BlockTypes> NextBlockTypes);
```

- **不変条件**: 生成時点の `GameSession` 状態の読み取り専用スナップショット。WPF はこれ(または互換ファサードの同等プロパティ)から描画する。

### 5.8 互換ファサード(Tetris.Application)

旧 `GameManager` の公開契約を保つラッパ。WPF と GameManagerTests の差分を最小化する。

```csharp
public sealed class GameManager
{
    public GameManager(Field field, IBlocksPoolManager pool);
    public bool IsGameOver { get; }
    public int Score { get; }
    public TSpinTypes TSpinType { get; }
    public int Line { get; }
    public double FrameRate { get; }           // 1000 / FPS(=60)。WPF タイマ間隔用
    public double GameLevel { get; }
    public double DownRate { get; }
    public List<Position> CurrentBlockPoints { get; }
    public BlockTypes CurrentBlocktype { get; }
    public List<Position> GhostBlockPoints { get; }
    public List<Position> FixedBlockPoints { get; }
    public List<(Position, BlockTypes)> FieldPointAndTypePairs { get; }
    public List<Position> FieldBlockPoints { get; }
    public BlockTypes HoldBlockType { get; }
    public List<BlockTypes> NextBlockTypes { get; }
    public int FieldWidth { get; }
    public int FieldHeight { get; }

    public void Start(int gamelevel = 1);
    public void Update(ActionTypes userAction);   // 旧フレーム駆動: 1 回=1 フレーム相当
    public void Tick(TimeSpan delta);             // 新規: GameSession.Advance へ委譲
    public GameStateSnapshot Snapshot();          // 新規
}
```

- **事後条件(Update)**: 旧 `GameManager.Update` と同値。内部で 1 フレーム(`FrameRate` ms)ぶんの重力(`GameSession.Advance`)+ 入力連射制御(旧 `DoContinueSameAction`: 同一操作 25 回以降 3 回に 1 回のみ受理)+ コマンド適用を行う。`ActionTypes` は旧 enum(`nothing, moveLeft, moveRight, moveDown, rotateLeft, rotateRight, hold, hardDrop`)を維持し `GameCommand` へ写像する。
- **エラー**: なし。

## 6. データ構造

- **Position**(§5.1): `readonly record struct`。不変条件なし(旧 `Point` と同一意味の単なる整数対)。生成は完全コンストラクタ。`System.Drawing` へ依存しない。
- **Block / Field**(§5.2): ロジック(回転・衝突・座標算出・ライン消去)は各型に集約(貧血化しない。現行の集約先を維持)。座標型のみ `Position` に置換。
- **ScoreRule / ScoreManager**(§5.3): 計算(規則)は Domain の純粋関数へ、可変な累積は Application の `ScoreManager` へ分離。ロジックの所在を明確化。
- **GameSession**(§5.6): 可変状態(Field/CurrentBlock/HoldBlock)と進行状態(Score/Level/REN/B2B/重力アキュムレータ)を保持。進行ルールを集約。時間概念は `Advance(TimeSpan)` の引数としてのみ受け取り、内部に FPS/フレームカウンタを持たない(換算定数=レベル→間隔のみルールとして保持)。
- **GameStateSnapshot**(§5.7): 読み取り専用の交換形式(read model)。可変状態を露出しない。
- **依存方向**: Presentation(WPF)→ Application → Domain、Infrastructure → Application/Domain。逆参照は禁止。Domain はフレームワーク非依存・時間概念なし。

## 7. 振る舞い(受け入れ基準)

### Requirement 1: プロジェクト構成・依存方向・検証境界

**対象**: §2・§6(依存方向)

**受け入れ基準**:
1.1. システムは、`Tetris.Domain` / `Tetris.Application` / `Tetris.Infrastructure` の 3 プロジェクト(いずれも `net10.0`)を持ち、`Tetris.sln` に登録されていなければならない。(常時)
1.2. システムは、参照方向を Presentation→Application→Domain、Infrastructure→Application/Domain に限定しなければならない(Domain は他層を参照しない)。(常時)
1.3. システムは、中央パッケージ管理(`Directory.Packages.props`)と共通 `Directory.Build.props` を導入し、テストパッケージのバージョンを一元管理しなければならない。(常時)
1.4. Domain 層をビルドする場合、システムは `System.Drawing` および WPF/時間 API に依存してはならない。(制約)
1.5. Linux 環境で検証する場合、システムは `Tetris.Domain`・`Tetris.Application`・`Tetris.Infrastructure` と各テストプロジェクトのみを対象にビルド/テストできなければならず、`TetrisWindow`(`net10.0-windows`)はローカルビルド対象から除外しなければならない。(制約)
1.6. `main` へ push または PR を作成した場合、システム(Windows CI)はソリューション全体をビルドしテストしなければならない。(イベント)

### Requirement 2: Position 値型と System.Drawing.Point の全廃

**対象**: §5.1 Position / §5.2 Block・Field

**受け入れ基準**:
2.1. システムは、`Tetris.Domain` に `public readonly record struct Position(int X, int Y)` を定義しなければならない。(常時)
2.2. システムは、Block・Field・GameSession・コマンド・WPF・テストの座標表現をすべて `Position` に置換し、`System.Drawing.Point` への参照を残してはならない。(常時)
2.3. `.X`/`.Y` でアクセスする場合、`Position` は旧 `Point` と同一の意味(整数の X/Y、負値・フィールド外可)を返さなければならない。(常時)

### Requirement 3: Domain 層への移設(挙動不変)

**対象**: §5.2 Block・Field・enum 群

**受け入れ基準**:
3.1. システムは、`BlockTypes`・`DirectionTypes`・`TSpinTypes`・`FieldTypes` と `Block`・`Field` を `Tetris.Domain` に定義しなければならない。(常時)
3.2. `Block.RotateRight`/`RotateLeft` を呼ぶ場合、システムは旧実装と同一の回転結果(`DrawBlock` 文字列)を返さなければならない。(イベント)
3.3. `Field.UpdateField` でライン消去を行う場合、システムは旧実装と同一の消去ライン数とフィールド状態を返さなければならない。(イベント)
3.4. 座標・衝突・T-Spin 判定用座標(`GetTSpinPoints` 等)を取得する場合、システムは旧実装と同一の座標集合を返さなければならない。(イベント)

### Requirement 4: スコア規則(Domain)と累積(Application)

**対象**: §5.3 ScoreRule・ScoreManager

**受け入れ基準**:
4.1. システムは、消去ライン数・T-Spin 種別・REN・B2B・全消しから、旧 `ScoreManager` と同一の係数表で加点を算出しなければならない。(常時)
4.2. `ScoreManager.Add` を呼ぶ場合、システムは `Score` を算出加点ぶん増加させなければならない。(イベント)
4.3. `ScoreManager.Reset` を呼ぶ場合、システムは `Score` を 0 に戻さなければならない。(イベント)
4.4. スコア規則(`ScoreRule`)は Domain に置き、時間・可変状態・フレームワークに依存してはならない。(制約)

### Requirement 5: コマンド化(ref 廃止)と SRS/T-Spin/Hold の移植

**対象**: §5.4 IGameCommand・各コマンド

**受け入れ基準**:
5.1. システムは、`IGameCommand`(`CanExecute(GameSession)`/`Execute(GameSession)`)を定義し、`ref` 渡しを用いてはならない。(制約)
5.2. 回転コマンドを実行する場合、システムは SRS 壁蹴り補正(通常 4 状態・I 専用 4 状態)を式・オフセット値を変更せず適用しなければならない。(イベント)
5.3. T ブロックの回転後、システムは `GetTSpinPoints` の衝突数が 3 以上のとき、`count==3 かつ MoveY==0` なら `tMini`、それ以外なら `tSpin` を、衝突数 3 未満なら `notTSpin` を設定しなければならない(旧判定式と同値)。(イベント)
5.4. Hold コマンドを実行する場合、`HoldBlock` が nothing なら現ブロックを退避し `CurrentBlock` を nothing にし、そうでなければ両者を交換し、退避したブロックに `CanSwap=false` を付与しなければならない(旧 `UA_Hold` と同値)。(イベント)
5.5. Hold の交換可否を判定する場合、`HoldBlock` が nothing なら真、そうでなければ `HoldBlock.CanSwap` を返さなければならない。(条件)
5.6. コマンドの生成 Factory(`GameCommand`→`IGameCommand`)は Application 内部に隠蔽し、公開 API に露出してはならない。(制約)

### Requirement 6: GameSession(進行ルール・タイミング非依存)

**対象**: §5.6 GameSession

**受け入れ基準**:
6.1. `GameSession.Start(level)` を呼ぶ場合、システムは level を 0–99 にクランプし、フィールド初期化・プールリセット・初手取得・スコアリセットを行い、`IsGameOver=false` にしなければならない。(イベント)
6.2. `Apply(command)` を呼ぶ場合、システムは旧 `GameManager` の該当処理と同一の結果(移動/回転/固定/ライン消去/REN・B2B/スコア加算/次ブロック生成/レベルアップ/Hold 差し替え)を生成しなければならない。(イベント)
6.3. `moveDown` が不成立、または `hardDrop` を適用した場合、システムはブロックを固定し、消去ライン数に応じてスコアと REN/B2B を更新しなければならない。(イベント)
6.4. 次ブロックのスポーンが不可能な場合、システムは `IsGameOver=true` にしなければならない。(異常系)
6.5. `IsGameOver` が真の場合、`Apply`/`Advance` はゲーム状態を変更してはならない。(異常系)
6.6. GameSession は内部に FPS・フレームカウンタを持たず、時間は `Advance(TimeSpan)` の引数としてのみ受け取らなければならない。(制約)

### Requirement 7: タイミング分離(Advance と等価換算)

**対象**: §5.6 GameSession.Advance

**受け入れ基準**:
7.1. システムは、レベル L(1–99)の落下間隔を `max(1, 60 - L*5)` フレーム × `(1000/60) ms` として保持し、旧 `DownRate`×`FrameRate` と等価にしなければならない。(常時)
7.2. `level==0` の場合、`Advance` は自然落下を発火してはならない。(条件)
7.3. `Advance(delta)` を呼ぶ場合、システムは累積時間が現在レベルの落下間隔以上になるごとに 1 セルぶんの重力落下(移動または固定)を発火しなければならない。(イベント)
7.4. 落下間隔に満たない `delta` を複数回渡した場合、システムは累積時間を保持し、閾値到達で 1 セル落下しなければならない(取りこぼし・多重発火しない)。(イベント)

### Requirement 8: 読み取りモデルと互換ファサード・WPF 再接続

**対象**: §5.7 GameStateSnapshot / §5.8 互換ファサード

**受け入れ基準**:
8.1. システムは、`GameStateSnapshot`(read model)に WPF 描画に必要な全項目(§5.7)を含め、可変状態を露出してはならない。(常時)
8.2. 互換ファサード `GameManager` は、旧公開プロパティ・メソッド(`Update(ActionTypes)`・`Start(int)`・`FrameRate`・`GameLevel`・各表示プロパティ・`IsGameOver`・`Score`・`TSpinType`・`Line`)を同名・同挙動で提供しなければならない。(常時)
8.3. 互換ファサードは、新規に `Tick(TimeSpan)`(`GameSession.Advance` へ委譲)と `Snapshot()`(`GameStateSnapshot` を返す)を提供しなければならない。(常時)
8.4. `Update(ActionTypes)` を呼ぶ場合、システムは 1 フレームぶんの重力 + 入力連射制御(同一操作 25 回以降 3 回に 1 回受理)+ コマンド適用を、旧 `GameManager.Update` と同値に実行しなければならない。(イベント)
8.5. WPF(`TetrisWindow`)は新 Application API(互換ファサード)に接続し直され、キー操作・描画・スコア表示・ゲームオーバー演出の挙動を変えてはならない。(制約、Windows CI で担保)

### Requirement 9: BlocksPoolManager(Infrastructure・7-bag)

**対象**: §5.5 IBlocksPoolManager・BlocksPoolManager

**受け入れ基準**:
9.1. システムは、`IBlocksPoolManager` を Application に、`BlocksPoolManager`(7-bag 実装)を Infrastructure に置かなければならない。(常時)
9.2. `TakeNextBlock` を呼ぶ場合、システムは 7 種を 1 バッグ単位で乱択供給し、プールが 6 個以下(=7 個未満)になったら次バッグを補充しなければならない(旧 `Count > 6` で return と等価)。(イベント)
9.3. `GetNextBlocksPool` を呼ぶ場合、システムは現在のプールを落下順で返さなければならない。(イベント)

### Requirement 10: テスト移行・回帰・挙動不変

**対象**: §2・全 §5

**受け入れ基準**:
10.1. システムは、既存 114 テストを新構成・新 API・`Position` へ移行し、アサーション値・期待文字列を原文どおり保ったまま全件 pass させなければならない。(常時)
10.2. システムは、テストプロジェクトを層に対応させて分割(`Tetris.Domain.Tests` / `Tetris.Application.Tests` / `Tetris.Infrastructure.Tests`)し、それぞれ Linux 上で `dotnet test` 可能にしなければならない。(常時)
10.3. 回転壁蹴り回帰テストを実行する場合、システムは旧実装と同一の回転結果・壁蹴り後座標を返さなければならない。(イベント)
10.4. Windows CI を実行する場合、システムはソリューション全体(WPF 含む)のビルドと全テストを成功させなければならない。(イベント)

## 8. 実現方針(要点のみ)

- **層とパッケージ**: `TetrisLogic` を解体し Domain/Application/Infrastructure へ再配置。`TetrisWindow` は Application を参照(Domain は推移参照)。`Directory.Packages.props`(中央パッケージ管理)+ `Directory.Build.props`(`TargetFramework=net10.0`・`Nullable=enable` の共通化。ただし WPF は `net10.0-windows`/`UseWPF` を自プロジェクトで指定)。
- **タイミング分離**: レベル→間隔換算(`max(1,60-L*5)` フレーム × `1000/60` ms)をルール定数として GameSession(または Domain のルール)に保持。GameSession は `Advance(TimeSpan)` で重力アキュムレータを進める。互換ファサード `Update` が「1 フレーム=`FrameRate` ms の `Advance` + コマンド」で旧フレーム駆動を再現し、GameManagerTests と WPF を無改造挙動で維持。
- **コマンド化**: 旧 `IUserAction`(`ref`)を `IGameCommand`(`GameSession` 操作)へ。SRS/T-Spin/Hold の式・数値は移植のみ。Factory は internal。
- **移植の厳守**: SRS 補正表・T-Spin 判定・Hold 差し替え・スコア係数・7-bag は式を書き換えない。
- **検証境界**: ローカル(Linux)は非 WPF プロジェクト + テストのみ `dotnet build`/`dotnet test`。WPF はコード上のみ新 API へ追従させ、ビルド確認は Windows CI に委ねる。

## 9. 参考資料

- 旧実装: `TetrisLogic/GameManager/GameManager.cs`(タイミング+ルール混在)、`TetrisLogic/GameObject/{Block,Field}.cs`、`TetrisLogic/UserAction/*`(SRS/T-Spin/Hold)、`TetrisLogic/GameManager/{ScoreManager,BlocksPoolManager}.cs`。
- WPF 連携: `TetrisWindow/MainWindow.xaml.cs`(`GameManager` の公開プロパティ・`Update`・`FrameRate` を使用)。
- CI: `.github/workflows/ci.yml`(Windows でソリューション全体をビルド・テスト)。
- 既存テスト 114 件: `TetrisLogicTests/**`(壁蹴り回帰=`UserAction/UA_RotateRightTests.cs` 等)。
