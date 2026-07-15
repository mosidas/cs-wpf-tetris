# tetris

クロスプラットフォーム(Windows / macOS / Linux)で動作するデスクトップ版テトリス。
UI は [Avalonia](https://avaloniaui.net/) 製。ゲームロジックはクリーンアーキテクチャで層分離している。

## 構成

| プロジェクト | 役割 |
| ------------ | ---- |
| `Tetris.Domain` | ドメイン(盤面・ブロック・座標・スコア規則)。フレームワーク非依存・時間概念なし。 |
| `Tetris.Application` | 進行ルール(`GameSession`)・コマンド・スコア・読み取りモデル(`GameStateSnapshot`)。 |
| `Tetris.Infrastructure` | `BlocksPoolManager`(7-bag)。 |
| `Tetris.Avalonia` | デスクトップ UI(Avalonia / MVVM / DI)。3 OS 対応。 |
| `*.Tests` | 各層および純粋マッパーの単体テスト(MSTest)。 |

## 必要環境

- .NET SDK 10.0 以上

## ビルド

```sh
dotnet build Tetris.sln
```

Windows / macOS / Linux のいずれでも同じコマンドでビルドできる(OS 専用 TFM に依存しない)。

## 実行

```sh
dotnet run --project Tetris.Avalonia
```

## テスト

```sh
dotnet test Tetris.sln
```

## 操作

| キー | 操作 |
| ---- | ---- |
| ↑ | ハードドロップ |
| ↓ / ← / → | 移動 |
| Z / X | 左回転 / 右回転 |
| Space | ホールド(プレイ中) / 開始・再開(ゲームオーバー・一時停止中) |
| P | 一時停止 |
| R | リセット(一時停止中) |
| Esc | 一時停止 → 終了 |

## 採用パッケージ(主要)

- Avalonia 12.1.0(`Avalonia` / `Avalonia.Desktop` / `Avalonia.Themes.Fluent`)
- CommunityToolkit.Mvvm 8.4.0
- Microsoft.Extensions.DependencyInjection 10.0.0

バージョンは [`Directory.Packages.props`](Directory.Packages.props) で一元管理している(Central Package Management)。
