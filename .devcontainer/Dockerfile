# 言語非依存の汎用ベース（devcontainer 公式 base イメージ）。
# .NET / gh は devcontainer feature で導入する。
FROM mcr.microsoft.com/devcontainers/base:ubuntu

# Claude Code / gh の設定永続化先（名前付きボリュームの所有者を vscode に合わせる）
RUN mkdir -p /home/vscode/.claude /home/vscode/.config/gh \
    && chown -R vscode:vscode /home/vscode/.claude /home/vscode/.config
