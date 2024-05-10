using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Click_Me_BattleRoyal
{
    internal class game_tournament_anim //internal = 同プロジェクト内で使用可能、 public = 別プロジェクトからでも使用可能
    {
        //別クラスからコントロールオブジェクトを参照するにはメソッドの引数に対象のページ名またはウィンドウ名を指定する必要がある。
        //その後、対象のページまたはウィンドウのxaml.csからメソッドを実行し、引数内にthisを指定すると別クラスからでもコントロールオブジェクトが参照できる。※説明じゃわかんない？ソースコード読めばわかる！ｗｗｗ
        //一応参考サイト：https://social.msdn.microsoft.com/Forums/ja-JP/a78a14c8-3575-46a7-aa09-5df0aa5bfdfc/wpf1239512390122892102912398xaml125011244912452125231239812467125?forum=csharpgeneralja

        static Dictionary<string, TranslateTransform> targets= new Dictionary<string, TranslateTransform>(); //ターゲットのオブジェクトを格納する辞書型を作成。Staticのためこのクラス内の他のメソッドから参照可能。

        public static void target_init(game game, List<string> names) //names = COM名（最大7つ）
        {
            targets.Clear();//初期化

            TranslateTransform[] players = new TranslateTransform[8];
            players[0] = game.game_player_pos;
            players[1] = game.com1_pos;
            players[2] = game.com2_pos;
            players[3] = game.com3_pos;
            players[4] = game.com4_pos;
            players[5] = game.com5_pos;
            players[6] = game.com6_pos;
            players[7] = game.com7_pos;

            targets.Add("あなた", game.game_player_pos); //プレイヤー
            for (int i = 0; i < names.Count; i++)
            {
                targets.Add(names[i], players[i + 1]); // 0番目のnames ＝ 1番目のCOM名、1番目のplayers_pos = com1_pos
            }
        }

        private static TranslateTransform target(string name) //オブジェクトを返してくれるメソッド
        {
            return targets[name]; //オブジェクトを返す
        }

        static Dictionary<string, Border> raw_targets = new Dictionary<string, Border>(); //ターゲットのオブジェクトを格納する辞書型を作成。Staticのためこのクラス内の他のメソッドから参照可能。
        public static void raw_target_init(game game, List<string> names) //names = COM名（最大7つ）
        {
            raw_targets.Clear();//初期化

            Border[] players = new Border[8]; //数の決まっている配列
            players[0] = game.game_player;
            players[1] = game.com1;
            players[2] = game.com2;
            players[3] = game.com3;
            players[4] = game.com4;
            players[5] = game.com5;
            players[6] = game.com6;
            players[7] = game.com7;

            raw_targets.Add("あなた", game.game_player); //プレイヤー
            for (int i = 0; i < names.Count; i++)
            {
                raw_targets.Add(names[i], players[i + 1]); // 0番目のnames ＝ 1番目のCOM名、1番目のplayers_pos = com1_pos
            }
        }

        private static Border raw_target(string name) //オブジェクトを返してくれるメソッド
        {
            return raw_targets[name]; //オブジェクトを返す
        }


        private static List<double> X_pos_phase1 = new List<double>(); //8等分した各エリアの中心点を格納
        private static async void phase1_win(game game, string winner, string side) //同プロジェクト内でどこでも 呼び出し可 返り値なし(オブジェクトが格納されている対象ページ名 変数名)
        {
            var win_player = target(winner); // Translate Transform
            var raw_win_player = raw_target(winner); // Border

            var general_h = MainWindow.win_h * 0.7417;
            var ajaster = ((game.game_window.Width * 0.0575) - 46) + 40; //ズレをBorderのWidthで補正

            //アニメーション作成
            DoubleAnimation y1 = new DoubleAnimation();
            y1.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            y1.To = -(MainWindow.win_h * 0.1667);
            //TransformプロパティにあらかじめXAMLで名前をつけ、そのプロパティに直接アニメーションを実行。(StoryBoardを介していないアニメーション。Storyboardは複数のアニメーションを行うのに向いているようだ。)
            win_player.BeginAnimation(TranslateTransform.YProperty, y1); //メソッド引数の変数名【ターゲットページ名またはウィンドウ名】.ターゲットオブジェクト.BeginAnimation(対象プロパティ,対象アニメーション)
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            win_player.BeginAnimation(TranslateTransform.YProperty, null);
            win_player.Y = -(MainWindow.win_h * 0.1677);

            if (side == "A") //ABlock
            {
                ThicknessAnimation x1 = new ThicknessAnimation(); 
                x1.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                x1.To = new Thickness(X_pos_phase1[0] - (ajaster / 2), general_h, 0, 0);
                raw_win_player.BeginAnimation(Border.MarginProperty, x1);
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                raw_win_player.BeginAnimation(Border.MarginProperty, null); //終わったらアニメーションをなし（NULL）にしておく ※じゃないとあとから手動（位置補正）または別のアニメーションでプロパティを変更できない
                raw_win_player.Margin = new Thickness(X_pos_phase1[0] - (ajaster / 2), general_h, 0, 0); //アニメーション後の値を手動で設定しておく
            }
            else if (side == "B")
            {
                ThicknessAnimation x1 = new ThicknessAnimation();
                x1.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                x1.To = new Thickness(X_pos_phase1[1] - (ajaster / 2), general_h, 0, 0);
                raw_win_player.BeginAnimation(Border.MarginProperty, x1);
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                raw_win_player.BeginAnimation(Border.MarginProperty, null);
                raw_win_player.Margin = new Thickness(X_pos_phase1[1] - (ajaster / 2), general_h, 0, 0);
            }
            else if (side == "C")
            {
                ThicknessAnimation x1 = new ThicknessAnimation();
                x1.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                x1.To = new Thickness(X_pos_phase1[2] - (ajaster / 2), general_h, 0, 0);
                raw_win_player.BeginAnimation(Border.MarginProperty, x1);
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                raw_win_player.BeginAnimation(Border.MarginProperty, null) ;
                raw_win_player.Margin = new Thickness(X_pos_phase1[2] - (ajaster / 2), general_h, 0, 0);
            }
            else if (side == "D")
            {
                ThicknessAnimation x1 = new ThicknessAnimation();
                x1.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                x1.To = new Thickness(X_pos_phase1[3] - (ajaster / 2), general_h, 0, 0);
                raw_win_player.BeginAnimation(Border.MarginProperty, x1);
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                raw_win_player.BeginAnimation(Border.MarginProperty, null);
                raw_win_player.Margin = new Thickness(X_pos_phase1[3] - (ajaster / 2), general_h, 0, 0);
            }

            DoubleAnimation y2 = new DoubleAnimation();
            y2.Duration = new Duration(TimeSpan.FromSeconds(1));
            y2.To = -(MainWindow.win_h * 0.2667);
            win_player.BeginAnimation(TranslateTransform.YProperty, y2);
            await Task.Delay(TimeSpan.FromSeconds(1));
            win_player.BeginAnimation(TranslateTransform.YProperty, null);
            win_player.Y = -(MainWindow.win_h * 0.2667);
        }

        private static List<double> X_pos_phase2 = new List<double>(); //8等分した各エリアの中心点を格納
        private static async void phase2_win(game game, string winner, string align) 
        {
            var win_player = target(winner);
            var raw_win_player = raw_target(winner);

            var general_h = MainWindow.win_h * 0.7417;
            var ajaster = ((game.game_window.Width * 0.0575) - 46) + 40; //ズレをBorderのWidthで補正

            if (align == "L")
            {
                ThicknessAnimation x2 = new ThicknessAnimation(); 
                x2.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                x2.To = new Thickness(X_pos_phase2[0] - (ajaster / 2), general_h, 0, 0);
                raw_win_player.BeginAnimation(Border.MarginProperty, x2);
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                raw_win_player.BeginAnimation(Border.MarginProperty, null); //終わったらアニメーションをなし（NULL）にしておく ※じゃないとあとから手動（位置補正）または別のアニメーションでプロパティを変更できない
                raw_win_player.Margin = new Thickness(X_pos_phase2[0] - (ajaster / 2), general_h, 0, 0); //アニメーション後の値を手動で設定しておく

            }
            else if (align == "R")
            {
                ThicknessAnimation x2 = new ThicknessAnimation(); 
                x2.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                x2.To = new Thickness(X_pos_phase2[1] - (ajaster / 2), general_h, 0, 0);
                raw_win_player.BeginAnimation(Border.MarginProperty, x2);
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                raw_win_player.BeginAnimation(Border.MarginProperty, null);
                raw_win_player.Margin = new Thickness(X_pos_phase2[1] - (ajaster / 2), general_h, 0, 0);

            }

            DoubleAnimation y1 = new DoubleAnimation();
            y1.Duration = new Duration(TimeSpan.FromSeconds(1));
            y1.To = -(game.game_window.Height * 0.4833);
            win_player.BeginAnimation(TranslateTransform.YProperty, y1);
            await Task.Delay(TimeSpan.FromSeconds(1));
            win_player.BeginAnimation(TranslateTransform.YProperty, null);
            win_player.Y = -(game.game_window.Height * 0.4833);

        }
        private static async void Winner(game game, string winner)
        {
            var win_player = target(winner);
            var raw_win_player = raw_target(winner);

            var general_h = MainWindow.win_h * 0.7417;
            var ajaster = ((game.game_window.Width * 0.0575) - 46) + 40; //ズレをBorderのWidthで補正

            ThicknessAnimation x3 = new ThicknessAnimation(); 
            x3.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            x3.To = new Thickness((game.game_window.Width / 2) - (ajaster / 2), general_h, 0, 0); //2等分して中心を求める
            raw_win_player.BeginAnimation(Border.MarginProperty, x3);
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            raw_win_player.BeginAnimation(Border.MarginProperty, null);
            raw_win_player.Margin = new Thickness((game.game_window.Width / 2) - (ajaster / 2), general_h, 0, 0);

            DoubleAnimation y1 = new DoubleAnimation();
            y1.Duration = new Duration(TimeSpan.FromSeconds(1));
            y1.To = -(game.game_window.Height * 0.5667);
            win_player.BeginAnimation(TranslateTransform.YProperty, y1);
            await Task.Delay(TimeSpan.FromSeconds(1));
            win_player.BeginAnimation(TranslateTransform.YProperty, null);
            win_player.Y = -(game.game_window.Height * 0.5667);

        }

        //これらのオブジェクトはPublic Staticのため別クラスの他のメソッドから参照可能。
        public static Dictionary<string, TranslateTransform> winners = new Dictionary<string, TranslateTransform>(); //勝利したターゲットのY方向を決めるTranslateTransformオブジェクトを格納する辞書型を作成。
        public static Dictionary<string, Border> raw_winners = new Dictionary<string, Border>(); //勝利したターゲットのX方向を決めるBorderオブジェクトを格納する辞書型を作成。
        public static Dictionary<string, string> winners_pos = new Dictionary<string, string>(); //ターゲットのオブジェクトの位置情報を格納する辞書型を作成。
        public static Dictionary<string, int> winners_phase = new Dictionary<string, int>(); //ターゲットの勝利した回数をオブジェクト名と結びつけて格納する辞書型を作成。
        public static List<string> winners_name = new List<string>(); //勝利したターゲットの名前を格納する。
        public static void judgement(game game, int phase, int player_score, List<int> score, List<string> name) //void = 戻り値なし
        {
            
            //比較用
            var score1 = 0;
            var score2 = 0;

            if (phase == 1)
            {
                var X_base = game.game_window.Width / 8; //8等分した結果を格納
                for (int phase1_column = 1; phase1_column <= 7; phase1_column += 2) //カウント用の数が奇数になるようにする。 For文の意味： カウント変数と初期値、条件式、カウント方法
                {
                    X_pos_phase1.Add(X_base * phase1_column); //8等分したうちの奇数等分のWidth位置を記録
                }

                //1回目のときのみ初期化
                winners.Clear();
                raw_winners.Clear();
                winners_name.Clear();
                winners_pos.Clear();
                winners_phase.Clear();

                score1 = score[5];
                score2 = score[6];
                if (score1 > score2)
                {
                    phase1_win(game, name[5], "D");
                    winners_name.Add(name[5]);
                    winners.Add(name[5], target(name[5]));
                    raw_winners.Add(name[5], raw_target(name[5]));
                    winners_pos.Add(name[5], "D");
                    winners_phase.Add(name[5], phase);
                }
                else
                {
                    phase1_win(game, name[6], "D");
                    winners_name.Add(name[6]);
                    winners.Add(name[6], target(name[6]));
                    raw_winners.Add(name[6], raw_target(name[6]));
                    winners_pos.Add(name[6], "D");
                    winners_phase.Add(name[6], phase);
                }

                score1 = score[3];
                score2 = score[4];
                if (score1 > score2)
                {
                    phase1_win(game, name[3], "C");
                    winners_name.Add(name[3]);
                    winners.Add(name[3], target(name[3]));
                    raw_winners.Add(name[3], raw_target(name[3]));
                    winners_pos.Add(name[3], "C");
                    winners_phase.Add(name[3], phase);
                }
                else
                {
                    phase1_win(game, name[4], "C");
                    winners_name.Add(name[4]);
                    winners.Add(name[4], target(name[4]));
                    raw_winners.Add(name[4], raw_target(name[4]));
                    winners_pos.Add(name[4], "C");
                    winners_phase.Add(name[4], phase);

                }

                score1 = score[1];
                score2 = score[2];
                if (score1 > score2)
                {
                    phase1_win(game, name[1], "B");
                    winners_name.Add(name[1]);
                    winners.Add(name[1], target(name[1]));
                    raw_winners.Add(name[1], raw_target(name[1]));
                    winners_pos.Add(name[1], "B");
                    winners_phase.Add(name[1], phase);
                }
                else
                {
                    phase1_win(game, name[2], "B");
                    winners_name.Add(name[2]);
                    winners.Add(name[2], target(name[2]));
                    raw_winners.Add(name[2], raw_target(name[2]));
                    winners_pos.Add(name[2], "B");
                    winners_phase.Add(name[2], phase);
                }

                score1 = player_score;
                score2 = score[0];
                if (score1 > score2)
                {
                    phase1_win(game, "あなた", "A");
                    winners_name.Add("あなた");
                    winners.Add("あなた", target("あなた"));
                    raw_winners.Add("あなた", raw_target("あなた"));
                    winners_pos.Add("あなた", "A");
                    winners_phase.Add("あなた", phase);
                }
                else
                {
                    phase1_win(game, name[0], "A");
                    winners_name.Add(name[0]);
                    winners.Add(name[0], target(name[0]));
                    raw_winners.Add(name[0], raw_target(name[0]));
                    winners_pos.Add(name[0], "A");
                    winners_phase.Add(name[0], phase);
                }


            }
            else if(phase == 2)
            {
                X_pos_phase2.Add(game.game_window.Width / 4); //4等分したうちの1番目のWidth位置を記録
                X_pos_phase2.Add((game.game_window.Width / 4) * 3); //4等分したうちの3番目のWidth位置を記録

                score1 = score[1];
                score2 = score[2];
                if (score1 > score2)
                {
                    phase2_win(game, name[1], "R");
                    winners_phase[name[1]] = phase; //進出したのでPhaseを編集
                }
                else
                {
                    phase2_win(game, name[2], "R");
                    winners_phase[name[2]] = phase;
                }

                score1 = player_score;
                score2 = score[0];
                if (score1 > score2)
                {
                    phase2_win(game, "あなた", "L");
                    winners_phase["あなた"] = phase;
                }
                else
                {
                    phase2_win(game, name[0], "L");
                    winners_phase[name[0]] = phase;
                }

            }
            else if(phase == 3)
            {
                score1 = player_score;
                score2 = score[0];
                if (score1 > score2)
                {
                    Winner(game, "あなた");
                    winners_phase["あなた"] = phase;
                }
                else
                {
                    Winner(game, name[0]);
                    winners_phase[name[0]] = phase;
                }

            }
        }

    }
}
