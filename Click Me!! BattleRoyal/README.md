# Click Me!! BattleRoyal - Created By QuestDragon
ゲームデザインを重視した進化版Click Me!!です。

C#のWPF（Windows Presentation Foundation）で作成されています。

ウィンドウサイズの変更、アニメーション、画面遷移、BGMやSEの実装、Discord Rich Presence対応など、今までのClick Me!!とは大きく異なる要素を持ちます。

中でも目玉は、ゲームタイトルにある「バトルロイヤル」モードの追加です。

## 遊び方
起動後、タイトルロゴが表示されるので、クリックするとメニューが表示されます。

| エンドレスモード | タイムアタックモード | トーナメントモード |
| :-------------: | :-------------: | :-------------: |
| 他のClick Me!!シリーズと同様、<br>おなじみのひたすらクリックし続けるモードです。 | こちらもClick Me!!シリーズで実装されていた、<br>時間制限付きのモードです。 | 本アプリケーションにて新たに実装されたモードです。<br>7人のコンピューターと対戦して優勝を目指すモードです。 |

「終了」ボタンを押してダイアログで「はい」を選択するとアプリケーションを終了できます。

「タイムアタックモード」または「トーナメントモード」を選択した場合は、制限時間を指定します。「戻る」ボタンで前の画面に戻ります。

ゲーム画面では、虹色で「Click Me!!」の文字がある四角形をクリックしてカウントします。できる限り多くクリックしましょう。なお、ボタンではなく図形なので、TAMAGOと同様キーボードの使用はこちらもできません。

ゲーム画面右下にBGMとSEの設定項目を用意しています。変更するとアプリケーションから再生される音声を変更できます。

「タイトルへ戻る」ボタンを押すとタイトル画面に戻ります。

「タイムアタックモード」では制限時間に達するとゲームが終了し、結果を表示してタイトル画面に戻ります。

「トーナメントモード」では制限時間に達すると他のコンピューターとのスコアを比較し、クリックした回数に応じて準決勝、決勝線に進出します。目指すは優勝です。

## ベータ版
Click Me!! BattleRoyal のみ、ベータ版を用意しています。

ベータ版では次の機能が追加されます。

- マルチプレイ
- Material Design
- 設定画面

ただし、ベータ版は安定版よりも機能が多い分、動作が不安定な場合があります。コードが不完全であるため、ベータ版のコードは公開しておりません。

安定した動作でのプレイをご希望の場合は、安定版をご使用ください。

## 配布形態
安定版は「[インストーラー](https://github.com/QuestDragon/Click-Me/releases/download/Click_Me_BR/ClickMeBR_Setup.zip)」、ベータ版は「[インストーラー](https://github.com/QuestDragon/Click-Me/releases/download/Click_Me_BR_BETA/ClickMeBR-beta_Setup.zip)」と「[zip圧縮によるポータブルパッケージファイル](https://github.com/QuestDragon/Click-Me/releases/download/Click_Me_BR_BETA/ClickMeBR_Beta.zip)」にて配布しています。

## Click Me!! BattleRoyal 音声ファイル提供

- 魔王魂
- フリー効果音 On-Jin ～音人～
- BGM・ジングル・効果音のフリー素材｜OtoLogic
- DOVA-SYNDROME
- ニコニ・コモンズ