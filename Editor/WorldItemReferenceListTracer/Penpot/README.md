# Penpot 開発環境サポート
UI設計ツール Penpot の 起動方法などのメモ

## 前提条件

- [Podman](https://podman.io/) (Desktop または CLI)
- [Node.js](https://nodejs.org/) (v22.x 推奨)
- [pnpm](https://pnpm.io/) (MCP サーバのビルドに必要)

## Podman 環境の構築 (初回のみ)

Mac で Podman を使用する場合、以下の手順で仮想マシンを初期化して起動する必要があります。

```bash
# Podman マシンの初期化
podman machine init

# Podman マシンの起動
podman machine start
```

## Penpot の起動手順

ローカル環境で Penpot インスタンスを起動するには、以下のコマンドを実行します。

```bash
cd /Volumes/SSD/work/Penpot
podman compose -p penpot -f compose.yaml up
```

起動後、ブラウザで [http://localhost:9001](http://localhost:9001) にアクセスしてください。
