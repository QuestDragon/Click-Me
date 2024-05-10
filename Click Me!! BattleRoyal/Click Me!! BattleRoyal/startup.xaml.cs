using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Click_Me_BattleRoyal
{
    /// <summary>
    /// startup.xaml の相互作用ロジック
    /// </summary>
    public partial class startup : Page
    {
        private static startup this_base = new startup();
        public static void startup_resize(double width, double height)
        {
            this_base.Base.Width = width -40;
            this_base.Base.Height = height -40;
        }


        public startup()
        {
            InitializeComponent();
            this.RegisterName("FirstWarning", FirstWarning); //XAMLに書いたオブジェクトをアニメーションとして使うためウィンドウに登録。（コードビハインドで書いた場合は名前部分を「変数名.name」で登録できるが、XAMLの場合は変数名がないので、ダブルクオーテーションでくくって名前を決める。※だいたいオブジェクト名と同じでOK）
            this.RegisterName("warn_border", warn_border);

            this_base = this;

            FirstWarning.Text = "ご注意\n\n本ソフトを権利者の許諾なく\r\nインターネットを通じて配信 配布する行為\r\nまた、違法なインターネット配信と知りながら\r\nダウンロードする行為は法律で固く禁じられております。\n\n皆様のご協力をよろしくお願いいたします。";
            FirstWarning.Foreground = Brushes.Red; //文字色：赤

            //アニメーション作成
            DoubleAnimation firstwarnfade1 = new DoubleAnimation();
            firstwarnfade1.Duration = new Duration(TimeSpan.FromSeconds(1)); //1秒
            firstwarnfade1.From = 0;
            firstwarnfade1.To = 1;

            DoubleAnimation firstwarnfade2 = new DoubleAnimation();
            firstwarnfade2.Duration = new Duration(TimeSpan.FromSeconds(1)); //1秒
            firstwarnfade2.From = 1;
            firstwarnfade2.To = 0;

            //ストーリーボードオブジェクトを作成してアニメーションを割当
            Storyboard.SetTargetName(firstwarnfade1, FirstWarning.Name); //適用先のオブジェクト名指定
            Storyboard.SetTargetProperty(firstwarnfade1, new PropertyPath(OpacityProperty)); //適用先プロパティ（PropertyPathは「オブジェクトのクラス名.プロパティ名」で記述する。なお、オブジェクトのクラス名は省略可能。）
            Storyboard.SetTargetName(firstwarnfade2, FirstWarning.Name); //適用先のオブジェクト名指定
            Storyboard.SetTargetProperty(firstwarnfade2, new PropertyPath(OpacityProperty)); //適用先プロパティ

            Storyboard.SetTargetName(firstwarnfade1, warn_border.Name); //適用先のオブジェクト名指定
            Storyboard.SetTargetProperty(firstwarnfade1, new PropertyPath(OpacityProperty)); //適用先プロパティ（PropertyPathは「オブジェクトのクラス名.プロパティ名」で記述する。なお、オブジェクトのクラス名は省略可能。）
            Storyboard.SetTargetName(firstwarnfade2, warn_border.Name); //適用先のオブジェクト名指定
            Storyboard.SetTargetProperty(firstwarnfade2, new PropertyPath(OpacityProperty)); //適用先プロパティ

            //ストーリーボード作成
            Storyboard fade1 = new Storyboard();
            fade1.Children.Add(firstwarnfade1); //アニメーション登録

            Storyboard fade2 = new Storyboard();
            fade2.Children.Add(firstwarnfade2);

            base.Loaded += async delegate (object sender, RoutedEventArgs args) //Grid（名前はXAML側で「base」と名付けた）がロードされたとき実行（await文を使うため「async delegate」を使用）
            {
                ClickMe.DiscordRPC.RP.idle();
                startup_resize(MainWindow.win_w, MainWindow.win_h); //ロード時に一度調整

                fade1.Begin(this); //メッセージ表示
                await Task.Delay(TimeSpan.FromSeconds(5)); //5秒待つ ※時間を引数などで使用するところではTimeSpan.From〇〇メソッドでデフォルトのミリ秒指定を秒指定や分指定に変えることができる。
                fade2.Begin(this);
                await Task.Delay(TimeSpan.FromSeconds(1));
                //タイトル画面に遷移
                var title_screen = new title(); //変数名 = new ページ名【xamlファイル名】();
                MainWindow.navigate = true; //ナビゲートの許可
                NavigationService.Navigate(title_screen); //アクセス
                MainWindow.navigate = false; //ナビゲートロック
            };
        }
    }
}
