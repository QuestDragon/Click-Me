using NAudio.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Click_Me_BattleRoyal
{
    /// <summary>
    /// endless.xaml の相互作用ロジック
    /// </summary>
    public partial class game : Page
    {
        //C#では、最初からなにも入ってない（というより何もないという事実すら存在しない）状態にできると言えばできるが、あまりいい書き方ではないらしい。そのため、「空っぽ」というモノでもいいから何かしら「最初に入れておく」必要がある。
        public static string gamemode = string.Empty; //staticを入れると静的フィールドとなり、値を共有可能。ちなみに、変数名で;を入力して終わらせてしまうとNull許容の警告（中にもともと何があるか決めてない）が出るので、空（Empty）を入れておく。Stringの場合は"" でもOK。
        static int game_time = 0;
        static int set_time = 0;
        static int judge_level = 0;
        static bool is_sudden_death = false; //延長戦かどうか
        static bool sudden_death_progress = false; //延長戦実施中かどうか
        static bool RPtimer_setOK = false;

        static List<Border> PLAYERS = new List<Border>(); //ボーダーオブジェクトリスト
        static List<string> com_name = new List<string>(); //コンピュータの名前リスト
        static List<string> raw_com_name = new List<string>(); //コンピュータの名前リスト(こちらは変動なし：ボーダー位置修正に使用する)
        static List<double> COM_base = new List<double>(); //コンピュータの基礎能力格納用リスト

        static game this_base = new game();

        public static void mode_check(string mode) //別クラスからも利用可能 外部呼び出し可能 戻り値なし メソッド名(引数の型 引数名)
        {
            gamemode = mode;
        }
        public static void set_timer(int time)
        {
            game_time = time; //カウントダウン用の時間
            set_time  = time; //設定された時間
        }

        /// 現在の問題：無限に肥大化する
        public static void game_size(double w, double h) //Game画面の場合、えげつないフォントサイズがあるせいでTitleやStartupのようにDouble.NaNで自由にサイズ変更ができない。（指定状態でないとウィンドウが広がってしまう）
        {

            //ウィンドウサイズからトーナメントツリーのサイズをアスペクト比固定で設定する。 (名残)
            //int width = (int)w;
            //int height = (int)h ; //ウィンドウやタイトルバーの誤差を修正(30ピクセル) + 余白として最低値の40ピクセル差し引き
            //var checkw = (height * 4) / 3;
            //var checkh = (width / 4) * 3;
            //Debug.WriteLine($"{w} x {h}");
            //Debug.WriteLine($"W: {width} H: {height}");
            //Debug.WriteLine($"CW: {checkw} CH: {checkh}");

            //if (checkw > width) //WidthがオーバーしたらWidth基準でリサイズ
            //{
            //    Debug.WriteLine("Width基準1");
            //    this_base.tournament_tree.Width = width;
            //    this_base.tournament_tree.Height = ((width / 4) * 3);
            //    Debug.WriteLine($"{this_base.tournament_tree.Width} x {this_base.tournament_tree.Height}");
            //}
            //else if (checkh > height)
            //{
            //    Debug.WriteLine("Height基準1");
            //    this_base.tournament_tree.Width = ((height * 4) / 3);
            //    this_base.tournament_tree.Height = height;
            //    Debug.WriteLine($"{this_base.tournament_tree.Width} x {this_base.tournament_tree.Height}");
            //}
            //else
            //{
            //    if (width > height)
            //    {
            //        Debug.WriteLine("Height基準2");
            //        this_base.tournament_tree.Width = ((height * 4) / 3);
            //        this_base.tournament_tree.Height = height;
            //        Debug.WriteLine($"{this_base.tournament_tree.Width} x {this_base.tournament_tree.Height}");
            //    }
            //    else
            //    {
            //        Debug.WriteLine("Width基準2");
            //        this_base.tournament_tree.Width = width;
            //        this_base.tournament_tree.Height = ((width / 4) * 3);
            //        Debug.WriteLine($"{this_base.tournament_tree.Width} x {this_base.tournament_tree.Height}");
            //    }

            //}

            this_base.game_window.Width = MainWindow.win_w;
            this_base.game_window.Height = MainWindow.win_h - 40; //タイトルバーの誤差
            //ちなみにStretchだとずれるのでGridは中央寄せにしておく。

            tournament_player_pos(MainWindow.win_w, MainWindow.win_h);

        }

        private static void tournament_player_pos(double width, double height)
        {
            //開始地点の位置修正
            var general_h = height * 0.7417;
            var ajaster = ((width * 0.0575) - 46) + 40; //ズレをBorderのWidthで補正
                                                        //(幅から一定比率による座標を求め、プレイヤーの左Marginとの差を求める。最後にborderのWidth 40 を足す。 ※ちなみにこれでどうやってAjasterとして機能しているかの仕組みは忘れた)
            
            var X_base = width / 8; //8等分した結果を格納
            var tree_margin = X_base / 2; //トーナメントツリーのマージン
            this_base.tree_A.Margin = new Thickness(tree_margin, 0, tree_margin, 0);
            this_base.tree_B.Margin = new Thickness(tree_margin, 0, tree_margin, 0);
            this_base.tree_C.Margin = new Thickness(tree_margin, 0, tree_margin, 0);
            this_base.tree_D.Margin = new Thickness(tree_margin, 0, tree_margin, 0);

            if (judge_level == 0)
            {
                var X1 = X_base / 2; //"あなた"の位置（コンピューターのX位置の計算にも使用する）
                var X_pos_center = new List<double>(); //8等分した各エリアの中心点を格納

                X_pos_center.Add(X1); //まずはプレイヤーX位置を記録 //変更前はX1をAdd
                for (int column = 2; column <= 8; column++) //計算の都合により2からスタート。コンピューター分繰り返す
                {
                    X_pos_center.Add((X_base * column) - X1);
                }

                for (int i = 0; i < PLAYERS.Count; i++)
                {

                    PLAYERS[i].Width = ajaster; //画面のWidthに合わせて太くする
                    PLAYERS[i].Margin = new Thickness(X_pos_center[i] - (ajaster / 2), general_h, 0, 0); //Borderの補正用Width値のうち半分を引いてMargin設定
                }

            }

            else //準々決勝以上が終了
            {
                var X_pos_phase1 = new List<double>(); //8等分した各エリアの中心点を格納
                for (int phase1_column = 1; phase1_column <= 7; phase1_column += 2) //カウント用の数が奇数になるようにする。 For文の意味： カウント変数と初期値、条件式、カウント方法
                {
                    X_pos_phase1.Add(X_base * phase1_column); //8等分したうちの奇数等分のWidth位置を記録
                }

                var X_pos_phase2 = new List<double>(); //8等分した各エリアの中心点を格納
                X_pos_phase2.Add(width / 4); //4等分したうちの1番目のWidth位置を記録
                X_pos_phase2.Add((width / 4) * 3); //4等分したうちの3番目のWidth位置を記録

                //for文： for(カウント用の型 カウント用変数名 = カウント用変数初期値; 条件式※今回は辞書型Winnersの要素数がカウント用変数の値より小さい間繰り返し; カウント用変数の値の変わり方※今回は1ずつインクリメント)
                for (int i = 0; i < game_tournament_anim.winners_name.Count; i++)
                {
                    var target_name = game_tournament_anim.winners_name[i]; //勝者を格納している名前からオブジェクトと位置を取得する

                    if (game_tournament_anim.winners_phase[target_name] == 1) //準決勝進出者
                    {
                        if (game_tournament_anim.winners_pos[target_name] == "A")
                        {
                            game_tournament_anim.raw_winners[target_name].Width = ajaster;
                            game_tournament_anim.raw_winners[target_name].Margin = new Thickness(X_pos_phase1[0] - (ajaster / 2), general_h, 0, 0); //Borderの補正用Width値のうち半分を引いてMargin設定
                        }
                        else if (game_tournament_anim.winners_pos[target_name] == "B")
                        {
                            game_tournament_anim.raw_winners[target_name].Width = ajaster;
                            game_tournament_anim.raw_winners[target_name].Margin = new Thickness(X_pos_phase1[1] - (ajaster / 2), general_h, 0, 0); 
                        }
                        else if (game_tournament_anim.winners_pos[target_name] == "C")
                        {
                            game_tournament_anim.raw_winners[target_name].Width = ajaster;
                            game_tournament_anim.raw_winners[target_name].Margin = new Thickness(X_pos_phase1[2] - (ajaster / 2), general_h, 0, 0); 
                        }
                        else if (game_tournament_anim.winners_pos[target_name] == "D")
                        {
                            game_tournament_anim.raw_winners[target_name].Width = ajaster;
                            game_tournament_anim.raw_winners[target_name].Margin = new Thickness(X_pos_phase1[3] - (ajaster / 2), general_h, 0, 0); 
                        }
                        game_tournament_anim.winners[target_name].Y = -(height * 0.2667); //なぜかここだけウィンドウ基準にしてしまったのでそういうことで…ｗ
                    }
                    else if (game_tournament_anim.winners_phase[target_name] == 2) //決勝進出者
                    {
                        if (game_tournament_anim.winners_pos[target_name] == "A" || game_tournament_anim.winners_pos[target_name] == "B") //C#でのorは || で表現する. ちなみにAndは && で表現する.
                        {
                            game_tournament_anim.raw_winners[target_name].Width = ajaster;
                            game_tournament_anim.raw_winners[target_name].Margin = new Thickness(X_pos_phase2[0] - (ajaster / 2), general_h, 0, 0);
                        }
                        else if (game_tournament_anim.winners_pos[target_name] == "C" || game_tournament_anim.winners_pos[target_name] == "D")
                        {
                            game_tournament_anim.raw_winners[target_name].Width = ajaster;
                            game_tournament_anim.raw_winners[target_name].Margin = new Thickness(X_pos_phase2[1] - (ajaster / 2), general_h, 0, 0); 
                        }
                        game_tournament_anim.winners[target_name].Y = -(this_base.game_window.Height * 0.4833);
                    }
                    else if (game_tournament_anim.winners_phase[target_name] == 3) //勝者
                    {
                        game_tournament_anim.raw_winners[target_name].Width = ajaster;
                        game_tournament_anim.raw_winners[target_name].Margin = new Thickness((this_base.game_window.Width / 2) - (ajaster / 2), general_h, 0, 0);
                        game_tournament_anim.winners[target_name].Y = -(this_base.game_window.Height * 0.5667);
                    }
                }

                //以下敗退者の処理

                var X1 = X_base / 2; //"あなた"の位置（コンピューターのX位置の計算にも使用する）
                var X_pos_center = new List<double>(); //8等分した各エリアの中心点を格納

                for (int column = 2; column <= 8; column++) //計算の都合により2からスタート。コンピューター分繰り返す
                {
                    X_pos_center.Add((X_base * column) - X1);
                }

                var losers = ClickMe.Tournament.Ai_Status.loser_index;
                for (int i = 0; i < losers.Count; i++)
                {
                    PLAYERS[losers[i] + 1].Width = ajaster; //画面のWidthに合わせて太くする ※Losersの0番目に0が入ってた場合、"あなた"が指定されるのを回避するため+1する
                    PLAYERS[losers[i] + 1].Margin = new Thickness(X_pos_center[losers[i]] - (ajaster / 2), general_h, 0, 0); //Borderの補正用Width値のうち半分を引いてMargin設定 ※ちなみにX_pos_centerは"あなた"の座標を入れていないため+1の必要はなし。
                    Debug.WriteLine($"Width : {width}");
                    Debug.WriteLine($"Losers: {losers[0]} / {losers[1]} / {losers[2]} / {losers[3]}");
                    Debug.WriteLine($"敗者:{PLAYERS[losers[i] + 1].Name} を{X_pos_center[losers[i]]} に補正しました");
                }

            }
        }

        public game()
        {
            InitializeComponent();

            //初期値を保管
            var ATTACK_init = ATTACK.Background;
            var timer_label_color_init = timer_label.Foreground;
            var attack_label_color_init = attack_label.Foreground;
            var attack_label_init = attack_label.Content;

            var attack_counts = 0;
            var terminate = false;

            this_base = this;

            select_se.Items.Add("なし");
            select_se.Items.Add("ランダム再生");
            select_se.Items.Add("クリック音");
            select_se.Items.Add("打撃1");
            select_se.Items.Add("打撃2");
            select_se.Items.Add("打撃3");
            select_se.Items.Add("打撃4");
            select_se.Items.Add("打撃5");
            select_se.Items.Add("銃撃1");
            select_se.Items.Add("叩き1");
            select_se.Items.Add("叩き2");
            select_se.Items.Add("爆発1");
            select_se.Items.Add("爆発2");
            select_se.Items.Add("爆発3");
            select_se.Items.Add("斬る1");
            select_se.Items.Add("斬る2");
            select_se.Items.Add("ガラス1");
            select_se.Items.Add("ガラス2");
            select_se.Items.Add("ガラス3");
            select_se.Items.Add("配管工攻撃");
            select_se.Items.Add("配管工収益");
            select_se.Items.Add("レジスター");
            select_se.Items.Add("Oh");
            select_se.Items.Add("銃撃2");
            select_se.Items.Add("ぽん");
            select_se.Items.Add("パンチ");
            select_se.Items.Add("ショットガン");
            select_se.Items.Add("Yeah");

            select_se.SelectedIndex= 0;


            this.RegisterName("fadeFX", fadeFX); //フェードイン用のRectangleを登録
            this.RegisterName("tournament_tree", tournament_tree); //トーナメントツリーを登録（フェードアニメーション）

            //フェードインアニメーション作成
            DoubleAnimation intro_fade = new DoubleAnimation();
            intro_fade.Duration = new Duration(TimeSpan.FromSeconds(1)); //1秒
            intro_fade.From = 0;
            intro_fade.To = 1;
            //ゲームセレクト後のフェードアウトアニメーション作成
            DoubleAnimation outro_fade = new DoubleAnimation();
            intro_fade.Duration = new Duration(TimeSpan.FromSeconds(0.5)); //0.5秒
            intro_fade.From = 1;
            intro_fade.To = 0;

            //トーナメントツリーフェード
            DoubleAnimation tree_fadein = new DoubleAnimation();
            tree_fadein.Duration = new Duration(TimeSpan.FromSeconds(1));
            tree_fadein.From = 0;
            tree_fadein.To = 1;
            DoubleAnimation tree_fadeout = new DoubleAnimation();
            tree_fadeout.Duration = new Duration(TimeSpan.FromSeconds(1));
            tree_fadeout.From = 1;
            tree_fadeout.To = 0;

            //ストーリーボードオブジェクトを作成してアニメーションを割当
            Storyboard.SetTargetName(intro_fade, fadeFX.Name);
            Storyboard.SetTargetProperty(intro_fade, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(outro_fade, fadeFX.Name);
            Storyboard.SetTargetProperty(outro_fade, new PropertyPath(Rectangle.OpacityProperty));

            Storyboard.SetTargetName(tree_fadein, tournament_tree.Name);
            Storyboard.SetTargetProperty(tree_fadein, new PropertyPath(OpacityProperty)); //クラス名は省略も可能
            Storyboard.SetTargetName(tree_fadeout, tournament_tree.Name);
            Storyboard.SetTargetProperty(tree_fadeout, new PropertyPath(OpacityProperty)); //クラス名は省略も可能


            //ストーリーボード作成
            Storyboard intro_fade_anim = new Storyboard();
            intro_fade_anim.Children.Add(intro_fade);
            Storyboard outro_fade_anim = new Storyboard();
            outro_fade_anim.Children.Add(outro_fade);

            Storyboard tree_fadein_anim = new Storyboard();
            tree_fadein_anim.Children.Add(tree_fadein);
            Storyboard tree_fadeout_anim = new Storyboard();
            tree_fadeout_anim.Children.Add(tree_fadeout);

            var hBitmap = Properties.Resources.clickme_focus.GetHbitmap(); //Bitmapをリソースから用意
            var endless_logo = Properties.Resources.名称未設定_2.GetHbitmap(); //Bitmapをリソースから用意
            var time_attack_logo = Properties.Resources.名称未設定_3.GetHbitmap(); //Bitmapをリソースから用意
            var tournament_logo = Properties.Resources.名称未設定_1.GetHbitmap(); //Bitmapをリソースから用意
            var tournament_top_picture = Properties.Resources.chanpion.GetHbitmap();

            chanpion_image.Source = Imaging.CreateBitmapSourceFromHBitmap(tournament_top_picture, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); //ゴニョゴニョしてImageSourceに変換

            bool tree_dismiss = false;
            int tournament_phase = 1; //準々決勝、準決勝、決勝を判定
            //準々決勝、準決勝、決勝戦でのコンピュータ数取得用メソッド
            string phase_check(int phase)
            {
                if(phase == 1)
                {
                    return "準々決勝";
                }
                else if(phase == 2)
                {
                    return "準決勝";
                }
                else
                {
                    return "決勝";
                }
            }


            List<Label> com_player_listed()
            {
                //ラベルオブジェクトを格納するリストを作成
                var coms = new List<Label>
                {
                    com1_name,
                    com2_name,
                    com3_name,
                    com4_name,
                    com5_name,
                    com6_name,
                    com7_name
                };
                return coms;
            }

            bool white = true;
            void click_image_change() //void = 戻り値がないメソッド
            {
                if (white) //== True
                {
                    click_click.Source = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); //ゴニョゴニョしてImageSourceに変換
                    white = false;
                }
                else
                {
                    click_click.Source = new BitmapImage(new Uri("/images/clickme.jpg", UriKind.Relative));
                    white = true;
                }
            }

            async Task finished(int counts)
            {
                double d_counts = counts;
                double per = d_counts / set_time; //double変数にはintがそのまま使える。（1を1.0と読めるから。）逆は不可。
                string per_attack = per.ToString("F2"); //小数第2位まで

                //メッセージボックスを表示する
                var result = System.Windows.MessageBox.Show($"お疲れ様でした！結果は {counts} でした！\n速度： {per_attack} /秒", "Click Me!!", MessageBoxButton.OK, MessageBoxImage.Information);

                //何が選択されたか調べる
                if (result == System.Windows.MessageBoxResult.OK)
                {
                    if(gamemode == "time_attack")
                    {
                        await back_to_menu();
                    }
                    else //Tournament Mode
                    {
                        var com_scores = ClickMe.Tournament.Ai_Status.COM_Score(COM_base, set_time); //コンピュータの基礎能力が格納されたリストと所要時間を指定し実行。各コンピュータのスコアがリスト型で返ってくる。

                        //コンピューターとプレイヤーが同点の場合
                        if (com_scores[0] == attack_counts)
                        {
                            is_sudden_death = true;
                            while(is_sudden_death)
                            {
                                tournament_name.Content = phase_check(tournament_phase) + "(延長戦)";
                                var sudden_death = System.Windows.MessageBox.Show($"スコアが同点の対戦者がいるため、次の延長戦を行います：\nあなた Vs. {com_name[0]}", "Click Me!!", MessageBoxButton.OK, MessageBoxImage.Information);
                                sudden_death_progress = true;
                                await game_start();
                                while (sudden_death_progress) //サドンデス開催中の間待機
                                {
                                    await Task.Delay(TimeSpan.FromSeconds(1));
                                }
                                com_scores[0] = ClickMe.Tournament.Ai_Status.COM_ReScore(COM_base, set_time); //サドンデスでの対戦相手のスコアを生成し、既存のスコアを書き換える
                                var result_2 = System.Windows.MessageBox.Show($"お疲れ様でした！結果は {counts} でした！\n速度： {per_attack} /秒\n\n対戦相手のスコア：{com_scores[0]}", "Click Me!!", MessageBoxButton.OK, MessageBoxImage.Information);
                                if (com_scores[0] != attack_counts) //同点でなくなった場合
                                {
                                    is_sudden_death = false;
                                }
                            }
                        }

                        var result_mes = $"あなた：{attack_counts}\n";
                        var next_tournament_name = phase_check(tournament_phase + 1);
                        for(int i = 0; i < com_scores.Count; i++)
                        {
                            result_mes += $"{com_name[i]}：{com_scores[i].ToString()}\n";
                        }

                        //トーナメントツリーフェード
                        tournament_tree.Visibility = Visibility.Visible;
                        tree_fadein_anim.Begin(this);
                        var result2 = System.Windows.MessageBox.Show($"中間発表\n\n{result_mes}", "Click Me!!", MessageBoxButton.OK, MessageBoxImage.Information);

                        game_tournament_anim.judgement(this, tournament_phase, attack_counts, com_scores, com_name); //判定とアニメーション(トーナメントツリーの画像寸法を必要とするのでこちらの情報を渡す)
                        await Task.Delay(TimeSpan.FromSeconds(2));
                        judge_level++; //１インクリメント

                        if (com_scores[0] < attack_counts)
                        {
                            if (tournament_phase == 3)
                            {
                                soundplayer.winBGM(); //勝利のBGMを再生
                                var win = System.Windows.MessageBox.Show($"おめでとうございます！\n優勝です！\n\n勝った対戦相手：{com_name[0]}", "Click Me!!", MessageBoxButton.OK, MessageBoxImage.Information);
                                await back_to_menu(); //終了
                            }
                            else
                            {
                                System.IO.Stream strmyes = Properties.Resources.Yeah_PUFF; //Properties.Resources.ファイル名（拡張子なし）リソースファイルの取得はStreamオブジェクトで取得する。
                                ClickMe.Sound.soundeffects.SEplay(strmyes);
                                var result3 = System.Windows.MessageBox.Show($"おめでとうございます！\n{next_tournament_name}進出です！\n\n勝った対戦相手：{com_name[0]}", "Click Me!!", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            
                        }
                        else
                        {
                            if (tournament_phase == 3)
                            {
                                soundplayer.loseBGM(); //敗北のBGMを再生
                                var win = System.Windows.MessageBox.Show($"残念…敗北です。\n優勝することはできませんでした。\n\n負けた対戦相手：{com_name[0]}", "Click Me!!", MessageBoxButton.OK, MessageBoxImage.Error);
                                await back_to_menu(); //終了
                            }
                            else
                            {
                                System.IO.Stream strmyes = Properties.Resources.chi_n; //Properties.Resources.ファイル名（拡張子なし）リソースファイルの取得はStreamオブジェクトで取得する。
                                ClickMe.Sound.soundeffects.SEplay(strmyes);
                                var result3 = System.Windows.MessageBox.Show($"残念…敗退です。\n{next_tournament_name}進出することはできませんでした。\n\n負けた対戦相手：{com_name[0]}", "Click Me!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                await back_to_menu(); //終了
                            }
                        }

                        if (tournament_phase != 3) //決勝以外（決勝終了後は終了処理になるが、終了処理をする間にここに来てしまうためListがOutofRangeになってしまう。それを阻止するためのIF。
                        {
                            //敗者の削除
                            ClickMe.Tournament.Ai_Status.COM_delete(judge_level, com_scores, ref com_name, ref COM_base); //引数の前にrefをつけると参照渡しになる。参照渡しすると、引数に指定した変数の中身をメソッド側で変更できる。（ただしメソッド側でもrefに対応している必要がある）
                        }

                        tree_dismiss = false;
                        while (tree_dismiss == false)
                        {
                            await Task.Delay(TimeSpan.FromSeconds(0.1));
                        }
                        tree_fadeout_anim.Begin(this); //ツリーをフェードアウト
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        tournament_tree.Visibility = Visibility.Hidden;
                        tournament_phase += 1; //次の対戦へ
                        tournament_name.Content = phase_check(tournament_phase);
                        //再度Discord RPCのタイマーを設定
                        RPtimer_setOK = false;
                        if(terminate != true)
                        {
                            await game_start();
                        }
                    }
                }

            }

            async Task back_to_menu()
            {
                ClickMe.Temp.temp.doBGM(BGM_Trigger.IsChecked); //IsCheckedがNull許容型（型に？がついている）ので、メソッド側の要求引数の型は非許容型に変換する。（非許容型にするには接頭辞に「(型名)」を入れる。)
                ClickMe.Temp.temp.doSound(select_se.SelectedIndex);
                attack_counts = 0; //初期化
                terminate = true; //中止信号送信
                soundplayer.stop(); //別クラスで作ったBGM再生停止メソッドを実行
                fadeFX.Visibility = Visibility.Visible;
                outro_fade_anim.Begin(this);
                await Task.Delay(TimeSpan.FromSeconds(1));
                var title_screen = new title(); //変数名 = new ページ名【xamlファイル名】();
                MainWindow.navigate = true; //ナビゲート許可
                NavigationService.Navigate(title_screen); //アクセス
                MainWindow.navigate = false; //ナビゲートロック
            }


            game_window.Loaded += async delegate (object sender, RoutedEventArgs args)
            {
                terminate = false;

                BGM_Trigger.IsChecked = ClickMe.Temp.temp.BGM; //前回の設定を引き継ぎ
                select_se.SelectedIndex = ClickMe.Temp.temp.Sound;

                //プレイ開始時間の設定
                RPtimer_setOK = false;

                if (gamemode == "endless")
                {
                    game_logo.Source = Imaging.CreateBitmapSourceFromHBitmap(endless_logo, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); //ゴニョゴニョしてImageSourceに変換
                    time_mode.Visibility = Visibility.Hidden;
                    count_border.Height = 140; //残り時間のスペースをカウンターの表示で埋める

                }
                else if (gamemode == "time_attack")
                {
                    game_logo.Source = Imaging.CreateBitmapSourceFromHBitmap(time_attack_logo, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    timer_label.Content = $"{game_time} 秒";
                }
                else //Tournament Mode
                {
                    judge_level = 0; //ジャッジを最初からに初期化(COM削除の実行回数を初期化する意味もある。)
                    tournament_name.Visibility = Visibility.Visible;
                    tournament_name.Content = phase_check(tournament_phase); //準々決勝の表示
                    game_logo.Source = Imaging.CreateBitmapSourceFromHBitmap(tournament_logo, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    timer_label.Content = $"{game_time} 秒";
                    tournament_class_name.Content = $"{set_time}秒クラストーナメント";
                }

                game_size(MainWindow.win_w, MainWindow.win_h);

                //フェードイン
                fadeFX.Visibility = Visibility.Visible;
                intro_fade_anim.Begin(this);
                await Task.Delay(TimeSpan.FromSeconds(1));
                fadeFX.Visibility = Visibility.Hidden;

                if (gamemode == "tournament")
                {
                    //参加者たち (現状ボーダー位置の修正に使用)
                    PLAYERS.Clear();
                    PLAYERS.Add(game_player);
                    PLAYERS.Add(com1);
                    PLAYERS.Add(com2);
                    PLAYERS.Add(com3);
                    PLAYERS.Add(com4);
                    PLAYERS.Add(com5);
                    PLAYERS.Add(com6);
                    PLAYERS.Add(com7);
                    com_name = ClickMe.Tournament.Ai_Gen.Ai_Name(7); //別クラスで作った名前ジェネレータメソッドからリスト型が返ってくるので、変数に格納する。
                    raw_com_name = com_name; //変動なしのリストにも格納
                    var com_label = com_player_listed(); //コンピュータのラベルオブジェクトがリストで返ってくるので変数に格納する。
                    //取得したリストの数だけForで繰り返す
                    for (int i = 0; i < com_name.Count; i++) //この文の意味：カウント用変数iは0から数え、com_nameリストの数だけ繰り返す。（数と添字は1つズレてるのでより小さいでズレを解消）カウント用変数iはインクリメントする。
                    {
                        com_label[i].Content = com_name[i]; //ラベルオブジェクトの文字にジェネレータから取ってきた名前をそれぞれつける。
                    }
                    COM_base = ClickMe.Tournament.Ai_Status.COM_Base(7); //各コンピュータの基礎能力がリスト型で返ってくる。

                    game_tournament_anim.target_init(this, com_name); //Borderアニメーション用のBorderオブジェクトをあっちで参照できるように、このページの情報を渡しておく。また、アニメーション用として使う辞書型オブジェクトのKeyにCom名を使用するためコンピュータ名が格納されたリストを渡す。
                    game_tournament_anim.raw_target_init(this, com_name); //Borderアニメーション後、ウィンドウサイズ変更時のBorderオブジェクト修正をあっちでできるように、このページの情報を渡し、修正対象オブジェクトとして判断材料で使う辞書型オブジェクトのKeyにCom名を使用するためコンピュータ名が格納されたリストを渡す。
                    tournament_player_pos(MainWindow.win_w, MainWindow.win_h); //画面フェードイン前だとなぜか調整されないためここで改めてトーナメントツリーだけ再調整

                    //トーナメントツリーをフェードイン
                    tournament_tree.Visibility = Visibility.Visible;
                    tree_fadein_anim.Begin(this);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    tree_dismiss = false;
                    while (tree_dismiss == false)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(0.1));
                    }
                    tree_fadeout_anim.Begin(this); //ツリーをフェードアウト
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    tournament_tree.Visibility = Visibility.Hidden;

                }

                await game_start();

                while (true)
                {
                    click_image_change();
                    await Task.Delay(TimeSpan.FromSeconds(1));
                };

            };

            async Task game_start()
            {
                //初期化
                attack_counts = 0;
                count_label.Content = attack_counts.ToString();
                game_time = set_time;
                ATTACK.Background = ATTACK_init;
                timer_label.Foreground = timer_label_color_init;
                attack_label.Foreground = attack_label_color_init;
                attack_label.Content = attack_label_init;
                if(gamemode != "endless")
                {
                    timer_label.Content = $"{game_time} 秒";
                }

                if (BGM_Trigger.IsChecked == true)
                {
                    //ループ再生
                    soundplayer.gameBGM(); //別クラスで作ったBattleBGMを再生
                }

                while (terminate != true)
                {
                    click_image_change();
                    await Task.Delay(TimeSpan.FromSeconds(1));

                    //Discord RPCの設定
                    if (gamemode == "endless")
                    {
                        ClickMe.DiscordRPC.RP.endless(attack_counts);
                    }
                    else if (gamemode == "time_attack")
                    {
                        ClickMe.DiscordRPC.RP.time_attack(attack_counts, set_time);
                    }
                    else if (gamemode == "tournament")
                    {
                        ClickMe.DiscordRPC.RP.tournament(attack_counts, set_time, phase_check(tournament_phase));
                    }
                    if (RPtimer_setOK == false)
                    {
                        if(gamemode == "endless")
                        {
                            ClickMe.DiscordRPC.RP.uptimer();
                        }
                        else
                        {
                            ClickMe.DiscordRPC.RP.downtimer(set_time);
                        }
                        RPtimer_setOK = true;
                    }

                    if (gamemode != "endless")
                    {
                        if (terminate)
                        {
                            break;
                        }
                        else if (game_time != 0)
                        {
                            game_time -= 1; //１ずつ減少
                            timer_label.Content = $"{game_time} 秒"; //$をつけると文字列の中で変数が使用可能。（Pythonで言うところのf""。）
                            if (game_time == 0) //0になった瞬間ブレイク
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }

                    }
                }
                //時間切れになったら
                if (terminate != true && gamemode != "") //&&でAND演算子になる。
                {
                    Debug.WriteLine("時間切れ！！");
                    System.IO.Stream strm = Properties.Resources.finish;
                    ClickMe.Sound.soundeffects.SEplay(strm); //ホイッスルを鳴らす

                    SolidColorBrush timeout_attack_color = new SolidColorBrush();
                    timeout_attack_color.Color = Colors.Red;
                    SolidColorBrush timeout_color = new SolidColorBrush();
                    timeout_color.Color = Colors.DarkGray;
                    SolidColorBrush timeout_background = new SolidColorBrush();
                    timeout_background.Color = Colors.Gray;
                    ATTACK.Background = timeout_background;
                    timer_label.Foreground = timeout_color;
                    timer_label.Content = "時間切れ！！";
                    attack_label.Foreground = timeout_attack_color;
                    attack_label.Content = "時間切れ！！";

                    click_image_change();
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    click_image_change();
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    if (is_sudden_death == false) //サドンデス戦でない場合はFinishedメソッド実行（サドンデス戦の場合はすでに実行しているため二重実行にならないようにする対策）
                    {
                        await finished(attack_counts);
                    }
                    else
                    {
                        sudden_death_progress = false; //サドンデス戦終了
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Terminated!");
                }

            };

            tournament_tree.MouseDown += delegate (object sender, MouseButtonEventArgs args)
            {
                tree_dismiss = true; //ツリーの確認を伝える
            };

            BGM_Trigger.Click += delegate (object sender, RoutedEventArgs args)
            {
                if (BGM_Trigger.IsChecked == true)
                {
                    soundplayer.gameBGM(); //別クラスで作ったBattleBGMを再生
                    System.Diagnostics.Debug.WriteLine("BGM: Enable");
                }
                else
                {
                    soundplayer.stop(); //別クラスで作ったBGM再生停止メソッドを実行
                    System.Diagnostics.Debug.WriteLine("BGM: Disable");
                }
            };

            ATTACK.MouseDown += async delegate (object sender, MouseButtonEventArgs args)
            {
                if (gamemode == "endless" | game_time != 0) // | でorという意味になる。
                {
                    System.Diagnostics.Debug.WriteLine("Attack!");
                    attack_counts += 1; //1加算
                    count_label.Content = attack_counts.ToString();
                    if (select_se.SelectedIndex != 0)
                    {
                        ClickMe.Sound.soundeffects.attack_SE(select_se.SelectedIndex); //クラスライブラリ(.dll) で作成した関数にComboBoxで選んだインデックス番号を渡してSEを再生してもらう。（インデックス番号と効果音はクラスライブラリ側で配列として対応済み）
                    }

                }

                if (attack_counts >= 999999999) //Int型ではこの桁数が限界のようだｗｗｗ（まあここまでクリックする人なんていないだろうけど…ねｗ)
                {
                    System.IO.Stream strmyes = Properties.Resources.gya_n; //Properties.Resources.ファイル名（拡張子なし）リソースファイルの取得はStreamオブジェクトで取得する。
                    ClickMe.Sound.soundeffects.SEplay(strmyes);

                    if (gamemode == "endless")
                    {
                        var count_stop = System.Windows.MessageBox.Show($"な、なんてこった…。カンストするまでクリックし続けるプレイヤーが居るとは…。\nこ、これは逃げるしかねぇ！！", "Click Me!! - カンストしました", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else if(gamemode == "time_attack")
                    {
                        var count_stop = System.Windows.MessageBox.Show($"お、おいおいおい！１分以内にこんなクリックできるのか！？\nどうなってんだアンタ！！ギネス行けるよ！！", "Click Me!! - カンストしました", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else if(gamemode == "tournament")
                    {
                        var count_stop = System.Windows.MessageBox.Show($"ちょいちょいちょい！そんなクリックされたらさすがのコンピューターでも勝てっこねえよ！\nここにエントリーしたコンピューターの中でそんな超人コンピューターいなかったぞ！？\nアンタにとっちゃこんなトーナメント話にならねえだろ…中止しよう、こりゃだめだわ…。", "Click Me!! - プレイヤーがオーバースペックすぎます", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    await back_to_menu(); //終了
                }
            };
            exit_button.Click += async delegate (object sender, RoutedEventArgs args)
            {
                await back_to_menu();
            };
        }
    }
}
