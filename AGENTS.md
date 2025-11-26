TS → MP4 変換ツール開発・公開ガイドライン

1. プロジェクト概要

TS（MPEG-TS）ファイルを Windows 上でドラッグ＆ドロップするだけで
ffmpeg を使用して MP4（copy 変換）に変換する簡易 GUI アプリケーションを開発する。

開発言語: C#

フレームワーク: .NET 6 / .NET 8（WinForms）

主機能:

ウィンドウにドロップされた .ts ファイルを順に ffmpeg で MP4 に変換

ffmpeg.exe はアプリ本体と同じディレクトリに配置して呼び出す

Copy 変換 (-c:v copy -c:a copy) のため高速

2. 技術仕様（変換処理）

実行される ffmpeg コマンドは次の通り。

ffmpeg.exe -y -i "input.ts" -c:v copy -c:a copy "input.mp4"


-y: 上書き許可

映像・音声は再エンコードせずコピー

入力と同じディレクトリに同名 .mp4 を出力

3. GUI 構成

WinForms アプリ（Form1）で構成する。

AllowDrop = True

Form 全体で DragEnter / DragDrop を受ける

下部にログ用 TextBox（Multiline, ScrollBars.Vertical）

ffmpeg.exe の検出結果や変換ログを逐次表示

4. ソース構成（推奨）
TsToMp4Dropper/
├─ src/
│  └─ TsToMp4Dropper/     （C# WinForms プロジェクト）
├─ README.md
├─ LICENSE
└─ .gitignore

5. GitHub 公開ポリシー

本プロジェクトは次の方針で GitHub 公開する。

5.1 ソースコード公開

本リポジトリには 自作アプリのソースコードのみ を含める

ffmpeg のバイナリは リポジトリに含めない

5.2 Releases の扱い

GitHub Releases には以下を含める。

TsToMp4Dropper.exe（アプリ本体）

ffmpeg は 含めない

ユーザーが外部サイト（ffmpeg.org / gyan.dev 等）から
自身で ffmpeg.exe を入手し、同じフォルダに置く方式を採用する。

5.3 この方式の利点

ffmpeg の GPL/LGPL の再配布義務を回避できる

ユーザーは任意の ffmpeg ビルドを選択可能

自作アプリは MIT ライセンス等でクリーンに公開できる

6. ライセンス設計
6.1 自作アプリのライセンス

MIT License または BSD-3-Clause を推奨。

理由:

ソース再利用の自由度が高く、GitHub で一般的

ffmpeg とライセンスを混同しない

LICENSE ファイルに MIT 形式を配置する。

6.2 README で明記すべき内容

README には以下を明確に記載する。

本ツールは ffmpeg を外部コマンドとして使用

ffmpeg.exe は同梱せず、ユーザーが各自ダウンロード

ffmpeg のライセンス（LGPL/GPL）は ffmpeg プロジェクトに準拠すること

自作アプリは MIT ライセンスで独立運用される

記載例:

このアプリは ffmpeg を外部プロセスとして利用します。
ffmpeg は本リポジトリおよび配布バイナリには含まれていません。
利用者自身で ffmpeg をダウンロードし、そのライセンス (LGPL/GPL) に従ってご利用ください。

7. ffmpeg 同梱を行わない理由（整理）

Gyan.dev ビルドは --enable-gpl などを含むため GPL 扱い

GPL バイナリを同梱した場合、追加の義務（ライセンス文書の添付、ソースコード提供手段の提示）が必要

アプリが ffmpeg に直接リンクしていなくても、同梱すると再配布扱いになるため注意が必要

非同梱であれば自作アプリのライセンスに ffmpeg が干渉しない

8. 将来的なオプション
8.1 ffmpeg 同梱版を出したい場合の取り扱い

Releases にのみ ffmpeg 同梱 zip を置く場合は以下を同梱する。

LICENSE-ffmpeg.txt（ffmpeg の GPL 文書コピー）

ffmpeg の入手元情報（gyan.dev リンク等）

ソースコード提供手段（ビルド提供元リンク、または提供宣言）

ただし、同梱しない構成が最もシンプルで安全。

9. 今後の拡張案

出力フォルダの固定設定

GUI に進捗バー追加

DragDrop の並列処理化

設定ファイル（JSON）でカスタム ffmpeg パラメータ設定

GitHub Actions による自動ビルド・自動 Release

10. 実装プラン（進行中）
1) WinForms プロジェクト雛形 (.NET 8) を作成し、Form1 に AllowDrop 設定とログ TextBox（Multiline/Vertical Scrollbars）を配置する。
2) 起動時に実行ディレクトリ内の ffmpeg.exe を検出し、見つからない場合はログにエラーを出してドロップ処理を抑止する仕組みを入れる。
3) DragEnter / DragDrop で .ts ファイルを受け取り、Task 実行で UI をブロックしないようにしつつ、複数ファイルを順次処理するキューを実装する（重複実行防止フラグも用意）。
4) 各ファイルについて ffmpeg.exe -y -i "input.ts" -c:v copy -c:a copy "input.mp4" を ProcessStartInfo で起動し、標準出力/エラーを逐次 TextBox に追記する。終了コードを確認し成功/失敗を記録。
5) README に使い方（ffmpeg の置き場所、ドラッグ＆ドロップ操作）、ライセンスと非同梱方針を記載し、MIT LICENSE を配置する。
