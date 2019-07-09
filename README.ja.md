# micro:witch
micro:witch は、micro:bit のためのブロック型プログラミング環境です。（based on MIT Scratch）

![screenshot1](https://raw.githubusercontent.com/EiichiroIto/microwitch/master/src/images/screenshot1.png)

## インストール方法
1. 適当なフォルダにコピーしてください。

## 起動方法
1. Scratch.exe を実行してください。
1. 日本語を選んでください。

## 使い方
1. micro:bit をパソコンに接続します。
1. micro:bit が適当なドライブとして認識されるのを待ちます。
1. 「緑旗がクリックされたとき」で始まるプログラムを作ります。
1. デバイスメニューから「micro:bitに送信する」を選びます。
1. micro:bit でプログラムが実行されます。

## REPL実行
1. micro:bit をパソコンに接続します。
1. デバイスメニューから「micro:bitに接続する」を選びます。
1. micro:bitが接続されているポートを選びます。
1. 「接続」が「受付中」に変わるまで数秒程度待ちます。
1. ブロックや作成したスクリプトをクリックすると、micro:bit に転送され実行されます。
1. 接続中は「micro:bitに送信する」で高速にプログラムを転送できます。

## トラブルシューティング
### REPL実行でうまく接続や転送ができない
micro:bit ファームウェアのバージョンが古い可能性があります。下記サイトを参考にファームウェアをバージョンアップしてみてください。

[micro:bitファームウェアをアップデートする](https://microbit.org/ja/guide/firmware/)

### REPL実行で作成したプログラムが、起動時・リセット時に実行されない
1. REPL実行中ならば、デバイスメニューから「micro:bit を切断する」で接続を解除します。
1. ファイルメニューで新規を選び、プログラムが何もない状態にします。
1. デバイスメニューから「micro:bitに送信する」を選んでファームウェアを転送します。
1. 上の作業を一度行った後で、再度REPL実行を行ってください。

## ライセンス
MIT License
