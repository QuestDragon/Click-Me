using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.UI.Core;
using Control = System.Windows.Forms.Control;

namespace Click_Me_BattleRoyal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static int exiting = 0;
        public static bool navigate = false;
        static Storyboard end_fade_anim= new Storyboard();

        public MainWindow()
        {
            List<string> dlls = new List<string>
            {
                "Click Me!! BattleRoyal.dll",
                "ClickMe.DiscordRPC.dll",
                "Click Me!! BattleRoyal.dll",
                "ClickMe.logger.dll",
                "ClickMe.Sound.dll",
                "ClickMe.Temp.dll",
                "ClickMe.Tournament.dll",
                "DiscordRPC.dll",
                "Microsoft.Windows.SDK.NET.dll",
                "NAudio.Asio.dll",
                "NAudio.Core.dll",
                "NAudio.dll",
                "NAudio.Midi.dll",
                "NAudio.Wasapi.dll",
                "NAudio.WinForms.dll",
                "NAudio.WinMM.dll",
                "Newtonsoft.Json.dll",
                "WinRT.Runtime.dll"
            };

            var dll_OK = true;
            var dll_ver_OK = true;
            var missing_dll_name = "";
            var wrong_dll_ver_name = "";
            for(int i = 0;i < dlls.Count; i++)
            {
                if (System.IO.File.Exists(dlls[i]) == false) //見つからない場合
                {
                    dll_OK = false; //必要DLL不足のためチェックFalse
                    missing_dll_name = dlls[i];
                    Debug.WriteLine($"Missing dll file: {dlls[i]}");
                    break; //ループ終了
                }
                //FileVersionInfoオブジェクトを取得し、ファイルバージョンを取得 (自前のDLLのみ)
                if (i <= 6 && System.Diagnostics.FileVersionInfo.GetVersionInfo(dlls[i]).FileVersion != "1.0.1.0") //Stable用1.0.1.0以外の場合
                {
                    dll_OK = false; //必要DLL不足のためチェックFalse
                    wrong_dll_ver_name = dlls[i];
                    Debug.WriteLine($"Wrong version dll file: {dlls[i]}");
                    break; //ループ終了
                }

            }
            if(dll_OK && dll_ver_OK)
            {
                Debug.WriteLine($"DLL CHECK: OK");
                InitializeComponent(); //XAMLに記述したオブジェクトを取得
                NameScope.SetNameScope(this, new NameScope()); //ネームスコープ作成
                this.Title = "Click Me!! - Battle Royale";

#if DEBUG //#if DEBUG と書いたIF文にはDebug状態でのみ実行できるコードを書くことが可能。
                Debug.WriteLine("デバッグモードで実行しています");
                developer_debug();
#endif

                this.RegisterName("EndFX", EndFX);

                //終了用アニメーション
                DoubleAnimation end_fade = new DoubleAnimation();
                end_fade.Duration = new Duration(TimeSpan.FromSeconds(1)); //1秒
                end_fade.From = 0;
                end_fade.To = 1;
                end_fade.DecelerationRatio = 0.3;
                Storyboard.SetTargetName(end_fade, EndFX.Name);
                Storyboard.SetTargetProperty(end_fade, new PropertyPath(OpacityProperty));
                end_fade_anim.Children.Add(end_fade);

                mw(this); //コントロールオブジェクトを別の静的メソッドから参照させるため情報を渡す

                ClickMe.DiscordRPC.RP.Initialize(); //Discord RPCの初期化（これをしないと動かない）

                Uri startup = new Uri("/startup.xaml", UriKind.Relative); //ページのディレクトリを指定して情報を格納
                frame.Source = startup; //アクセス

            }
            else if (dll_OK != true)
            {
                //メッセージボックスを表示する
                System.Windows.MessageBox.Show($"必要ライブラリが不足しています。\n以下のライブラリが実行ファイルと同じディレクトリに存在するかご確認ください。\n\n・{missing_dll_name}", "Click Me!! - 起動できません", MessageBoxButton.OK, MessageBoxImage.Error);
                exiting = 1; //終了
                System.Windows.Application.Current.Shutdown();
            }
            else if(dll_ver_OK!= true)
            {
                //メッセージボックスを表示する
                System.Windows.MessageBox.Show($"ライブラリのバージョンが非対応です。\n以下のライブラリがStable版「ClickMe!! Battle Royale」に対応したバージョン(v1.0.0.0)かご確認ください。\n\n・{wrong_dll_ver_name}\n\n※Stable版に同梱されるDLLファイルとBata版に同梱されるDLLファイルは互換性がありません。\nバージョンが非対応の場合は再インストールをお試しください。", "Click Me!! - 起動できません", MessageBoxButton.OK, MessageBoxImage.Error);
                exiting = 1; //終了
                System.Windows.Application.Current.Shutdown();
            }

            // エクスプローラーの戻る、進むの原理を使って画面遷移をしているため、マウスのサイドボタンで画面遷移が行われてしまう。
            // そのため、画面遷移をする際に「これはプログラム側からの画面遷移」という信号を受け取った上で画面遷移を行えるようにする処理。
            // プログラム側からの画面遷移でない場合、つまりマウスのサイドボタンなどによるナビゲートの試行時はキャンセル処理を行うようにする。
            frame.Navigating += delegate (object sender, NavigatingCancelEventArgs args) 
            {
                if (navigate)
                {
                    Debug.WriteLine("ナビゲートを許可しました");
                    args.Cancel = false; //ナビゲート実行
                }
                else
                {
                    Debug.WriteLine("ナビゲートは許可されませんでした");
                    args.Cancel = true; //ナビゲートキャンセル処理
                }
            };

        }

        private async void developer_debug()
        {
            this.Title += " (Development)";
            this.RegisterName("Dev_Bar", Dev_Bar);
            this.RegisterName("Dev_warn", Dev_warn);
            var bar = Properties.Resources.Warning_Bar.GetHbitmap();
            Dev_Bar.Source = Imaging.CreateBitmapSourceFromHBitmap(bar, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); //ゴニョゴニョしてImageSourceに変換

            ThicknessAnimation war1 = new ThicknessAnimation();
            war1.Duration = new Duration(TimeSpan.FromSeconds(2));
            war1.To = new Thickness(0, 0, 0, 0);
            war1.DecelerationRatio = 0.5;
            ThicknessAnimation war2 = new ThicknessAnimation();
            war2.Duration = new Duration(TimeSpan.FromSeconds(2));
            war2.To = new Thickness(0, 0, 0, 5);
            war2.DecelerationRatio = 0.5;

            Storyboard.SetTargetName(war1, Dev_Bar.Name);
            Storyboard.SetTargetProperty(war1, new PropertyPath(MarginProperty));
            Storyboard.SetTargetName(war2, Dev_warn.Name);
            Storyboard.SetTargetProperty(war2, new PropertyPath(MarginProperty));

            Storyboard dev_warning = new Storyboard();
            dev_warning.Children.Add(war1);
            dev_warning.Children.Add(war2);
            dev_warning.Begin(this);
            await Task.Delay(TimeSpan.FromSeconds(2));

        }

        protected virtual async void Window_Closing(object sender, CancelEventArgs args)
        {
            exiting += 1;
            if(exiting== 1)
            {
                args.Cancel = true; //アプリケーションの終了をキャンセルしておく

                //メッセージボックスを表示する
                var result = System.Windows.MessageBox.Show("終了しますか？", "Click Me!!", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                //何が選択されたか調べる
                if (result == System.Windows.MessageBoxResult.Yes) //「はい」が選択された時
                {
                    EndFX.Visibility = Visibility.Visible;
                    end_fade_anim.Begin(this);
                    soundplayer.stop(); //別クラスで作ったメソッドを実行し、BGMを停止

                    System.IO.Stream strmbye = Properties.Resources.see_you; //Properties.Resources.ファイル名（拡張子なし）リソースファイルの取得はStreamオブジェクトで取得する。
                    ClickMe.Sound.soundeffects.SEplay(strmbye);

                    game.gamemode = ""; //RPCの読込を停止するため、ゲームモードを空っぽに
                    ClickMe.DiscordRPC.RP.Deinitialize(); //終了時はRPCを開放する。（これをしないとDiscord側のRPCが続いてしまう、メモリリークが発生する）

                    await Task.Delay(TimeSpan.FromSeconds(2.2));
                    System.Windows.Application.Current.Shutdown();
                }
                else if (result == System.Windows.MessageBoxResult.No)
                {
                    //「いいえ」が選択された時
                    exiting = 0; //Exitingをリセット
                }
            }
            else
            {
                //終了
            }
        }

        public static MainWindow mw_data;

        void mw(MainWindow data)
        {
            mw_data = data;
        }

        public static async Task end_app()
        {

            //メッセージボックスを表示する
            var result = System.Windows.MessageBox.Show("終了しますか？", "Click Me!!", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            //何が選択されたか調べる
            if (result == System.Windows.MessageBoxResult.Yes) //「はい」が選択された時
            {
                mw_data.EndFX.Visibility = Visibility.Visible;
                end_fade_anim.Begin(mw_data);
                soundplayer.stop(); //別クラスで作ったメソッドを実行し、BGMを停止

                System.IO.Stream strmbye = Properties.Resources.see_you; //Properties.Resources.ファイル名（拡張子なし）リソースファイルの取得はStreamオブジェクトで取得する。
                ClickMe.Sound.soundeffects.SEplay(strmbye);

                game.gamemode = ""; //RPCの読込を停止するため、ゲームモードを空っぽに
                ClickMe.DiscordRPC.RP.Deinitialize(); //終了時はRPCを開放する。（これをしないとDiscord側のRPCが続いてしまう、メモリリークが発生する）

                await Task.Delay(TimeSpan.FromSeconds(2.2));
                exiting = 1; //終了
                System.Windows.Application.Current.Shutdown();
            }
            else if (result == System.Windows.MessageBoxResult.No)
            {
                //「いいえ」が選択された時

            }

        }

        public static double win_w = 800;
        public static double win_h = 600;

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            win_w = e.NewSize.Width;
            win_h = e.NewSize.Height;
            //res.Content = $"{win_w} x {win_h}";
            startup.startup_resize(win_w, win_h);
            title.title_resize(win_w, win_h);
            game.game_size(win_w, win_h);

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //5つのボタンが付いたマウスMicrosoft IntelliMouse Explorerでの
            //XBUTTON1とXBUTTON2を調べる
            if (e.ChangedButton == MouseButton.XButton1)
            {
                Debug.WriteLine("マウスのXBUTTON1が押されています。");
            }
            if (e.ChangedButton == MouseButton.XButton2)
            {
                Debug.WriteLine("マウスのXBUTTON2が押されています。");
            }
        }

    }
}
