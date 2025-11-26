# TsToMp4Dropper

TS ファイルをドラッグ＆ドロップするだけで、ffmpeg を使って同名の MP4 にコピー変換する Windows 用 GUI ツールです。WinForms (.NET 6) で動作します。

## 特徴
- ffmpeg.exe を同じフォルダに置くだけのシンプル構成
- `-c:v copy -c:a copy` による高速コピー変換（再エンコードなし）
- 変換ログをウィンドウのテキストボックスに逐次表示
- 配布物は self-contained ビルドのため、利用者は .NET ランタイム不要

## 使い方
1. ffmpeg をインストールします（下記参照）。
2. `TsToMp4Dropper.exe` と同じフォルダに `ffmpeg.exe` を置きます。
3. `TsToMp4Dropper.exe` を起動します。
4. エクスプローラーから `.ts` ファイルをこのウィンドウにドラッグ＆ドロップすると、同じフォルダに `同名.mp4` が出力されます。
5. ログに ffmpeg の出力と結果が表示されるので確認してください。

## ffmpeg について
このツールは動画変換処理に ffmpeg を利用します。ffmpeg 自体は本リポジトリには含まれていません。

- ffmpeg 公式サイト: https://ffmpeg.org/
- Windows ビルド例: https://www.gyan.dev/ffmpeg/builds/

ffmpeg のライセンス（LGPL/GPL）に従ってご利用ください。本ツールは ffmpeg を外部コマンドとして呼び出しており、ffmpeg の作者とは無関係の個人プロジェクトです。

## 変換コマンド
内部では次のコマンドで copy 変換を行います。

```
ffmpeg.exe -y -i "input.ts" -c:v copy -c:a copy "input.mp4"
```

## ビルド
```
dotnet build src/TsToMp4Dropper.sln -c Release
```
生成物: `src/TsToMp4Dropper/bin/Release/net6.0-windows/`

## 前提
- Windows 環境
- 利用者は .NET ランタイム不要（self-contained で配布するため）
- 開発時: .NET 6 SDK が必要
- ffmpeg.exe（外部で入手し、アプリと同じフォルダに配置）

## ライセンスと同梱ポリシー
- このアプリは MIT License（`LICENSE`）で配布します。
- ffmpeg は本リポジトリ・配布物に同梱しません。利用者が外部サイトから取得し、そのライセンス (LGPL/GPL) に従って利用してください。
