# avalonia-migration — 実装タスク

> 仕様の詳細は同じディレクトリの仕様文書 spec.md を参照する。
> このファイルには仕様を転記しない。要件 ID(例 `1.2`)は spec.md §7 を指す。

## 検証境界(全タスク共通・厳守)

- GUI の実プレイ(描画・キー操作・演出の実挙動)は機械検証不可(spec.md §3・§7 R9)。ローカル合否は **「Tetris.Avalonia のビルド成功」+「Domain/Application/Infrastructure(および新設 Tetris.Avalonia.Tests の純粋マッパー)テスト緑」** に限定する。
- WPF 撤去(タスク 5)**まで**は `dotnet build Tetris.sln` が WPF(`net10.0-windows`)により Linux で失敗するため、タスク 1〜4 の検証は**個別プロジェクト**(`dotnet build Tetris.Avalonia` / `dotnet test <各テストプロジェクト>`)で行う。WPF 撤去**後**は `dotnet build Tetris.sln` 全体が Linux で成功する。
- 静的に二値判定できる項目(WPF 不参照・`FindName` 不使用・キー対応表・色マッピング・WPF プロジェクト削除・sln/README 更新)はビルド/テスト/grep で確認する。

## File Structure Plan

| ファイルパス | 区分 | 責務 |
| ------------ | ---- | ---- |
| `Tetris.Avalonia/Tetris.Avalonia.csproj` | 新規 | `net10.0` 単一 TFM の Exe。Avalonia 系 + CommunityToolkit.Mvvm + Microsoft.Extensions.DependencyInjection を参照。Application/Infrastructure をプロジェクト参照(WPF 不参照)。 |
| `Tetris.Avalonia/Program.cs` | 新規 | エントリポイント。`BuildAvaloniaApp()` とデスクトップ起動。 |
| `Tetris.Avalonia/App.axaml` | 新規 | `Application` 定義。Fluent テーマ適用。 |
| `Tetris.Avalonia/App.axaml.cs` | 新規 | DI 構築を呼び、解決した `MainWindow` を起動。 |
| `Tetris.Avalonia/Composition/ServiceRegistration.cs` | 新規 | 合成ルート。`IBlocksPoolManager→BlocksPoolManager`・`Field`・`GameSession`・`MainViewModel`・`MainWindow` を登録(単一 `GameSession` 共有)。 |
| `Tetris.Avalonia/ViewModels/MainViewModel.cs` | 新規 | `ObservableObject`。スナップショット・スコア・演出テキスト・ホールド/ネクスト種別・状態メッセージ/フラグを公開。 |
| `Tetris.Avalonia/Views/MainWindow.axaml` | 新規 | 最小シェルを 1.1 で作成し、3.3/4.x で盤面/ホールド/ネクスト/スコア/演出テキスト/メッセージのレイアウトを追加。 |
| `Tetris.Avalonia/Views/MainWindow.axaml.cs` | 新規 | 最小シェルを 1.1 で作成し、3.3 で `KeyDown`/`KeyUp` 受信・ゲームループ配線・ポーズ/リセット/終了の制御を追加。 |
| `Tetris.Avalonia/Controls/BoardControl.cs` | 新規 | `Control` 継承。`Render(DrawingContext)` で 10×20 を Snapshot 駆動で直接描画(`FindName` 不使用)。 |
| `Tetris.Avalonia/Controls/BlockPreviewControl.cs` | 新規 | ホールド/ネクストの小パネル。ブロック種別を色付きで描画。 |
| `Tetris.Avalonia/Rendering/BlockColors.cs` | 新規 | `BlockTypes → 色` の純粋・全域マッピング(未定義は既定色)。 |
| `Tetris.Avalonia/Input/KeyMapper.cs` | 新規 | `Avalonia.Input.Key → GameCommand`/制御操作の純粋変換。 |
| `Tetris.Avalonia/Game/GameLoop.cs` | 新規 | `DispatcherTimer`(≈16ms)+ `Stopwatch` 差分を `GameSession.Advance` へ渡す進行制御。ポーズ/GameOver で停止、再開時に再基準化。 |
| `Tetris.Avalonia.Tests/Tetris.Avalonia.Tests.csproj` | 新規 | 純粋マッパーの単体テスト(表示なし)骨格。MSTest。**1.4 で先行生成**(2.1/3.1 が共有するため)。 |
| `Tetris.Avalonia.Tests/BlockColorsTests.cs` | 新規 | `BlockColors` の全域性・WPF 踏襲色の検証(2.1)。 |
| `Tetris.Avalonia.Tests/KeyMapperTests.cs` | 新規 | キー対応表(spec.md §5.5)の検証(3.1)。 |
| `Tetris.sln` | 変更 | `TetrisWindow` を除去、`Tetris.Avalonia`・`Tetris.Avalonia.Tests` を追加。 |
| `README.md` | 変更 | クロスプラットフォームのビルド/実行手順(`dotnet run --project Tetris.Avalonia`)を追記。 |
| `TetrisWindow/`(一式) | 削除 | WPF プレゼンテーション。使用ゼロ確認を経て削除。 |
| `Tetris.Application/GameManager.cs` | 削除 | WPF 向け互換ファサード。WPF 撤去で非テスト消費者ゼロ。使用ゼロ確認を経て削除。 |
| `Tetris.Application/ActionTypes.cs` | 削除 | `GameManager` 専用の入力列挙。`GameManager` 撤去に伴い孤立。 |
| `Tetris.Application.Tests/GameManagerTests.cs` | 削除 | 互換ファサード配線のテスト(7 件)。ファサード撤去に伴い対象消滅(ゲームロジック本体は GameSessionTests/Command テストが直接カバー)。 |

## タスク一覧

- [x] 1. Tetris.Avalonia プロジェクト追加と合成ルート(DI/MVVM)・起動骨格
  - [x] 1.1 プロジェクト骨格を作成しビルドを通す(リスク先行: net10.0 対応 Avalonia の入手可否をここで確認)
        _Requirements: 1.1, 1.2, 1.4_
        _Boundary: Tetris.Avalonia_
    - 対象ファイル: `Tetris.Avalonia/Tetris.Avalonia.csproj`, `Tetris.Avalonia/Program.cs`, `Tetris.Avalonia/App.axaml`(+`.cs`), `Tetris.Avalonia/Views/MainWindow.axaml`(+`.cs`)(いずれも新規・最小シェル)。最小の `MainWindow` で起動可能な状態にする(3.3/4.x が中身を追加)。
    - 仕様参照: spec.md §5.1, §8
    - 検証コマンド: `dotnet build Tetris.Avalonia`(成功)。`grep -L TetrisWindow Tetris.Avalonia/Tetris.Avalonia.csproj` で WPF 不参照を確認。net10.0 対応版が無い場合は TFM 調整を検討し停止・報告。
  - [x] 1.4 Tetris.Avalonia.Tests(純粋マッパー用テストプロジェクト)骨格を作成
        _Requirements: 9.1_
        _Boundary: Tetris.Avalonia.Tests_
        _Depends: 1.1_
    - 対象ファイル: `Tetris.Avalonia.Tests/Tetris.Avalonia.Tests.csproj`(新規)。MSTest、`Tetris.Avalonia` を参照。テストクラスは 2.1/3.1 が各自の `*.cs` を追加する(SDK 暗黙 include のため csproj 共有による競合なし)。
    - 仕様参照: spec.md §7 R9.1
    - 検証コマンド: `dotnet test Tetris.Avalonia.Tests`(0 件でも成功)
  - [x] 1.2 合成ルート(DI)を実装する
        _Requirements: 2.1, 2.2, 2.4_
        _Boundary: Tetris.Avalonia_
        _Depends: 1.1_
    - 対象ファイル: `Tetris.Avalonia/Composition/ServiceRegistration.cs`, `Tetris.Avalonia/App.axaml.cs`(新規)。`IBlocksPoolManager→BlocksPoolManager`・`Field`・`GameSession`・`MainViewModel`・`MainWindow` を登録し、Window と VM が単一 `GameSession` を共有。未登録依存は起動時例外。
    - 仕様参照: spec.md §5.3, §7 R2
    - 検証コマンド: `dotnet build Tetris.Avalonia`。DI 解決の疎通(可能なら軽い起動確認)。
  - [x] 1.3 MainViewModel の骨格(CommunityToolkit.Mvvm)
        _Requirements: 2.3_
        _Boundary: Tetris.Avalonia_
        _Depends: 1.1_
    - 対象ファイル: `Tetris.Avalonia/ViewModels/MainViewModel.cs`(新規)。`ObservableObject` 継承、`[ObservableProperty]` で Snapshot・スコア・演出テキスト・ホールド/ネクスト種別・状態メッセージ/フラグを公開(値は後続タスクで供給)。
    - 仕様参照: spec.md §6.3, §7 R2.3
    - 検証コマンド: `dotnet build Tetris.Avalonia`

- [x] 2. 盤面カスタム描画コントロール(Snapshot 駆動)と色マッピング
  - [x] 2.1 BlockColors 純粋マッピング + 単体テスト (P)
        _Requirements: 3.3, 6.2_
        _Boundary: Tetris.Avalonia.Rendering_
        _Depends: 1.4_
    - 対象ファイル: `Tetris.Avalonia/Rendering/BlockColors.cs`(新規), `Tetris.Avalonia.Tests/BlockColorsTests.cs`(新規)。全 `BlockTypes` に WPF 踏襲色を割当、未定義は既定色(DarkGray)にフォールバック。
    - 仕様参照: spec.md §6.2
    - 検証コマンド: `dotnet test Tetris.Avalonia.Tests`(全 BlockTypes の色・全域性が緑)
  - [x] 2.2 BoardControl(Render(DrawingContext) で 10×20 直接描画)
        _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5_
        _Boundary: Tetris.Avalonia.Controls_
        _Depends: 2.1, 1.3_
    - 対象ファイル: `Tetris.Avalonia/Controls/BoardControl.cs`(新規)。`StyledProperty<GameStateSnapshot?>`、更新で再描画、確定/移動ブロック=不透明度1.0・ゴースト=0.5、`null`→空盤面(例外なし)、盤面外セルは無視。`FindName`・名前付きセル不使用。
    - 仕様参照: spec.md §5.4, §7 R3
    - 検証コマンド: `dotnet build Tetris.Avalonia`。`grep -R "FindName" Tetris.Avalonia` が 0 件であること(R3.1)。

- [x] 3. ゲームループ(DispatcherTimer+Stopwatch→Advance)と入力マッピング
  - [x] 3.1 KeyMapper 純粋変換 + 単体テスト (P)
        _Requirements: 5.1, 5.3_
        _Boundary: Tetris.Avalonia.Input_
        _Depends: 1.4_
    - 対象ファイル: `Tetris.Avalonia/Input/KeyMapper.cs`(新規), `Tetris.Avalonia.Tests/KeyMapperTests.cs`(新規)。`Up→hardDrop, Down→moveDown, Left→moveLeft, Right→moveRight, Z→rotateLeft, X→rotateRight, Space→hold`、未定義キー→何もしない(nothing)。制御キー(P/Esc/R/Space の文脈別)の分類も公開。
    - 仕様参照: spec.md §5.5, §7 R5.1, R5.3
    - 検証コマンド: `dotnet test Tetris.Avalonia.Tests`(キー対応表が緑)
  - [x] 3.2 ゲームループ(GameLoop: DispatcherTimer + Stopwatch → GameSession.Advance)
        _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5_
        _Boundary: Tetris.Avalonia.Game_
        _Depends: 1.2, 1.3_
    - 対象ファイル: `Tetris.Avalonia/Game/GameLoop.cs`(新規)。間隔≈16ms、各 tick で Stopwatch 実経過 `delta` を測り進行中のみ `Advance(delta)`(固定フレーム量で進めない)、ポーズ/GameOver 中は Advance せず、再開時に再基準化。`Snapshot()` を VM/BoardControl へ反映。
    - 仕様参照: spec.md §5.6, §7 R4
    - 検証コマンド: `dotnet build Tetris.Avalonia`(ループ実挙動は人間の `dotnet run` 目視・機械検証外)
  - [x] 3.3 入力ハンドラ配線(KeyDown/KeyUp → Apply / ポーズ・リセット・終了)
        _Requirements: 5.2, 5.4, 5.5, 5.6_
        _Boundary: Tetris.Avalonia_
        _Depends: 3.1, 3.2_
    - 対象ファイル: `Tetris.Avalonia/Views/MainWindow.axaml`(変更), `Tetris.Avalonia/Views/MainWindow.axaml.cs`(変更。最小シェルは 1.1 が作成済み)。プレイ中は KeyMapper 経由で `GameSession.Apply`(hardDrop/回転/hold は KeyDown 1 回=1 適用)、`P`/`Esc`=一時停止、一時停止中 `Space`=再開/`R`=リセット/`Esc`=終了、GameOver 中 `Space`=開始/`Esc`=終了。移動連続入力は OS キーリピートに委任(spec.md §3)。
    - 仕様参照: spec.md §5.5, §7 R5.2, R5.4, R5.5, R5.6
    - 検証コマンド: `dotnet build Tetris.Avalonia`(操作実挙動は人間目視・機械検証外)

- [x] 4. ホールド/ネクスト/スコア/T-Spin/ゲームオーバー表示
  - [x] 4.1 ホールド/ネクスト小パネル(色・WPF 踏襲レイアウト)
        _Requirements: 6.2, 6.4_
        _Boundary: Tetris.Avalonia.Controls_
        _Depends: 2.1, 1.3_
    - 対象ファイル: `Tetris.Avalonia/Controls/BlockPreviewControl.cs`(新規), `Tetris.Avalonia/Views/MainWindow.axaml`(変更)。ブロック種別を BlockColors で描画、ホールド未使用(nothing)は空/非表示。
    - 仕様参照: spec.md §7 R6.2, R6.4
    - 検証コマンド: `dotnet build Tetris.Avalonia`
  - [x] 4.2 スコアと T-Spin/ライン演出テキスト
        _Requirements: 6.1, 6.3_
        _Boundary: Tetris.Avalonia_
        _Depends: 1.3_
    - 対象ファイル: `Tetris.Avalonia/ViewModels/MainViewModel.cs`(変更), `Tetris.Avalonia/Views/MainWindow.axaml`(変更)。スコア反映、演出文言(`T-Spin Mini!`/`T-Spin Single!`/`T-Spin Double!!`/`T-Spin Triple!!!`/4 ライン `Tetris!!!`)を表示し一定時間後に消去。
    - 仕様参照: spec.md §6.3, §7 R6.1, R6.3
    - 検証コマンド: `dotnet build Tetris.Avalonia`
  - [x] 4.3 ゲームオーバー処理・表示
        _Requirements: 7.1, 7.2, 7.3_
        _Boundary: Tetris.Avalonia_
        _Depends: 3.2, 1.3_
    - 対象ファイル: `Tetris.Avalonia/Game/GameLoop.cs`(変更), `Tetris.Avalonia/ViewModels/MainViewModel.cs`(変更), `Tetris.Avalonia/Views/MainWindow.axaml`(変更)。`IsGameOver` でループ停止、再開/操作案内メッセージ表示。塗り潰し等の演出は WPF の考え方を踏襲(ベストエフォート・合否対象外)。
    - 仕様参照: spec.md §7 R7
    - 検証コマンド: `dotnet build Tetris.Avalonia`

- [x] 5. WPF 撤去・GameManager ファサード撤去・sln/README 更新・全体回帰
  - [x] 5.1 WPF(TetrisWindow)一式を削除し sln を更新
        _Requirements: 8.1, 8.2_
        _Boundary: Solution_
        _Depends: 1.1_
    - 対象ファイル: `TetrisWindow/`(削除・一式), `Tetris.sln`(変更: `TetrisWindow` 除去、`Tetris.Avalonia`・`Tetris.Avalonia.Tests` 追加)。
    - 仕様参照: spec.md §7 R8.1, R8.2
    - 検証コマンド: `dotnet sln Tetris.sln list`(TetrisWindow が無く Tetris.Avalonia が有る)。`grep -R "TetrisWindow" Tetris.sln` が 0 件。
  - [x] 5.2 互換ファサード GameManager の使用ゼロ確認と撤去(ユーザーのタスク分解指示による認可済みデッドコード整理)
        _Requirements: 9.3_
        _Boundary: Tetris.Application_
        _Depends: 5.1_
    - 対象ファイル: `Tetris.Application/GameManager.cs`(削除), `Tetris.Application/ActionTypes.cs`(削除), `Tetris.Application.Tests/GameManagerTests.cs`(削除)。方針: WPF 撤去後、非テスト消費者がゼロであることを grep で確認してから削除する。本撤去は spec.md §3 の「GameManager ではなく GameSession を採用する判断」の帰結であり、ゲームロジック(GameSession/コマンド/スコア/7-bag)は不変・挙動不変(R1.4 の趣旨=ゲームロジックを変えないに抵触しない)。失う 7 件は互換ファサード配線のテストのみで、ロジック本体は GameSessionTests・Command テストが直接カバーする(→ R9.3 のロジックテスト緑を維持)。
    - 仕様参照: spec.md §3(GameManager ではなく GameSession を採用する判断), §7 R9.3、参考 §7 R1.4
    - 検証コマンド: `grep -Rn "GameManager\|ActionTypes" --include='*.cs' . | grep -v '/bin/\|/obj/'` が(コメント参照を除き)コード消費ゼロであることを削除前に確認。削除後 `dotnet test Tetris.Application.Tests` 緑。
  - [x] 5.3 README にクロスプラットフォーム手順を追記
        _Requirements: 8.4_
        _Boundary: Docs_
        _Depends: 5.1_
    - 対象ファイル: `README.md`(変更)。`dotnet run --project Tetris.Avalonia`、Windows/macOS/Linux でのビルド手順、採用パッケージ版を追記(spec.md §9 と整合)。
    - 仕様参照: spec.md §7 R8.4
    - 検証コマンド: `grep -q "dotnet run --project Tetris.Avalonia" README.md`
  - [x] 5.4 全体回帰(クロスプラットフォーム・ビルド + 全テスト緑)
        _Requirements: 1.3, 8.3, 9.1, 9.2, 9.3, 9.4_
        _Boundary: Solution_
        _Depends: 2.2, 3.3, 4.1, 4.2, 4.3, 5.1, 5.2, 5.3_
    - 対象ファイル: なし(検証タスク)。`net10.0-windows` を含まないことを確認。
    - 仕様参照: spec.md §7 R1.3, R8.3, R9
    - 検証コマンド: `dotnet build Tetris.sln`(Linux 成功)、`dotnet test`(Domain/Application/Infrastructure/Tetris.Avalonia.Tests 全緑)。GUI 実プレイは人間が `dotnet run --project Tetris.Avalonia` で目視確認する前提を完了報告に明記(R9.2)。任意の headless スモークを追加する場合も合否は「起動して例外なく1フレーム描画」に限定(R9.4)。

## Implementation Notes

- **[1.1] リスク先行ゲート = 通過**: net10.0 + Avalonia **12.1.0**(最新安定版)で `dotnet build Tetris.Avalonia` 成功。ライブラリ側/sln 全体の TFM 降格は不要。CommunityToolkit.Mvvm 8.4.0 / Microsoft.Extensions.DependencyInjection 10.0.0 も net10.0 で解決可。
- **[1.1] CPM(Central Package Management)**: 本リポジトリは `Directory.Packages.props` で版を一元管理。`PackageReference` に `Version` を書くと NU1008。新規パッケージ版は `Directory.Packages.props` の `PackageVersion` に追加する。
- **[1.1] TFM/Nullable は `Directory.Build.props`(net10.0)から継承**。各 csproj で再定義しない(WPF のみ net10.0-windows を自プロジェクトで上書き)。
- **[1.1] 名前空間衝突に注意**: プロジェクト名前空間 `Tetris.Avalonia` の外側 `Tetris` に `Tetris.Application` があるため、素の `Application` は型でなく名前空間に解決される(CS0118)。Avalonia の `Application` は `global::Avalonia.Application` で完全修飾する。同様に `Tetris.Application` 型を使う場面では `using Tetris.Application;` の可視化で足りるが、`Application` 単独名は避ける。
- **[1.1] OutputType**: spec §5.1 の「Exe」は Avalonia デスクトップ慣例に合わせ `WinExe` で実現(Windows でコンソール窓を出さない。3 OS で実行可能アプリという R1.1 の意図は充足)。挙動不変。
- **[2.2/4.1] コンパイル済みバインディング**: `AvaloniaUseCompiledBindingsByDefault=true`。XAML の Window に `x:DataType="vm:MainViewModel"` を付け、バインド対象を静的に検証。
- **[3.3] 離散キーの1押下1適用**: Avalonia の `KeyEventArgs` に IsRepeat が無いため、押下中キーの `HashSet` で OS リピートを抑止(Up/Z/X/Space)。移動系(Down/Left/Right)は抑止せず OS リピートで連続移動(DAS 相当、spec §3)。
- **[5.2] GameManager 撤去**: WPF 撤去後、非テスト消費者ゼロを grep で確認し `GameManager.cs`/`ActionTypes.cs`/`GameManagerTests.cs`(7 件)を撤去。Application.Tests は 108→101 件。ゲームロジックは GameSessionTests/Command テストが直接カバーし挙動不変。spec §2/§9 を撤去認可の記述へ更新済み。
- **[5.4] 最終回帰**: `dotnet build Tetris.sln`(Linux 成功・windows TFM 不含)、`dotnet test Tetris.sln` 全緑(Domain 14 / Application 101 / Infrastructure 5 / Tetris.Avalonia 29 = 149)。GUI 実プレイは人間が `dotnet run --project Tetris.Avalonia` で目視確認する前提(機械検証外)。
