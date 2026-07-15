# avalonia-migration — 仕様

## 1. 目的と背景

現在のプレゼンテーション層は WPF(`TetrisWindow`, `net10.0-windows`)のみで、Windows でしか動作せず、Linux コンテナ/CI ではビルドすらできない。直前の段でロジックは Domain / Application / Infrastructure に層分離済みであり、プレゼンテーションを差し替える準備が整っている。本作業単位は、Avalonia(`net10.0`)による新しいデスクトップ UI `Tetris.Avalonia` を追加し、既存の読み取りモデル(`GameStateSnapshot`)とゲーム進行(`GameSession`)を再利用してクロスプラットフォーム(Windows / macOS / Linux)対応にする。あわせて WPF 一式を撤去し、`Tetris.sln` 全体が Linux でビルド可能な状態にすることを価値とする。

## 2. スコープ

### 対象(やること)

- 新規プロジェクト `Tetris.Avalonia`(Avalonia, `net10.0`, 実行可能アプリ)を追加し、`Tetris.Application` / `Tetris.Infrastructure` を参照する。
- DI に `Microsoft.Extensions.DependencyInjection`、MVVM に `CommunityToolkit.Mvvm` を用い、合成ルート(composition root)で `IBlocksPoolManager` とゲーム進行を登録する。
- 盤面描画を、名前付きセル(`FindName`)方式を廃し、`GameStateSnapshot` 駆動のカスタム `Control`(`Render(DrawingContext)` で 10×20 を直接描画)で実装する。
- ホールド/ネクスト/スコア/T-Spin 演出/ゲームオーバー演出を、バインドした小パネル/テキストで実装する(WPF の考え方を踏襲)。
- ゲームループを `Avalonia.Threading.DispatcherTimer`(約 16ms)で回し、経過時間を `Stopwatch` 差分として `GameSession.Advance(TimeSpan)` に渡す(タイマのジッタが落下速度に影響しない)。
- 入力を `Avalonia.Input.Key` にマッピングして操作/コマンドへ変換する(WPF のキー割当を踏襲)。
- WPF プロジェクト `TetrisWindow` 一式を削除し、`Tetris.sln` から除去する。`Tetris.sln` に `Tetris.Avalonia` を追加する。
- `README.md` にクロスプラットフォームのビルド/実行手順を追記する。

### 対象外(やらないこと)

- Domain / Application / Infrastructure の変更 — 理由: ロジックは前段で凍結済みで、本作業は挙動不変のプレゼンテーション差し替えに限定する(既存の公開 API を利用するのみ)。
- 新しいゲーム機能・ルール・スコアリングの追加や変更 — 理由: 移植の等価性を保つため。
- モバイル(iOS/Android)・WebAssembly ブラウザ対応 — 理由: 本段はデスクトップ 3 OS に限定する。Avalonia の構成上、後段で追加可能な余地は残すが本段では扱わない。
- GUI の実プレイ(描画・キー操作)の自動テストによる合否判定 — 理由: コンテナ/CI に表示がなく、目視確認できない(§3・§7 Requirement 9 参照)。
- WPF が持っていた演出の**タイミング/アニメーションの厳密な再現**(DAS 連射の正確なフレーム周期、ゲームオーバーのランダム塗り潰し順序など) — 理由: 目視でしか確認できず機械検証の対象外のため、考え方のみ踏襲し厳密一致は求めない。

## 3. 前提(未検証の賭け)

- **【検証境界】GUI の実プレイは機械検証できない** — 検証方法: コンテナ/CI では `Tetris.Avalonia` の**ビルド成功**と、ロジックテスト(Domain / Application / Infrastructure)の**緑維持**のみを合否基準とする。描画・入力・演出の実挙動は、人間が後日 `dotnet run --project Tetris.Avalonia` で目視確認する前提とする。/ 状態: 未検証(人間確認待ち)。
- **Avalonia は Linux でビルド・実行可能で、`net10.0` 単一 TFM でデスクトップ 3 OS 対応できる** — 検証方法: `dotnet build Tetris.sln` が Linux コンテナで成功すること。/ 状態: 未検証(実装時に確認)。
- **ゲーム進行の登録対象として互換ファサード `GameManager` ではなく `GameSession` を採用する**(お任せ判断) — 理由: 本段の要求「経過時間を `Stopwatch` 差分で `Advance(TimeSpan)` に渡し、ジッタが落下速度に影響しないこと」と「入力を時間非依存に適用すること」を両立するには、重力は `GameSession.Advance(delta)`、入力は `GameSession.Apply(GameCommand)` を直接呼ぶ必要がある。互換ファサード `GameManager` は `Tick(delta)`(重力のみ)は公開するが公開の `Apply` を持たず、`Update(ActionTypes)` は固定フレーム量(`FrameRate` ms)を前提とするためジッタ非依存にできない。したがって `GameSession` を合成ルートに登録する。/ 検証方法: ビルド成功とロジックテスト緑。/ 状態: 確定(本 spec の設計判断)。
- **押しっぱなし移動(オートリピート)は OS のキーリピート(繰り返し `KeyDown`)に委ねる**(お任せ判断) — 理由: WPF の DAS(25 フレーム後に 3 フレームごと)はプレゼンテーション層の入力連射制御であり、目視でしか確認できない。厳密再現より単純さを優先し、`KeyDown` ごとに 1 回コマンドを適用する方式とする(移動系キーの連続移動は OS のキーリピートで実現)。連射周期の WPF との差は許容する。/ 状態: 未検証(人間確認待ち)。
- **Avalonia の headless レンダリングによるスモークテストは任意**とする — 検証方法: 導入する場合も合否は「ビルド + 起動して例外なくフレームを 1 回描画できる」程度に限り、実プレイ判定には用いない。/ 状態: 未検証(任意)。

## 4. 用語定義

| 用語 | 定義 |
| ---- | ---- |
| スナップショット | `GameStateSnapshot`。ある時点の `GameSession` の読み取り専用の状態(盤面・現在ブロック・ゴースト・ホールド・ネクスト・スコア・T-Spin 種別など)。 |
| 合成ルート | アプリ起動時に依存(`IBlocksPoolManager`・`Field`・`GameSession`・ViewModel・View)を組み立てる単一箇所(DI コンテナの構成)。 |
| 盤面コントロール | `GameStateSnapshot` を入力に 10×20 セルを `DrawingContext` へ直接描画するカスタム `Control`。 |
| DAS | Delayed Auto Shift。移動キー押しっぱなし時の自動連続移動。本段では OS キーリピートに委ねる(§3)。 |

## 5. 公開インターフェース(API)

本作業単位が新規に外部公開する契約は「実行可能アプリ `Tetris.Avalonia`」であり、ライブラリ API を新設しない。利用する既存契約(変更しない)と、プロジェクト内の主要コンポーネントの契約を以下に定める。

### 5.1 プロジェクト `Tetris.Avalonia`(成果物)

- **定義**: `Tetris.Avalonia.csproj`。`<OutputType>Exe</OutputType>`、`<TargetFramework>net10.0</TargetFramework>`。
- **依存(パッケージ)**: `Avalonia`、`Avalonia.Desktop`、`Avalonia.Themes.Fluent`(テーマ)、`CommunityToolkit.Mvvm`、`Microsoft.Extensions.DependencyInjection`。バージョンは実装時に `net10.0` 対応の安定版を採用し、採用版を §9 に記録する。
- **参照(プロジェクト)**: `Tetris.Application`、`Tetris.Infrastructure`。
- **事前条件**: `net10.0` SDK が利用可能。
- **事後条件**: `dotnet build` が Windows / macOS / Linux で成功する。`dotnet run` でウィンドウが起動する。
- **エラー**: 起動時に DI 解決に失敗した場合は例外を送出してプロセスを終了する(無効状態で継続しない)。

### 5.2 利用する既存契約(変更しない・参照のみ)

- `Tetris.Application.GameSession`
  - `GameSession(Field field, IBlocksPoolManager pool)` / `void Start(int level = 1)` / `bool Apply(GameCommand command)` / `void Advance(TimeSpan delta)` / `GameStateSnapshot Snapshot()` / `bool IsGameOver`。
- `Tetris.Application.GameCommand`(列挙): `nothing, moveLeft, moveRight, moveDown, rotateLeft, rotateRight, hold, hardDrop`。
- `Tetris.Application.IBlocksPoolManager` と実装 `Tetris.Infrastructure.BlocksPoolManager`(7-bag)。
- `Tetris.Domain.Field`(`Width`/`Height` = 10/20)、`Tetris.Domain.Position`、`Tetris.Domain.BlockTypes`(`nothing,T,I,J,L,S,Z,O`)、`Tetris.Domain.TSpinTypes`(`notTSpin, tMini, tSpin`)。
- `Tetris.Application.GameStateSnapshot`(§6.1)。
- **事前条件**: これらのシグネチャ・挙動を変更しない。プレゼンテーションは公開メンバのみを呼ぶ。

### 5.3 合成ルート(DI)

- **定義**: アプリ起動時に `IServiceProvider` を構築する単一箇所。
- **登録内容(最低限)**: `IBlocksPoolManager → BlocksPoolManager`、`Field`、`GameSession`(`Field` と `IBlocksPoolManager` を注入)、メイン ViewModel、メインウィンドウ。
- **事前条件**: なし(起動時に 1 回だけ構築する)。
- **事後条件**: メインウィンドウとその ViewModel が、単一の `GameSession` インスタンスを共有して解決される。
- **エラー**: 未登録依存の解決要求は起動時例外として表面化させる(隠蔽しない)。

### 5.4 盤面コントロール `BoardControl`

- **定義**: Avalonia の `Control` を継承し、`public override void Render(DrawingContext context)` を実装するカスタムコントロール。描画対象のスナップショットを受け取る依存プロパティ/`StyledProperty`(例: `GameStateSnapshot? Snapshot`)を持つ。
- **入力**: `GameStateSnapshot`(盤面サイズ・現在ブロック点・ゴースト点・確定/フィールドブロックの (座標, 種別) 対)。
- **出力(描画)**: 10×20 の各セルを矩形として直接描画する。名前付きセル・`FindName` を使わない。
- **事前条件**: `Snapshot` が `null` の場合は空盤面(背景色)を描画する(例外を投げない)。
- **事後条件**: `Snapshot` が更新されたら再描画される(`InvalidateVisual` 相当)。
- **描画規則**:
  - セル色は §6.2 の `BlockTypes → 色` マッピングに従う。空セル/`nothing` は `DarkGray` 相当。
  - ゴーストブロックは現在ブロック色を不透明度 0.5 で描画する。
  - フィールドの確定/移動中ブロックは不透明度 1.0 で描画する。
- **エラー**: スナップショットのブロック点が盤面外を指す場合でも例外で落とさず、盤面内のセルのみ描画する。

### 5.5 入力ハンドラ(キーマッピング)

- **定義**: メインウィンドウ/ViewModel が `KeyDown`/`KeyUp`(`Avalonia.Input.KeyEventArgs`)を受け、`Avalonia.Input.Key` を操作へ変換する。
- **プレイ中のマッピング**(WPF 踏襲):
  - `Up` → ハードドロップ(`GameCommand.hardDrop`)
  - `Down` → 下移動(`moveDown`) / `Left` → 左移動(`moveLeft`) / `Right` → 右移動(`moveRight`)
  - `Z` → 左回転(`rotateLeft`) / `X` → 右回転(`rotateRight`)
  - `Space` → ホールド(`hold`)
  - `P` → 一時停止 / `Esc` → 一時停止(再度 `Esc` で終了)
- **一時停止中のマッピング**: `Space` → 再開 / `R` → リセット(再スタート) / `Esc` → 終了。
- **ゲームオーバー時のマッピング**: `Space` → 開始(レベル 1 でスタート) / `Esc` → 終了。
- **事前条件**: ハードドロップ・回転・ホールドは 1 回の `KeyDown` につき 1 回だけ適用する(押下の重複適用をしない)。移動系の連続入力は OS キーリピートに委ねる(§3)。
- **事後条件**: 変換した `GameCommand` は `GameSession.Apply` に渡す。一時停止/リセット/終了はプレゼンテーション状態またはループ制御に反映する(ロジックには渡さない)。
- **エラー**: マッピングに無いキーは無視する(`GameCommand.nothing` 相当、何もしない)。

### 5.6 ゲームループ

- **定義**: `Avalonia.Threading.DispatcherTimer`(`Interval` ≈ 16ms)による周期処理。
- **手順(各 tick)**: `Stopwatch` で前 tick からの実経過時間 `delta` を測り、ゲーム進行中は `GameSession.Advance(delta)` を呼び、`Snapshot()` を取得して ViewModel/盤面へ反映する。`IsGameOver` になったらループを止め、ゲームオーバー表示へ遷移する。
- **事前条件**: 一時停止中・ゲームオーバー中は `Advance` を呼ばない(進行を止める)。再開時は経過時間の累積が落下に持ち越されないよう `Stopwatch` を再基準化する。
- **事後条件**: タイマ間隔がゆらいでも、実経過時間 `delta` を渡すため落下速度は実時間基準で一定に保たれる(§7 Requirement 4)。
- **エラー**: tick 内の描画/反映で例外が出た場合はループを停止し、無効状態で描画し続けない。

## 6. データ構造

### 6.1 `GameStateSnapshot`(既存・再利用/変更しない)

`Tetris.Application.GameStateSnapshot`(`readonly record struct`)を読み取りモデルとしてそのまま用いる。主なフィールド: `FieldWidth, FieldHeight, IsGameOver, Score, Level, TSpinType, Line, CurrentBlockType, CurrentBlockPoints, GhostBlockPoints, FieldPointAndTypePairs, FieldBlockPoints, FixedBlockPoints, HoldBlockType, NextBlockTypes`。

- **不変条件**: 生成時点の `GameSession` 状態のコピーであり、可変状態を露出しない(既存の設計を維持)。
- **ロジックの所在**: スナップショットの生成は `GameSession.Snapshot()`(Application 層)。プレゼンテーションは読むだけで、盤面計算・ゴースト計算をしない。

### 6.2 `BlockTypes → 色` マッピング(新規・プレゼンテーション定数)

WPF の色対応を Avalonia の色で踏襲する(等価な色名/値を用いる)。

| BlockTypes | 色(WPF 踏襲) |
| ---------- | -------------- |
| `I` | LightBlue |
| `O` | Yellow |
| `S` | Green |
| `Z` | Red |
| `J` | Blue |
| `L` | Orange |
| `T` | Purple |
| `nothing`/空 | DarkGray |

- **不変条件**: 全 `BlockTypes` に色が定義され、未定義入力は既定色(`DarkGray`)にフォールバックする(網羅・全域関数)。
- **ロジックの所在**: プレゼンテーション層の単一のマッピング関数/リソースに集約し、盤面・ホールド・ネクストで共有する。

### 6.3 表示用 ViewModel(新規・`CommunityToolkit.Mvvm`)

`ObservableObject` を継承し、`[ObservableProperty]` でビューにバインドする観測可能プロパティを公開する。最低限:

- 現在のスナップショット(盤面コントロールへ渡す)
- スコア表示(文字列)
- T-Spin/ライン消去の演出テキスト(`"T-Spin Mini!"`, `"T-Spin Single!"`, `"T-Spin Double!!"`, `"T-Spin Triple!!!"`, `"Tetris!!!"`、非該当時は空)
- ホールドブロック種別・ネクストブロック種別(小パネル描画用)
- 状態メッセージ(開始/一時停止/リセットの案内)とゲームオーバー/一時停止フラグ

- **不変条件**: ViewModel はゲームルールを持たず、`GameSession`/`GameStateSnapshot` の値を表示形へ変換するのみ(貧血にならない範囲での表示ロジックに限る)。
- **ロジックの所在**: ゲーム進行・スコアリングは Application 層。ViewModel は「表示への変換」と「入力→コマンド変換の仲介」に限る。

## 7. 振る舞い(受け入れ基準)

### Requirement 1: プロジェクト追加とビルド

**対象**: §5.1 `Tetris.Avalonia`

**受け入れ基準**:
1.1. `Tetris.Avalonia` は `net10.0` 単一 TFM の実行可能アプリとして構成されていなければならない。(常時)
1.2. `Tetris.Avalonia` は `Tetris.Application` と `Tetris.Infrastructure` のみをプロジェクト参照し、`TetrisWindow`(WPF)を参照してはならない。(常時)
1.3. `dotnet build Tetris.sln` を Linux 上で実行したとき、システムはエラーなくビルドを完了しなければならない。(イベント)
1.4. `Tetris.Avalonia` は Domain / Application / Infrastructure のソースを変更してはならない(公開 API の利用のみ)。(常時)

### Requirement 2: 合成ルートと DI/MVVM

**対象**: §5.3 合成ルート / §6.3 ViewModel

**受け入れ基準**:
2.1. アプリ起動時、システムは `Microsoft.Extensions.DependencyInjection` の DI コンテナで `IBlocksPoolManager → BlocksPoolManager` と `GameSession` を登録しなければならない。(イベント)
2.2. システムは、メインウィンドウとその ViewModel が単一の `GameSession` インスタンスを共有するように解決しなければならない。(常時)
2.3. ViewModel は `CommunityToolkit.Mvvm` の `ObservableObject` を継承し、ビューはコードビハインドの名前付きセル探索ではなくバインディング/描画で状態を反映しなければならない。(常時)
2.4. 未登録の依存を解決しようとした場合、システムは起動時に例外を送出し、無効状態で継続してはならない。(異常系)

### Requirement 3: スナップショット駆動の盤面描画

**対象**: §5.4 `BoardControl` / §6.1 `GameStateSnapshot`

**受け入れ基準**:
3.1. `BoardControl` は `Render(DrawingContext)` で 10×20 セルを直接描画し、名前付きセルや `FindName` を使用してはならない。(常時)
3.2. `Snapshot` が更新されたとき、システムは盤面を再描画しなければならない。(イベント)
3.3. システムは、フィールドの確定/移動中ブロックを §6.2 のマッピング色・不透明度 1.0 で、ゴーストブロックを現在ブロック色・不透明度 0.5 で描画しなければならない。(常時)
3.4. `Snapshot` が `null` の場合、システムは例外を投げず空盤面(背景色)を描画しなければならない。(異常系)
3.5. スナップショットのブロック点が盤面外を指す場合でも、システムは例外で停止せず盤面内のセルのみを描画しなければならない。(異常系)

### Requirement 4: ジッタ非依存のゲームループ

**対象**: §5.6 ゲームループ

**受け入れ基準**:
4.1. システムは `Avalonia.Threading.DispatcherTimer`(間隔約 16ms)でループを駆動しなければならない。(常時)
4.2. 各 tick で、システムは `Stopwatch` により前 tick からの実経過時間 `delta` を測定し、進行中は `GameSession.Advance(delta)` に渡さなければならない。(イベント)
4.3. タイマ間隔が公称値からゆらいだ場合でも、システムは実経過時間 `delta` を渡すことで落下速度を実時間基準で一定に保たなければならない(固定フレーム量で進めてはならない)。(異常系)
4.4. 一時停止中およびゲームオーバー中、システムは `Advance` を呼び出してはならない。(状態)
4.5. 一時停止から再開したとき、システムは一時停止中の経過時間を落下に持ち越さないよう `Stopwatch` を再基準化しなければならない。(イベント)

### Requirement 5: 入力(キーマッピング)

**対象**: §5.5 入力ハンドラ

**受け入れ基準**:
5.1. プレイ中、システムはキーを次に対応付けて `GameSession.Apply` を呼ばなければならない: `Up`→hardDrop、`Down`→moveDown、`Left`→moveLeft、`Right`→moveRight、`Z`→rotateLeft、`X`→rotateRight、`Space`→hold。(イベント)
5.2. ハードドロップ・回転・ホールドは 1 回の `KeyDown` につき 1 回だけ適用しなければならない。(常時)
5.3. マッピングに存在しないキーを押した場合、システムは何もしてはならない(状態を変えない)。(異常系)
5.4. プレイ中に `P` または `Esc` を押したとき、システムはゲームを一時停止しなければならない。(イベント)
5.5. 一時停止中、`Space` は再開、`R` はリセット(再スタート)、`Esc` は終了に対応付けなければならない。(イベント)
5.6. ゲームオーバー中、`Space` はレベル 1 での開始、`Esc` は終了に対応付けなければならない。(イベント)

### Requirement 6: ホールド/ネクスト/スコア/T-Spin 表示

**対象**: §6.3 ViewModel / §6.2 色マッピング

**受け入れ基準**:
6.1. システムは、スナップショットのスコアをスコア表示に反映しなければならない。(常時)
6.2. ホールド/ネクストのブロック種別が変化したとき、システムは対応する小パネルを §6.2 の色・WPF 踏襲のレイアウトで描画しなければならない。(イベント)
6.3. ライン消去/T-Spin が発生したとき、システムは対応する演出テキスト(`"T-Spin Mini!"`/`"T-Spin Single!"`/`"T-Spin Double!!"`/`"T-Spin Triple!!!"`/4 ライン時 `"Tetris!!!"`)を表示し、一定時間後に消去しなければならない。(イベント)
6.4. ホールドが未使用(`nothing`)の場合、システムはホールドパネルを空/非表示にしなければならない。(状態)

### Requirement 7: ゲームオーバー処理

**対象**: §5.6 ゲームループ / §6.3 ViewModel

**受け入れ基準**:
7.1. `GameSession.IsGameOver` が真になったとき、システムはループを停止しなければならない。(イベント)
7.2. ゲームオーバー時、システムは再開/操作案内メッセージを表示しなければならない。(イベント)
7.3. ゲームオーバー演出(盤面の塗り潰し等)は WPF の考え方を踏襲してよいが、その厳密なアニメーション/順序は合否判定の対象外とする(ベストエフォート)。(常時)

### Requirement 8: WPF 撤去とソリューション/README 更新

**対象**: §2 スコープ

**受け入れ基準**:
8.1. システムは `TetrisWindow`(WPF)プロジェクト一式を削除しなければならない。(イベント)
8.2. `Tetris.sln` から `TetrisWindow` を除去し、`Tetris.Avalonia` を追加しなければならない。(常時)
8.3. WPF 撤去後、`dotnet build Tetris.sln` は `windows` 専用 TFM を含まず Linux で成功しなければならない。(異常系/回帰)
8.4. `README.md` はクロスプラットフォームのビルド/実行手順(`dotnet run --project Tetris.Avalonia`)を含まなければならない。(常時)

### Requirement 9: 検証境界と回帰(非機能)

**対象**: §3 前提 / §2 対象外

**受け入れ基準**:
9.1. 本作業単位のローカル合否は「`Tetris.Avalonia` のビルド成功」と「Domain / Application / Infrastructure のテストが緑を維持」に限定しなければならない。(常時)
9.2. GUI の実プレイ(描画・キー操作・演出の実挙動)は機械検証の対象外とし、人間が `dotnet run` で目視確認する前提を spec と完了報告に明記しなければならない。(常時)
9.3. 既存ロジックテスト(Domain / Application / Infrastructure)は 1 件も失敗してはならない(挙動不変の回帰保証)。(異常系/回帰)
9.4. Avalonia の headless スモークテストを追加する場合でも、合否は「起動して例外なく 1 フレーム描画できる」程度に限り、実プレイ判定には用いてはならない。(状態)

## 8. 実現方針(要点のみ)

- **ゲーム進行は `GameSession` を DI 登録**(互換ファサード `GameManager` ではない)。理由は §3 の前提に記載(ジッタ非依存の重力 `Advance(delta)` と時間非依存の入力 `Apply(command)` の両立)。互換ファサード `GameManager` は WPF 向けの薄い互換層であり、本段の Avalonia では新しい入力/ループ設計に合う `GameSession` を直接使う。
- **層の依存方向**: `Tetris.Avalonia`(プレゼンテーション)→ `Tetris.Application` → `Tetris.Domain`、および `Tetris.Infrastructure` → `Tetris.Application`/`Tetris.Domain`。Domain/Application/Infrastructure は本段で不変。
- **描画**: `FindName` の名前付きセルを廃し、単一のカスタム `Control` が `GameStateSnapshot` を入力に 10×20 を `Render(DrawingContext)` で直接描く。ホールド/ネクストは同方式の小コントロールまたはバインド済みパネルで表現する。
- **入力/DAS**: `KeyDown` ごとにコマンドを 1 回適用。移動系の連続入力は OS キーリピートに委ねる(WPF の DAS フレーム周期は厳密再現しない、§3)。
- **色マッピング**は WPF の対応(§6.2)を Avalonia の色で踏襲し、単一箇所に集約する。
- **パッケージ版**: `Avalonia` 系・`CommunityToolkit.Mvvm`・`Microsoft.Extensions.DependencyInjection` の採用版は実装時に `net10.0` 対応の安定版を選び、§9 と README に記録する。

## 9. 参考資料

- 既存契約(参照のみ): `Tetris.Application/GameSession.cs`、`GameManager.cs`、`GameStateSnapshot.cs`、`GameCommand.cs`、`IBlocksPoolManager.cs`、`Tetris.Infrastructure/BlocksPoolManager.cs`、`Tetris.Domain/*`。
- 移植元(挙動・レイアウト・キー割当・色・演出の考え方の出典): `TetrisWindow/MainWindow.xaml.cs`、`TetrisWindow/MainWindow.xaml`。
- 採用パッケージのバージョン出典: 実装時に確定し本節へ追記する。
