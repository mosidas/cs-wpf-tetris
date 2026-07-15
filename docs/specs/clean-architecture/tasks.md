# clean-architecture — 実装タスク

> 仕様の詳細は同じディレクトリの仕様文書 spec.md を参照する。
> このファイルには仕様を転記しない(要件 ID・節への参照で解決する)。

## 検証境界(全タスク共通・厳守)

- WPF(`TetrisWindow`, `net10.0-windows`)は Linux コンテナでビルド不可。**`dotnet build Tetris.sln`(sln 全体)は使わない**(WPF で失敗するため)。
- ローカル検証は非 WPF プロジェクトを**個別に** `dotnet build` / `dotnet test` する(`Tetris.Domain` / `Tetris.Application` / `Tetris.Infrastructure` と各テストプロジェクト)。
- WPF の健全性は GitHub の Windows CI(`.github/workflows/ci.yml`, sln 全体ビルド/テスト)が担保する。`TetrisWindow` はコード追従のみ(ローカル検証対象外)。
- 中間期間は緑維持のため旧 `TetrisLogic`(製品)・旧 `TetrisLogicTests`(テスト)を残して並行構築し、**最終タスク(8)で両方を対称に撤去**して全体回帰する。

## File Structure Plan

| ファイルパス | 区分 | 責務 |
| ------------ | ---- | ---- |
| `Directory.Packages.props` | 新規 | 中央パッケージ管理(テスト系パッケージのバージョン一元化) |
| `Directory.Build.props` | 新規 | 共通ビルド設定(`net10.0`・`Nullable=enable`。WPF は自 csproj で `net10.0-windows`/`UseWPF` を上書き) |
| `Tetris.sln` | 変更 | 新規 6 プロジェクト追加 → 最終タスクで旧 2 プロジェクト除去 |
| `Tetris.Domain/Tetris.Domain.csproj` | 新規 | Domain 層(フレームワーク非依存・時間概念なし) |
| `Tetris.Domain/Position.cs` | 新規 | `readonly record struct Position(int X, int Y)`(§5.1) |
| `Tetris.Domain/Enums/*.cs` | 新規 | `BlockTypes`/`DirectionTypes`/`TSpinTypes`/`FieldTypes`(定義変更なし移設) |
| `Tetris.Domain/Block.cs` | 新規 | 旧 `Block` を移設し座標型を `Position` へ置換(挙動不変) |
| `Tetris.Domain/Field.cs` | 新規 | 旧 `Field` を移設し座標型を `Position` へ置換(挙動不変) |
| `Tetris.Domain/ScoreRule.cs` | 新規 | 純粋スコア規則(旧係数表を式変更なし移植)(§5.3) |
| `Tetris.Application/Tetris.Application.csproj` | 新規 | Application 層(進行ルール・コマンド・ファサード) |
| `Tetris.Application/ScoreManager.cs` | 新規 | 実行時スコア累積(`ScoreRule` へ委譲)(§5.3) |
| `Tetris.Application/GameCommand.cs` | 新規 | `enum GameCommand`(§5.4) |
| `Tetris.Application/IGameCommand.cs` | 新規 | `CanExecute/Execute(GameSession)`(§5.4) |
| `Tetris.Application/Commands/*.cs` | 新規 | 8 コマンド(Move/Rotate/Hold/HardDrop/Nothing)。SRS/T-Spin/Hold を式変更なし移植 |
| `Tetris.Application/GameCommandFactory.cs` | 新規 | `GameCommand→IGameCommand`(internal、非公開) |
| `Tetris.Application/IBlocksPoolManager.cs` | 新規 | プール抽象(§5.5) |
| `Tetris.Application/GameSession.cs` | 新規 | 可変状態 + 進行ルール + 重力アキュムレータ(§5.6) |
| `Tetris.Application/GameStateSnapshot.cs` | 新規 | 読み取り専用 read model(§5.7) |
| `Tetris.Application/ActionTypes.cs` | 新規 | 旧 `ActionTypes` enum を維持(ファサード互換用) |
| `Tetris.Application/GameManager.cs` | 新規 | 互換ファサード(`Update`/`Tick`/`Snapshot`)(§5.8) |
| `Tetris.Infrastructure/Tetris.Infrastructure.csproj` | 新規 | Infrastructure 層 |
| `Tetris.Infrastructure/BlocksPoolManager.cs` | 新規 | 7-bag 実装(旧実装を式変更なし移植)(§5.5) |
| `Tetris.Domain.Tests/Tetris.Domain.Tests.csproj` | 新規 | Domain テストプロジェクト |
| `Tetris.Domain.Tests/BlockTests.cs`, `FieldTests.cs` | 新規 | 旧 `GameObject/*Tests` を `Position`・新 namespace へ移行 |
| `Tetris.Application.Tests/Tetris.Application.Tests.csproj` | 新規 | Application テストプロジェクト |
| `Tetris.Application.Tests/Commands/*.cs` | 新規 | 旧 `UserAction/UA_*Tests` を新コマンド駆動へ移行(壁蹴り回帰含む) |
| `Tetris.Application.Tests/ScoreManagerTests.cs` | 新規 | 旧 `ScoreManagerTests` を移行 |
| `Tetris.Application.Tests/GameSessionTests.cs` | 新規 | GameSession(進行・タイミング)テスト |
| `Tetris.Application.Tests/GameManagerTests.cs` | 新規 | 旧 `GameManagerTests` を互換ファサードへ移行 |
| `Tetris.Application.Tests/Stub/*.cs` | 新規 | 旧 `Stub/BlocksPoolManagerDummy` 相当のテスト用スタブ |
| `Tetris.Infrastructure.Tests/Tetris.Infrastructure.Tests.csproj` | 新規 | Infrastructure テストプロジェクト |
| `Tetris.Infrastructure.Tests/BlocksPoolManagerTests.cs` | 新規 | 旧 `BlocksPoolManagerTests` を移行 |
| `TetrisWindow/TetrisWindow.csproj` | 変更 | `ProjectReference` を `TetrisLogic`→`Tetris.Application` へ |
| `TetrisWindow/MainWindow.xaml.cs` | 変更 | 互換ファサード API へ再接続・`Point`→`Position`(コード追従のみ / Windows CI で検証) |
| `.github/workflows/ci.yml` | 変更(必要時) | 新プロジェクト構成で sln 全体ビルド/テストが通ることの確認・調整 |
| `TetrisLogic/**`(プロジェクト一式) | 削除 | 3 層へ移設後、使用ゼロ確認を経て最終タスクで撤去 |
| `TetrisLogicTests/**`(プロジェクト一式) | 削除 | 3 層テストへ移行後、最終タスクで撤去 |

## タスク一覧

- [x] 1. 層スキャフォールドと中央パッケージ管理
  - [x] 1.1 3 製品プロジェクト + 3 テストプロジェクトの雛形を作成し sln に登録する
        _Requirements: 1.1, 1.2, 1.5, 10.2_
        _Boundary: BuildConfig_
    - 対象ファイル: `Tetris.Domain/Tetris.Domain.csproj`, `Tetris.Application/Tetris.Application.csproj`, `Tetris.Infrastructure/Tetris.Infrastructure.csproj`, `Tetris.Domain.Tests/*.csproj`, `Tetris.Application.Tests/*.csproj`, `Tetris.Infrastructure.Tests/*.csproj`(いずれも新規), `Tetris.sln`(変更)
    - 内容: 参照方向を `Application→Domain` / `Infrastructure→Application,Domain` / 各 `*.Tests→対応層` に設定(逆参照なし)。旧 `TetrisLogic`/`TetrisLogicTests` は残す。
    - 仕様参照: spec.md §5・§6(依存方向)・§8(層とパッケージ)
    - 検証コマンド: `dotnet build Tetris.Domain/Tetris.Domain.csproj && dotnet build Tetris.Application/Tetris.Application.csproj && dotnet build Tetris.Infrastructure/Tetris.Infrastructure.csproj`(各テスト空ビルドも同様に個別実行)
  - [x] 1.2 中央パッケージ管理と共通ビルド設定を導入する
        _Requirements: 1.3, 1.4_
        _Boundary: BuildConfig_
        _Depends: 1.1_
    - 対象ファイル: `Directory.Packages.props`(新規), `Directory.Build.props`(新規)
    - 内容: テスト系パッケージのバージョンを `Directory.Packages.props` に集約。共通 `TargetFramework=net10.0`・`Nullable=enable` を `Directory.Build.props` に。WPF は自 csproj で `net10.0-windows`/`UseWPF` を上書きし、Domain は `System.Drawing`/WPF/時間 API を参照しない構成にする。
    - 仕様参照: spec.md §7 Req1(1.3, 1.4)・§8
    - 検証コマンド: `dotnet build Tetris.Domain/Tetris.Domain.csproj`(Domain が `System.Drawing` 非依存でビルドできること)
  - [x] 1.3 CI が新構成の sln 全体をビルド/テストできることを確認・調整する
        _Requirements: 1.6, 10.4_
        _Boundary: BuildConfig_
        _Depends: 1.1_
    - 対象ファイル: `.github/workflows/ci.yml`(必要時変更)
    - 内容: Windows CI が新規 6 プロジェクトを含む sln 全体をビルド/テストする(ローカルでは実行しない。push/PR で担保)。
    - 仕様参照: spec.md §7 Req1(1.6)・Req10(10.4)
    - 検証コマンド: (ローカル不可)`.github/workflows/ci.yml` の対象が sln 全体であることを目視確認。実ビルド/テストは push/PR 後の Windows CI に委譲。

- [x] 2. Domain 移設と Position 全廃(新コード)
  - [x] 2.1 Position 値型と enum 群を Domain に定義する
        _Requirements: 2.1, 2.3, 3.1_
        _Boundary: Domain_
        _Depends: 1.1_
    - 対象ファイル: `Tetris.Domain/Position.cs`, `Tetris.Domain/Enums/*.cs`(新規)
    - 仕様参照: spec.md §5.1・§5.2(enum 群)
    - 検証コマンド: `dotnet build Tetris.Domain/Tetris.Domain.csproj`
  - [x] 2.2 Block を Domain へ移設し座標を Position へ置換する(挙動不変)
        _Requirements: 2.2, 3.1, 3.2, 3.4_
        _Boundary: Domain_
        _Depends: 2.1_
    - 対象ファイル: `Tetris.Domain/Block.cs`(新規), `Tetris.Domain.Tests/BlockTests.cs`(旧 `GameObject/BlockTests.cs` を移行)
    - 内容: 公開メンバは現行同一。座標返却は `List<Position>`。回転結果(`DrawBlock`)・座標集合・`GetTSpinPoints` を式変更なしで維持。`System.Drawing.Point` を残さない。
    - 仕様参照: spec.md §5.2 Block・§7 Req3(3.2, 3.4)
    - 検証コマンド: `dotnet test Tetris.Domain.Tests/Tetris.Domain.Tests.csproj`
  - [x] 2.3 Field を Domain へ移設し座標を Position へ置換する(挙動不変)
        _Requirements: 2.2, 3.1, 3.3, 3.4_
        _Boundary: Domain_
        _Depends: 2.1_
    - 対象ファイル: `Tetris.Domain/Field.cs`(新規), `Tetris.Domain.Tests/FieldTests.cs`(旧 `GameObject/FieldTests.cs` を移行)
    - 内容: 10×20 固定。`UpdateField` のライン消去数・フィールド状態、衝突判定を式変更なしで維持。座標返却を `Position` 化。
    - 仕様参照: spec.md §5.2 Field・§7 Req3(3.3, 3.4)
    - 検証コマンド: `dotnet test Tetris.Domain.Tests/Tetris.Domain.Tests.csproj`

- [x] 3. スコア規則(Domain)と累積(Application)
  - [x] 3.1 ScoreRule を Domain の純粋関数として移植する
        _Requirements: 4.1, 4.4_
        _Boundary: Domain_
        _Depends: 2.1_
    - 対象ファイル: `Tetris.Domain/ScoreRule.cs`(新規)
    - 内容: 旧 `ScoreManager` の係数表(base/additional 分岐)を式・数値変更なしで `ScoreRule.Calculate(line, tSpin, ren, btb, allClear)` へ。時間・可変状態・フレームワーク非依存。
    - 仕様参照: spec.md §5.3 ScoreRule・§7 Req4(4.1, 4.4)
    - 検証コマンド: `dotnet build Tetris.Domain/Tetris.Domain.csproj`
  - [x] 3.2 ScoreManager を Application に置き累積・Reset をテストで検証する
        _Requirements: 4.2, 4.3_
        _Boundary: Application_
        _Depends: 3.1_
    - 対象ファイル: `Tetris.Application/ScoreManager.cs`(新規), `Tetris.Application.Tests/ScoreManagerTests.cs`(旧 `ScoreManagerTests` を移行・原文アサーション維持)
    - 内容: `Add` は `ScoreRule` で計算し `Score` を加点ぶん増加、`Reset` で 0 復帰。旧 24 件のスコアテストを新 API・namespace へ機械移行。
    - 仕様参照: spec.md §5.3 ScoreManager・§7 Req4(4.2, 4.3)
    - 検証コマンド: `dotnet test Tetris.Application.Tests/Tetris.Application.Tests.csproj`

- [x] 4. コマンド化(ref 廃止)と SRS・T-Spin・Hold の移植
  - [x] 4.1 GameSession 可変状態コンテナとコマンド契約を定義する
        _Requirements: 5.1, 6.6_
        _Boundary: Application_
        _Depends: 2.2, 2.3_
    - 対象ファイル: `Tetris.Application/GameCommand.cs`, `Tetris.Application/IGameCommand.cs`, `Tetris.Application/GameSession.cs`(可変状態 `Field`/`CurrentBlock`/`HoldBlock` + ctor のみ。進行は Task 5 で追加)(新規)
    - 内容: `ref` 渡しを用いない `IGameCommand(CanExecute/Execute(GameSession))`。コマンド・テストが直接駆動できる可視性で可変状態を公開。
    - 仕様参照: spec.md §5.4・§5.6(可変状態)・§7 Req5(5.1)
    - 検証コマンド: `dotnet build Tetris.Application/Tetris.Application.csproj`
  - [x] 4.2 移動系・HardDrop・Nothing コマンドを移植しテスト移行する
        _Requirements: 5.6_
        _Boundary: Application_
        _Depends: 4.1_
    - 対象ファイル: `Tetris.Application/Commands/{MoveLeft,MoveRight,MoveDown,HardDrop,Nothing}Command.cs`, `Tetris.Application/GameCommandFactory.cs`(internal), `Tetris.Application.Tests/Commands/{Move*,HardDrop}Tests.cs`(旧 `UA_Move*`/`UA_HardDrop`Tests を移行)
    - 内容: 旧 `UA_*` の規則を式変更なし移植。Factory は internal(公開 API に露出しない)。旧 `UserActionFactoryTests`(8 件)の意図は Factory 内部化に伴い `InternalsVisibleTo` かファサード写像テスト(Task 7)へ振り替える。
    - 仕様参照: spec.md §5.4・§7 Req5(5.6)
    - 検証コマンド: `dotnet test Tetris.Application.Tests/Tetris.Application.Tests.csproj`
  - [x] 4.3 回転コマンドの SRS 壁蹴りと T-Spin 判定を移植し壁蹴り回帰を移行する
        _Requirements: 5.2, 5.3_
        _Boundary: Application_
        _Depends: 4.1_
    - 対象ファイル: `Tetris.Application/Commands/{RotateLeft,RotateRight}Command.cs`, `Tetris.Application.Tests/Commands/{RotateLeft,RotateRight}Tests.cs`(旧 `UA_Rotate*Tests` を移行)
    - 内容: SRS 壁蹴り補正(通常 4 状態・I 専用 4 状態)を式・オフセット値変更なしで移植。T-Spin 判定(`GetTSpinPoints` 衝突数 ≥3、`count==3 && MoveY==0`→tMini、他→tSpin、<3→notTSpin)を旧式と同値で。壁蹴り後座標・`DrawBlock` を原文期待値で検証。
    - 仕様参照: spec.md §5.4・§7 Req5(5.2, 5.3)・Req10(10.3)
    - 検証コマンド: `dotnet test Tetris.Application.Tests/Tetris.Application.Tests.csproj`
  - [x] 4.4 Hold コマンドの差し替え規則を移植しテスト移行する
        _Requirements: 5.4, 5.5_
        _Boundary: Application_
        _Depends: 4.1_
    - 対象ファイル: `Tetris.Application/Commands/HoldCommand.cs`, `Tetris.Application.Tests/Commands/HoldTests.cs`(旧 `UA_HoldTests` を移行)
    - 内容: `HoldBlock` が nothing なら退避のみ・`CurrentBlock` を nothing に、そうでなければ交換し退避側に `CanSwap=false`。交換可否は nothing→真 / 他→`HoldBlock.CanSwap`。旧 `UA_Hold` と同値。
    - 仕様参照: spec.md §5.4・§7 Req5(5.4, 5.5)
    - 検証コマンド: `dotnet test Tetris.Application.Tests/Tetris.Application.Tests.csproj`

- [x] 5. GameSession 進行とタイミング分離
  - [x] 5.1 プール抽象と Start・Apply(進行ルール)を実装する
        _Requirements: 6.1, 6.2, 6.3, 6.4, 6.5_
        _Boundary: Application_
        _Depends: 3.2, 4.2, 4.3, 4.4_
    - 対象ファイル: `Tetris.Application/IBlocksPoolManager.cs`(新規), `Tetris.Application/GameSession.cs`(変更: Start/Apply/進行状態), `Tetris.Application.Tests/Stub/BlocksPoolManagerDummy.cs`(新規), `Tetris.Application.Tests/GameSessionTests.cs`(新規)
    - 内容: `Start(level)` は 0–99 クランプ・初期化・プールリセット・初手取得・スコアリセット。`Apply` は旧 `UpdateGameState` と同一(固定判定・ライン消去・REN/B2B・スコア加算・次ブロック生成・レベルアップ・Hold 差し替え)。スポーン不可で `IsGameOver=true`、`IsGameOver` 真なら `Apply`/`Advance` は状態不変。内部に FPS/フレームカウンタを持たない。
    - 仕様参照: spec.md §5.6・§7 Req6(6.1–6.5)
    - 検証コマンド: `dotnet test Tetris.Application.Tests/Tetris.Application.Tests.csproj`
  - [x] 5.2 Advance(重力アキュムレータ)と等価換算を実装する
        _Requirements: 6.6, 7.1, 7.2, 7.3, 7.4_
        _Boundary: Application_
        _Depends: 5.1_
    - 対象ファイル: `Tetris.Application/GameSession.cs`(変更: Advance), `Tetris.Application.Tests/GameSessionTests.cs`(変更: タイミングテスト追加)
    - 内容: レベル L の落下間隔を `max(1, 60-L*5)` フレーム × `(1000/60)` ms で保持(旧 `DownRate×FrameRate` 等価)。`level==0` は自然落下なし。累積が閾値到達ごとに 1 セル落下(移動/固定)。閾値未満 `delta` の複数回は累積保持(取りこぼし・多重発火なし)。
    - 仕様参照: spec.md §5.6 Advance・§7 Req6(6.6)・Req7(7.1–7.4)
    - 検証コマンド: `dotnet test Tetris.Application.Tests/Tetris.Application.Tests.csproj`

- [x] 6. Infrastructure(7-bag)
  - [x] 6.1 BlocksPoolManager(7-bag)を Infrastructure に移植しテスト移行する
        _Requirements: 9.1, 9.2, 9.3_
        _Boundary: Infrastructure_
        _Depends: 2.2, 5.1_
    - 対象ファイル: `Tetris.Infrastructure/BlocksPoolManager.cs`(新規), `Tetris.Infrastructure.Tests/BlocksPoolManagerTests.cs`(旧 `BlocksPoolManagerTests` を移行)
    - 内容: `IBlocksPoolManager`(Application)を Infrastructure が実装。7 種を 1 バッグ単位で乱択供給、プールが 6 個以下で次バッグ補充(旧 `Count > 6` return と等価)。`GetNextBlocksPool` は落下順。式・数値変更なし移植。
    - 仕様参照: spec.md §5.5・§7 Req9(9.1–9.3)
    - 検証コマンド: `dotnet test Tetris.Infrastructure.Tests/Tetris.Infrastructure.Tests.csproj`

- [x] 7. 読み取りモデル・互換ファサード・WPF 再接続
  - [x] 7.1 GameStateSnapshot と互換ファサード GameManager を実装する
        _Requirements: 8.1, 8.2, 8.3, 8.4_
        _Boundary: Application_
        _Depends: 5.2_
    - 対象ファイル: `Tetris.Application/GameStateSnapshot.cs`, `Tetris.Application/ActionTypes.cs`, `Tetris.Application/GameManager.cs`(新規), `Tetris.Application.Tests/GameManagerTests.cs`(旧 `GameManagerTests` を移行)
    - 内容: `GameStateSnapshot`(read model, 可変状態を露出しない)。互換ファサードは旧公開契約(`Update(ActionTypes)`/`Start`/`FrameRate`/`GameLevel`/`DownRate`/各表示プロパティ/`IsGameOver`/`Score`/`TSpinType`/`Line`)を同名・同挙動で提供し、`Tick(TimeSpan)`/`Snapshot()` を新規追加。`Update` は 1 フレーム重力 + 入力連射制御(同一操作 25 回以降 3 回に 1 回受理)+ コマンド適用を旧 `Update` と同値に。旧 `UserActionFactoryTests` の写像意図はここ(`ActionTypes→GameCommand`)で担保する。
    - 仕様参照: spec.md §5.7・§5.8・§7 Req8(8.1–8.4)
    - 検証コマンド: `dotnet test Tetris.Application.Tests/Tetris.Application.Tests.csproj`
  - [x] 7.2 WPF(TetrisWindow)を互換ファサード API へ再接続する(コード追従のみ)
        _Requirements: 8.5_
        _Boundary: Presentation_
        _Depends: 7.1_
    - 対象ファイル: `TetrisWindow/TetrisWindow.csproj`(参照を `Tetris.Application` へ変更), `TetrisWindow/MainWindow.xaml.cs`(namespace・`Point`→`Position`・API 再接続)
    - 内容: キー操作・描画・スコア表示・ゲームオーバー演出の挙動を変えない。**Linux ではビルドせず、健全性は Windows CI で担保**(ローカル検証対象外)。
    - 仕様参照: spec.md §5.8・§7 Req8(8.5)
    - 検証コマンド: (ローカル不可)Windows CI に委譲。ローカルでは `Tetris.Application` 個別ビルドのみ確認。

- [x] 8. テスト 3 分割の完了・全体回帰・旧 2 プロジェクトの対称撤去
  - [x] 8.1 114 テスト移行の網羅を確認し残テストを配置する
        _Requirements: 10.1, 10.2, 10.3_
        _Boundary: TestMigration_
        _Depends: 2.2, 2.3, 3.2, 4.2, 4.3, 4.4, 5.2, 6.1, 7.1_
    - 対象ファイル: `Tetris.Domain.Tests/**`, `Tetris.Application.Tests/**`, `Tetris.Infrastructure.Tests/**`(未移行・残余テストの配置)
    - 内容: 旧 `TetrisLogicTests` の全テストが 3 層いずれかへ移行済みか監査(アサーション値・期待文字列を原文どおり維持)。Factory 内部化で行き場を失った旧 `UserActionFactoryTests` の扱いを確定(ファサード写像テスト or 破棄の判断を明記)。全 3 テストプロジェクトを Linux で個別 `dotnet test`。
    - 仕様参照: spec.md §7 Req10(10.1, 10.2, 10.3)
    - 検証コマンド: `dotnet test Tetris.Domain.Tests/Tetris.Domain.Tests.csproj && dotnet test Tetris.Application.Tests/Tetris.Application.Tests.csproj && dotnet test Tetris.Infrastructure.Tests/Tetris.Infrastructure.Tests.csproj`
  - [x] 8.2 Point 全廃と旧プロジェクト無参照を確認し旧 2 プロジェクトを撤去する
        _Requirements: 2.2, 10.1_
        _Boundary: TestMigration_
        _Depends: 7.2, 8.1_
    - 対象ファイル: `Tetris.sln`(変更: 旧 2 プロジェクト除去), `TetrisLogic/**`(削除), `TetrisLogicTests/**`(削除)
    - 内容: `System.Drawing.Point` 参照ゼロを grep 確認(新コード・WPF・テスト全域)。`TetrisLogic`/`TetrisLogicTests` への `ProjectReference` がゼロであることを確認してから、**製品(旧 TetrisLogic)とテスト(旧 TetrisLogicTests)の両方を対称に削除**(片方だけ残さない)。sln から除去。
    - 仕様参照: spec.md §2・§7 Req2(2.2)・Req10(10.1)
    - 検証コマンド: `grep -rn "System.Drawing.Point" --include=*.cs .`(0 件)、非 WPF 3 製品 + 3 テストの個別 `dotnet build`/`dotnet test`
  - [x] 8.3 撤去後の全体回帰とドキュメント反映
        _Requirements: 10.1, 10.4_
        _Boundary: TestMigration_
        _Depends: 8.2_
    - 対象ファイル: `.github/workflows/ci.yml`(必要時: 旧プロジェクト除去に伴う調整), README/アーキテクチャ説明(存在時のみ更新)
    - 内容: 非 WPF の 3 製品 + 3 テストを個別に緑回帰(114 相当 全件 pass)。WPF 含む sln 全体は Windows CI が担保(push/PR 後)。層構成の変更をドキュメントに反映(該当ファイルが存在する場合のみ)。
    - 仕様参照: spec.md §7 Req10(10.1, 10.4)
    - 検証コマンド: 3 製品 + 3 テストの個別 `dotnet build`/`dotnet test`(sln 全体ビルドは使わない)

## Implementation Notes

- **UserActionFactoryTests の扱い(決定)**: `GameCommandFactory` は Req 5.6 に従い `internal` を維持。旧 `UserActionFactoryTests`(8 件)の「列挙値→コマンド型の写像」カバレッジを最も忠実に保つため、`Tetris.Application.csproj` に `InternalsVisibleTo(Tetris.Application.Tests)` を付与し、`Commands/GameCommandFactoryTests.cs` で `GameCommandFactory.Create` を直接検証する方式を採用(ファサード写像テストへの間接化より精度が高い)。公開 API には factory を露出しない。
- **検証境界**: 全タスクで非 WPF プロジェクトを個別に `dotnet build`/`dotnet test`。`dotnet build Tetris.sln` は不使用。
- **オーケストレーション**: 移植の忠実性のため実装はメイン文脈で直接行い(dev-implement 手動モード)、高リスク移植(Task 4 の SRS/T-Spin/Hold)には独立文脈の dev-reviewer を起動して敵対的検証。全タスク完了後に観点別レビューパネルで最終検証する。
