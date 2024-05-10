using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MouseEventArgs = System.Windows.Input.MouseEventArgs; //あいまいになるのでMouseEventArgsと書かれたときはInput側のMouseEventArgsであるとここで言っておく。

namespace Click_Me_BattleRoyal
{
    /// <summary>
    /// title.xaml の相互作用ロジック
    /// </summary>
    public partial class title : Page
    {
        static title this_base = new title();
        private static double margin = 800;

        public static void title_resize(double width, double height)
        {
            margin = width;

            if (this_base.select_game.Margin != new Thickness(0, 0, 0, 0))
            {
                this_base.select_game.Margin = new Thickness(0, 0, margin, 0);
            }
            if (this_base.select_time.Margin != new Thickness(0, 0, 0, 0))
            {
                this_base.select_time.Margin = new Thickness(margin, 0, 0, 0);
            }

            this_base.title_window.Width = width - 40;
            this_base.title_window.Height = height - 40;
            this_base.logo.Width = width - 40;
            this_base.logo.Height = height - 40;

        } //void型では;を必要としない

        private static void logo_expand_anim(title title)
        {
            //ロゴズームアニメーション作成
            DoubleAnimation icon_expand_H = new DoubleAnimation();
            icon_expand_H.Duration = new Duration(TimeSpan.FromSeconds(1)); //1秒
            icon_expand_H.To = MainWindow.win_h - 40;
            DoubleAnimation icon_expand_W = new DoubleAnimation();
            icon_expand_W.Duration = new Duration(TimeSpan.FromSeconds(1)); //1秒
            icon_expand_W.To = MainWindow.win_w - 40;

            Storyboard.SetTargetName(icon_expand_W, this_base.logo.Name); //適用するアニメーション名と適用先のオブジェクト名指定
            Storyboard.SetTargetProperty(icon_expand_W, new PropertyPath(Image.WidthProperty)); //適用先プロパティ（PropertyPathは「オブジェクトのクラス名.プロパティ名」で記述する）
            Storyboard.SetTargetName(icon_expand_H, this_base.logo.Name); //適用するアニメーション名と適用先のオブジェクト名指定
            Storyboard.SetTargetProperty(icon_expand_H, new PropertyPath(Image.HeightProperty)); //適用先プロパティ（PropertyPathは「オブジェクトのクラス名.プロパティ名」で記述する）

            Storyboard logo_expand = new Storyboard();
            logo_expand.Children.Add(icon_expand_W);
            logo_expand.Children.Add(icon_expand_H);

            logo_expand.Begin(title);
        }
        private static void logo_shrink_anim(title title)
        {
            //ロゴズームアニメーション作成
            DoubleAnimation icon_shrink_H = new DoubleAnimation();
            icon_shrink_H.Duration = new Duration(TimeSpan.FromSeconds(0.3)); //0.3秒
            icon_shrink_H.To = MainWindow.win_h - 150;
            DoubleAnimation icon_shrink_W = new DoubleAnimation();
            icon_shrink_W.Duration = new Duration(TimeSpan.FromSeconds(0.3)); //0.3秒
            icon_shrink_W.To = MainWindow.win_w - 150;

            Storyboard.SetTargetName(icon_shrink_W, this_base.logo.Name); //適用するアニメーション名と適用先のオブジェクト名指定
            Storyboard.SetTargetProperty(icon_shrink_W, new PropertyPath(Image.WidthProperty)); //適用先プロパティ（PropertyPathは「オブジェクトのクラス名.プロパティ名」で記述する）
            Storyboard.SetTargetName(icon_shrink_H, this_base.logo.Name); //適用するアニメーション名と適用先のオブジェクト名指定
            Storyboard.SetTargetProperty(icon_shrink_H, new PropertyPath(Image.HeightProperty)); //適用先プロパティ（PropertyPathは「オブジェクトのクラス名.プロパティ名」で記述する）

            Storyboard logo_shrink = new Storyboard();
            logo_shrink.Children.Add(icon_shrink_W);
            logo_shrink.Children.Add(icon_shrink_H);

            logo_shrink.Begin(title);

        }

        public static void select_menu_anim(string target)
        {
            //ゲームモード選択移動アニメーション
            ThicknessAnimation gamemode_out = new ThicknessAnimation(); //Staticにはできないので個別で新しく作る
            gamemode_out.Duration = new Duration(TimeSpan.FromSeconds(1));
            gamemode_out.To = new Thickness(0, 0, margin, 0);
            //50%加減速
            gamemode_out.AccelerationRatio = 0.5;
            gamemode_out.DecelerationRatio = 0.5;

            ThicknessAnimation gamemode_in = new ThicknessAnimation();
            gamemode_in.Duration = new Duration(TimeSpan.FromSeconds(1));
            gamemode_in.To = new Thickness(0, 0, 0, 0);
            //50%加減速
            gamemode_in.AccelerationRatio = 0.5;
            gamemode_in.DecelerationRatio = 0.5;

            //タイマー選択移動アニメーション
            ThicknessAnimation time_select_in = new ThicknessAnimation();
            time_select_in.Duration = new Duration(TimeSpan.FromSeconds(1));
            time_select_in.From = new Thickness(margin, 0, 0, 0);
            time_select_in.To = new Thickness(0, 0, 0, 0);
            //50%加減速
            time_select_in.AccelerationRatio = 0.5;
            time_select_in.DecelerationRatio = 0.5;
            //タイマー選択移動アニメーション
            ThicknessAnimation time_select_out = new ThicknessAnimation();
            time_select_out.Duration = new Duration(TimeSpan.FromSeconds(1));
            time_select_out.To = new Thickness(margin, 0, 0, 0);
            //50%加減速
            time_select_out.AccelerationRatio = 0.5;
            time_select_out.DecelerationRatio = 0.5;
            //タイマー選択移動アニメーション
            ThicknessAnimation time_selected = new ThicknessAnimation();
            time_selected.Duration = new Duration(TimeSpan.FromSeconds(1));
            time_selected.To = new Thickness(0, 0, margin, 0);
            //50%加減速
            time_selected.AccelerationRatio = 0.5;
            time_selected.DecelerationRatio = 0.5;

            Storyboard.SetTargetName(gamemode_out, this_base.select_game.Name);
            Storyboard.SetTargetProperty(gamemode_out, new PropertyPath(MarginProperty));
            Storyboard.SetTargetName(gamemode_in, this_base.select_game.Name);
            Storyboard.SetTargetProperty(gamemode_in, new PropertyPath(MarginProperty));
            Storyboard.SetTargetName(time_select_in, this_base.select_time.Name);
            Storyboard.SetTargetProperty(time_select_in, new PropertyPath(MarginProperty));
            Storyboard.SetTargetName(time_select_out, this_base.select_time.Name);
            Storyboard.SetTargetProperty(time_select_out, new PropertyPath(MarginProperty));
            Storyboard.SetTargetName(time_selected, this_base.select_time.Name);
            Storyboard.SetTargetProperty(time_selected, new PropertyPath(MarginProperty));

            Storyboard timer_select_anim = new Storyboard();
            timer_select_anim.Children.Add(gamemode_out);
            timer_select_anim.Children.Add(time_select_in);
            Storyboard timer_sel_back_anim = new Storyboard();
            timer_sel_back_anim.Children.Add(gamemode_in);
            timer_sel_back_anim.Children.Add(time_select_out);
            Storyboard timer_selected_anim = new Storyboard();
            timer_selected_anim.Children.Add(time_selected);

            if (target == "timer_select_anim")
            {
                timer_select_anim.Begin(this_base);
            }
            else if(target == "timer_sel_back_anim")
            {
                timer_sel_back_anim.Begin(this_base);
            }
            else if(target == "timer_selected_anim")
            {
                timer_selected_anim.Begin(this_base);
            }

        }


        public title()
        {
            InitializeComponent();
            NameScope.SetNameScope(this, new NameScope()); //ネームスコープ作成
            this_base = this;

            //(ダブルクオーテーションでくくった名前がストーリーボードオブジェクトの適用先オブジェクト名で使用される。
            //ストーリーボードオブジェクトの適用先オブジェクト名を「〇〇.name」で指定する場合はダブルクオーテーションでくくる名前をXAMLで決めたオブジェクト名(Name)と同じにすること！「良い例：("test", test)」「悪い例：("test_object", test)」)
            this.RegisterName("fadeFX", fadeFX); //フェードイン用のRecrangleを登録
            this.RegisterName("EndfadeFX", EndfadeFX); //フェードアウト用のRecrangleを登録
            this.RegisterName("logo", logo); //スタートボタンを登録
            this.RegisterName("select_game", select_game); //ゲームセレクトボタンの下地、Gridを登録(アニメーションとして使うため)
            this.RegisterName("endless", endless); //エンドレスボタンを登録（アニメーションとして使うため）
            this.RegisterName("time_attack", time_attack); //タイムアタックボタンを登録（アニメーションとして使うため）
            this.RegisterName("Tournament", Tournament); //トーナメントボタンを登録（アニメーションとして使うため）
            this.RegisterName("select_time", select_time); //タイマー選択ボタンの下地、Gridを登録（アニメーション用）
            this.RegisterName("second10", second10); //エンドレスボタンを登録（アニメーションとして使うため）
            this.RegisterName("second30", second30); //タイムアタックボタンを登録（アニメーションとして使うため）
            this.RegisterName("second60", second60); //トーナメントボタンを登録（アニメーションとして使うため）

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


            //ゲームセレクト画面の表示アニメーション
            DoubleAnimation select_menu_anim_data = new DoubleAnimation();
            select_menu_anim_data.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            select_menu_anim_data.From = 0;
            select_menu_anim_data.To = 1;
            select_menu_anim_data.DecelerationRatio = 0.5; //50％部分で減速

            //ボタンの拡大アニメーション
            DoubleAnimation button_expand1 = new DoubleAnimation();
            button_expand1.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_expand1.To = 450;
            button_expand1.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_expand2 = new DoubleAnimation();
            button_expand2.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_expand2.To = 450;
            button_expand2.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_expand3 = new DoubleAnimation();
            button_expand3.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_expand3.To = 450;
            button_expand3.DecelerationRatio = 0.5; //50%で減速

            DoubleAnimation button_expand4 = new DoubleAnimation();
            button_expand4.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_expand4.To = 200;
            button_expand4.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_expand5 = new DoubleAnimation();
            button_expand5.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_expand5.To = 100;
            button_expand5.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_expand6 = new DoubleAnimation();
            button_expand6.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_expand6.To = 200;
            button_expand6.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_expand7 = new DoubleAnimation();
            button_expand7.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_expand7.To = 100;
            button_expand7.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_expand8 = new DoubleAnimation();
            button_expand8.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_expand8.To = 200;
            button_expand8.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_expand9 = new DoubleAnimation();
            button_expand9.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_expand9.To = 100;
            button_expand9.DecelerationRatio = 0.5; //50%で減速

            //ボタンの縮小アニメーション
            DoubleAnimation button_shrink1 = new DoubleAnimation();
            button_shrink1.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_shrink1.To = 400;
            button_shrink1.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_shrink2 = new DoubleAnimation();
            button_shrink2.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_shrink2.To = 400;
            button_shrink2.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_shrink3 = new DoubleAnimation();
            button_shrink3.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_shrink3.To = 400;
            button_shrink3.DecelerationRatio = 0.5; //50%で減速

            DoubleAnimation button_shrink4 = new DoubleAnimation();
            button_shrink4.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_shrink4.To = 150;
            button_shrink4.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_shrink5 = new DoubleAnimation();
            button_shrink5.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_shrink5.To = 60;
            button_shrink5.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_shrink6 = new DoubleAnimation();
            button_shrink6.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_shrink6.To = 150;
            button_shrink6.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_shrink7 = new DoubleAnimation();
            button_shrink7.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_shrink7.To = 60;
            button_shrink7.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_shrink8 = new DoubleAnimation();
            button_shrink8.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_shrink8.To = 150;
            button_shrink8.DecelerationRatio = 0.5; //50%で減速
            DoubleAnimation button_shrink9 = new DoubleAnimation();
            button_shrink9.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            button_shrink9.To = 60;
            button_shrink9.DecelerationRatio = 0.5; //50%で減速

            //ボタンの余白移動アニメーション
            ThicknessAnimation time_button1_anim = new ThicknessAnimation();
            time_button1_anim.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            time_button1_anim.To = new Thickness(0, 0, 400, 0);
            time_button1_anim.DecelerationRatio = 0.5;
            ThicknessAnimation time_button1_reset = new ThicknessAnimation();
            time_button1_reset.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            time_button1_reset.To = new Thickness(0, 0, 300, 0);
            time_button1_reset.DecelerationRatio = 0.5;
            ThicknessAnimation time_button2_animL = new ThicknessAnimation();
            time_button2_animL.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            time_button2_animL.To = new Thickness(0,0,100,0);
            time_button2_animL.DecelerationRatio = 0.5;
            ThicknessAnimation time_button2_animR = new ThicknessAnimation();
            time_button2_animR.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            time_button2_animR.To = new Thickness(100,0,0,0);
            time_button2_animR.DecelerationRatio = 0.5;
            ThicknessAnimation time_button2_reset = new ThicknessAnimation();
            time_button2_reset.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            time_button2_reset.To = new Thickness(0,0,0,0);
            time_button2_reset.DecelerationRatio = 0.5;
            ThicknessAnimation time_button3_anim = new ThicknessAnimation();
            time_button3_anim.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            time_button3_anim.To = new Thickness(400,0,0,0);
            time_button3_anim.DecelerationRatio = 0.5;
            ThicknessAnimation time_button3_reset = new ThicknessAnimation();
            time_button3_reset.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            time_button3_reset.To = new Thickness(300,0,0,0);
            time_button3_reset.DecelerationRatio = 0.5;


            //ストーリーボードオブジェクトを作成してアニメーションを割当
            Storyboard.SetTargetName(intro_fade, fadeFX.Name);
            Storyboard.SetTargetProperty(intro_fade, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(outro_fade, fadeFX.Name);
            Storyboard.SetTargetProperty(outro_fade, new PropertyPath(Rectangle.OpacityProperty));


            Storyboard.SetTargetName(select_menu_anim_data, select_game.Name);
            Storyboard.SetTargetProperty(select_menu_anim_data, new PropertyPath(Grid.OpacityProperty));

            Storyboard.SetTargetName(button_expand1, endless.Name);
            Storyboard.SetTargetProperty(button_expand1, new PropertyPath(WidthProperty));
            Storyboard.SetTargetName(button_shrink1, endless.Name);
            Storyboard.SetTargetProperty(button_shrink1, new PropertyPath(WidthProperty));
            Storyboard.SetTargetName(button_expand2, time_attack.Name);
            Storyboard.SetTargetProperty(button_expand2, new PropertyPath(WidthProperty));
            Storyboard.SetTargetName(button_shrink2, time_attack.Name);
            Storyboard.SetTargetProperty(button_shrink2, new PropertyPath(WidthProperty));
            Storyboard.SetTargetName(button_expand3, Tournament.Name);
            Storyboard.SetTargetProperty(button_expand3, new PropertyPath(WidthProperty));
            Storyboard.SetTargetName(button_shrink3, Tournament.Name);
            Storyboard.SetTargetProperty(button_shrink3, new PropertyPath(WidthProperty));

            Storyboard.SetTargetName(button_expand4, second10.Name);
            Storyboard.SetTargetProperty(button_expand4, new PropertyPath(WidthProperty));
            Storyboard.SetTargetName(button_expand5, second10.Name);
            Storyboard.SetTargetProperty(button_expand5, new PropertyPath(HeightProperty));
            Storyboard.SetTargetName(button_shrink4, second10.Name);
            Storyboard.SetTargetProperty(button_shrink4, new PropertyPath(WidthProperty));
            Storyboard.SetTargetName(button_shrink5, second10.Name);
            Storyboard.SetTargetProperty(button_shrink5, new PropertyPath(HeightProperty));
            Storyboard.SetTargetName(button_expand6, second30.Name);
            Storyboard.SetTargetProperty(button_expand6, new PropertyPath(WidthProperty));
            Storyboard.SetTargetName(button_expand7, second30.Name);
            Storyboard.SetTargetProperty(button_expand7, new PropertyPath(HeightProperty));
            Storyboard.SetTargetName(button_shrink6, second30.Name);
            Storyboard.SetTargetProperty(button_shrink6, new PropertyPath(WidthProperty));
            Storyboard.SetTargetName(button_shrink7, second30.Name);
            Storyboard.SetTargetProperty(button_shrink7, new PropertyPath(HeightProperty));
            Storyboard.SetTargetName(button_expand8, second60.Name);
            Storyboard.SetTargetProperty(button_expand8, new PropertyPath(WidthProperty));
            Storyboard.SetTargetName(button_expand9, second60.Name);
            Storyboard.SetTargetProperty(button_expand9, new PropertyPath(HeightProperty));
            Storyboard.SetTargetName(button_shrink8, second60.Name);
            Storyboard.SetTargetProperty(button_shrink8, new PropertyPath(WidthProperty));
            Storyboard.SetTargetName(button_shrink9, second60.Name);
            Storyboard.SetTargetProperty(button_shrink9, new PropertyPath(HeightProperty));

            Storyboard.SetTargetName(time_button1_anim, second10.Name);
            Storyboard.SetTargetProperty(time_button1_anim, new PropertyPath(MarginProperty));
            Storyboard.SetTargetName(time_button1_reset, second10.Name);
            Storyboard.SetTargetProperty(time_button1_reset, new PropertyPath(MarginProperty));
            Storyboard.SetTargetName(time_button2_animL, second30.Name);
            Storyboard.SetTargetProperty(time_button2_animL, new PropertyPath(MarginProperty));
            Storyboard.SetTargetName(time_button2_animR, second30.Name);
            Storyboard.SetTargetProperty(time_button2_animR, new PropertyPath(MarginProperty));
            Storyboard.SetTargetName(time_button2_reset, second30.Name);
            Storyboard.SetTargetProperty(time_button2_reset, new PropertyPath(MarginProperty));
            Storyboard.SetTargetName(time_button3_anim, second60.Name);
            Storyboard.SetTargetProperty(time_button3_anim, new PropertyPath(MarginProperty));
            Storyboard.SetTargetName(time_button3_reset, second60.Name);
            Storyboard.SetTargetProperty(time_button3_reset, new PropertyPath(MarginProperty));

            //Storyboard.SetTargetName(gamemode_in, select_game.Name);
            //Storyboard.SetTargetProperty(gamemode_in, new PropertyPath(MarginProperty));
            //Storyboard.SetTargetName(gamemode_out, select_game.Name);
            //Storyboard.SetTargetProperty(gamemode_out, new PropertyPath(MarginProperty));
            //Storyboard.SetTargetName(time_select_in, select_time.Name);
            //Storyboard.SetTargetProperty(time_select_in, new PropertyPath(MarginProperty));
            //Storyboard.SetTargetName(time_select_out, select_time.Name);
            //Storyboard.SetTargetProperty(time_select_out, new PropertyPath(MarginProperty));
            //Storyboard.SetTargetName(time_selected, select_time.Name);
            //Storyboard.SetTargetProperty(time_selected, new PropertyPath(MarginProperty));


            //ストーリーボード作成
            Storyboard intro_fade_anim = new Storyboard();
            intro_fade_anim.Children.Add(intro_fade);
            Storyboard outro_fade_anim = new Storyboard();
            outro_fade_anim.Children.Add(outro_fade);


            Storyboard select_menu = new Storyboard();
            select_menu.Children.Add(select_menu_anim_data);

            Storyboard expand_anim1 = new Storyboard();
            expand_anim1.Children.Add(button_expand1);
            Storyboard expand_anim2 = new Storyboard();
            expand_anim2.Children.Add(button_expand2);
            Storyboard expand_anim3 = new Storyboard();
            expand_anim3.Children.Add(button_expand3);
            Storyboard shrink_anim1 = new Storyboard();
            shrink_anim1.Children.Add(button_shrink1);
            Storyboard shrink_anim2 = new Storyboard();
            shrink_anim2.Children.Add(button_shrink2);
            Storyboard shrink_anim3 = new Storyboard();
            shrink_anim3.Children.Add(button_shrink3);

            Storyboard time10_expand = new Storyboard();
            time10_expand.Children.Add(button_expand4);
            time10_expand.Children.Add(button_expand5);
            time10_expand.Children.Add(time_button2_animR);
            time10_expand.Children.Add(time_button3_anim);
            Storyboard time10_shrink = new Storyboard();
            time10_shrink.Children.Add(button_shrink4);
            time10_shrink.Children.Add(button_shrink5);
            time10_shrink.Children.Add(time_button2_reset);
            time10_shrink.Children.Add(time_button3_reset);
            Storyboard time30_expand = new Storyboard();
            time30_expand.Children.Add(button_expand6);
            time30_expand.Children.Add(button_expand7);
            time30_expand.Children.Add(time_button1_anim);
            time30_expand.Children.Add(time_button3_anim);
            Storyboard time30_shrink = new Storyboard();
            time30_shrink.Children.Add(button_shrink6);
            time30_shrink.Children.Add(button_shrink7);
            time30_shrink.Children.Add(time_button1_reset);
            time30_shrink.Children.Add(time_button3_reset);
            Storyboard time60_expand = new Storyboard();
            time60_expand.Children.Add(button_expand8);
            time60_expand.Children.Add(button_expand9);
            time60_expand.Children.Add(time_button1_anim);
            time60_expand.Children.Add(time_button2_animL);
            Storyboard time60_shrink = new Storyboard();
            time60_shrink.Children.Add(button_shrink8);
            time60_shrink.Children.Add(button_shrink9);
            time60_shrink.Children.Add(time_button1_reset);
            time60_shrink.Children.Add(time_button2_reset);

            //Storyboard timer_select_anim= new Storyboard();
            //timer_select_anim.Children.Add(gamemode_out);
            //timer_select_anim.Children.Add(time_select_in);
            //Storyboard timer_sel_back_anim = new Storyboard();
            //timer_sel_back_anim.Children.Add(gamemode_in);
            //timer_sel_back_anim.Children.Add(time_select_out);
            //Storyboard timer_selected_anim = new Storyboard();
            //timer_selected_anim.Children.Add(time_selected);


            title_window.Loaded += async delegate (object sender, RoutedEventArgs args)
            {
                title_resize(MainWindow.win_w, MainWindow.win_h); //ロード時に一度調整

                MainWindow.mw_data.this_window.Width = double.NaN; //ウィンドウ広がり抑制を解除(game画面からの遷移時)
                MainWindow.mw_data.this_window.Height = double.NaN;

                ClickMe.DiscordRPC.RP.idle();
                //自分自身のバージョン情報を取得する
                Version.Content = $"v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version} - Stable";

                fadeFX.Visibility = Visibility.Visible;
                logo.Width  = MainWindow.win_w - 150;
                logo.Height = MainWindow.win_h - 150;
                logo_expand_anim(this) ; //ロードできた時点でロゴのズームアニメーションを開始しておく
                intro_fade_anim.Begin(this);
                await Task.Delay(TimeSpan.FromSeconds(1));
                fadeFX.Visibility = Visibility.Hidden;

                //ループ再生
                soundplayer.titleBGM(); //別クラスで作ったタイトルBGMを流す

                logo_shrink_anim(this);
                await Task.Delay(TimeSpan.FromSeconds(0.3));
                while (true){ //無限ループ
                    logo_expand_anim(this);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    logo_shrink_anim(this);
                    await Task.Delay(TimeSpan.FromSeconds(0.3));
                }
            };


            // Brush は FrameworkElement ではないので、設定する Name プロパティはありません。その代わりに名前を登録するだけです。
            this.RegisterName("b1_color", title_color.b1_color()); //別クラスファイルに作ってある関数を使う
            this.RegisterName("b1_color2", title_color.b1_color2());
            this.RegisterName("b2_color", title_color.b2_color()); 
            this.RegisterName("b2_color2", title_color.b2_color2());
            this.RegisterName("b3_color", title_color.b3_color()); 
            this.RegisterName("b3_color2", title_color.b3_color2());

            logo.MouseDown += delegate (object sender, MouseButtonEventArgs args) //スタートアイコン（画像）の上でマウスがクリックされたときに実行
            {
                System.Diagnostics.Debug.WriteLine("Clicked");
                //オーディオリソースを取り出す (リソースへのアクセス：ソリューションプロパティ【プロジェクト＞Click Me!! BattleRoyalのプロパティ】＞リソース) ※Resources.resxの保存を忘れずに…保存し忘れると文法エラーになる
                System.IO.Stream strm = Properties.Resources.welcome; //Properties.Resources.ファイル名（拡張子なし） リソースファイルの取得はStreamオブジェクトで取得する。
                ClickMe.Sound.soundeffects.SEplay(strm); //クラスライブラリ(.dll) で作成した関数にストリームオブジェクトを渡してSEを再生してもらう。

                select_game.Visibility = Visibility.Visible; //表示に変更
                select_menu.Begin(this);

            };

            endless.MouseEnter += delegate (object sender, MouseEventArgs args)
            {
                // 別クラスファイルで作ったブラシを関数名で参照し、ウィンドウに背景を描きます。
                endless.Background = title_color.b1_color();

                expand_anim1.Begin(this);
            };
            endless.MouseLeave += delegate (object sender, MouseEventArgs args)
            {

                // 別クラスファイルで作ったブラシを関数名で参照し、ウィンドウに背景を描きます。
                endless.Background = title_color.b1_color2();

                shrink_anim1.Begin(this);
            };

            endless.MouseDown += async delegate (object sender, MouseButtonEventArgs args) //エンドレスボタンがクリックされたとき実行
            {
                // 別クラスファイルで作ったブラシを関数名で参照し、ウィンドウに背景を描きます。
                endless.Background = title_color.b1_color2();

                System.Diagnostics.Debug.WriteLine("Selected: Endless");
                //オーディオリソースを取り出す
                System.IO.Stream strm = Properties.Resources.mode_selected; //Properties.Resources.ファイル名（拡張子なし） リソースファイルの取得はStreamオブジェクトで取得する。
                ClickMe.Sound.soundeffects.SEplay(strm); //クラスライブラリ(.dll) で作成した関数にストリームオブジェクトを渡してSEを再生してもらう。

                fadeFX.Visibility = Visibility.Visible;
                outro_fade_anim.Begin(this);
                await Task.Delay(TimeSpan.FromSeconds(1));
                soundplayer.stop(); //別クラスで作ったBGM再生停止メソッドを実行

                game.mode_check("endless"); //エンドレスモードを伝える

                var game_screen = new game(); //変数名 = new ページ名【xamlファイル名】();
                MainWindow.navigate = true; //ナビゲート許可
                NavigationService.Navigate(game_screen); //アクセス
                MainWindow.navigate = false; //ナビゲートロック
            };

            time_attack.MouseEnter += delegate (object sender, MouseEventArgs args)
            {
                time_attack.Background = title_color.b2_color();
                expand_anim2.Begin(this);
            };
            time_attack.MouseLeave += delegate (object sender, MouseEventArgs args)
            {
                time_attack.Background = title_color.b2_color2();
                shrink_anim2.Begin(this);
            };

            time_attack.MouseDown += delegate (object sender, MouseButtonEventArgs args) //エンドレスボタンがクリックされたとき実行
            {
                time_attack.Background = title_color.b2_color2();

                System.Diagnostics.Debug.WriteLine("Selected: Time Attack");
                //オーディオリソースを取り出す
                System.IO.Stream strm = Properties.Resources.mode_selected; //Properties.Resources.ファイル名（拡張子なし） リソースファイルの取得はStreamオブジェクトで取得する。
                ClickMe.Sound.soundeffects.SEplay(strm); //クラスライブラリ(.dll) で作成した関数にストリームオブジェクトを渡してSEを再生してもらう。

                game.mode_check("time_attack"); //タイムアタックモードを伝える
                select_time.Visibility = Visibility.Visible;
                select_menu_anim("timer_select_anim"); //タイマー選択画面を出す

            };
            Tournament.MouseEnter += delegate (object sender, MouseEventArgs args)
            {
                Tournament.Background = title_color.b3_color();
                expand_anim3.Begin(this);
            };
            Tournament.MouseLeave += delegate (object sender, MouseEventArgs args)
            {
                Tournament.Background = title_color.b3_color2();
                shrink_anim3.Begin(this);
            };

            Tournament.MouseDown += delegate (object sender, MouseButtonEventArgs args) //エンドレスボタンがクリックされたとき実行
            {
                Tournament.Background = title_color.b3_color2();

                System.Diagnostics.Debug.WriteLine("Selected: Tournament");
                //オーディオリソースを取り出す
                System.IO.Stream strm = Properties.Resources.mode_selected; //Properties.Resources.ファイル名（拡張子なし） リソースファイルの取得はStreamオブジェクトで取得する。
                ClickMe.Sound.soundeffects.SEplay(strm); //クラスライブラリ(.dll) で作成した関数にストリームオブジェクトを渡してSEを再生してもらう。

                game.mode_check("tournament"); //タイムアタックモードを伝える
                select_time.Visibility = Visibility.Visible;
                select_menu_anim("timer_select_anim"); //タイマー選択画面を出す

            };

            second10.MouseEnter += delegate (object sender, MouseEventArgs args)
            {
                // 別クラスファイルで作ったブラシを関数名で参照し、ウィンドウに背景を描きます。
                second10.Background = title_color.b1_color();

                time10_expand.Begin(this);
            };
            second10.MouseLeave += delegate (object sender, MouseEventArgs args)
            {

                // 別クラスファイルで作ったブラシを関数名で参照し、ウィンドウに背景を描きます。
                second10.Background = title_color.b1_color2();

                time10_shrink.Begin(this);
            };

            second10.MouseDown += async delegate (object sender, MouseButtonEventArgs args) //エンドレスボタンがクリックされたとき実行
            {
                // 別クラスファイルで作ったブラシを関数名で参照し、ウィンドウに背景を描きます。
                second10.Background = title_color.b1_color2();

                System.Diagnostics.Debug.WriteLine("Selected: 10seconds");
                //オーディオリソースを取り出す
                System.IO.Stream strm = Properties.Resources.time_selected; //Properties.Resources.ファイル名（拡張子なし） リソースファイルの取得はStreamオブジェクトで取得する。
                ClickMe.Sound.soundeffects.SEplay(strm); //クラスライブラリ(.dll) で作成した関数にストリームオブジェクトを渡してSEを再生してもらう。

                fadeFX.Visibility = Visibility.Visible;
                select_menu_anim("timer_selected_anim");
                outro_fade_anim.Begin(this);
                await Task.Delay(TimeSpan.FromSeconds(1));
                soundplayer.stop(); //別クラスで作ったBGM再生停止メソッドを実行

                game.set_timer(10); //タイマーの秒数を伝える
                var game_screen = new game(); //変数名 = new ページ名【xamlファイル名】();
                MainWindow.navigate = true; //ナビゲート許可
                NavigationService.Navigate(game_screen); //アクセス
                MainWindow.navigate = false; //ナビゲートロック
            };

            second30.MouseEnter += delegate (object sender, MouseEventArgs args)
            {
                second30.Background = title_color.b2_color();
                time30_expand.Begin(this);
            };
            second30.MouseLeave += delegate (object sender, MouseEventArgs args)
            {
                time_attack.Background = title_color.b2_color2();
                time30_shrink.Begin(this);
            };

            second30.MouseDown += async delegate (object sender, MouseButtonEventArgs args) //エンドレスボタンがクリックされたとき実行
            {
                second30.Background = title_color.b2_color2();

                System.Diagnostics.Debug.WriteLine("Selected: 30seconds");
                //オーディオリソースを取り出す
                System.IO.Stream strm = Properties.Resources.time_selected; //Properties.Resources.ファイル名（拡張子なし） リソースファイルの取得はStreamオブジェクトで取得する。
                ClickMe.Sound.soundeffects.SEplay(strm); //クラスライブラリ(.dll) で作成した関数にストリームオブジェクトを渡してSEを再生してもらう。

                fadeFX.Visibility = Visibility.Visible;
                select_menu_anim("timer_selected_anim");
                outro_fade_anim.Begin(this);
                await Task.Delay(TimeSpan.FromSeconds(1));
                soundplayer.stop(); //別クラスで作ったBGM再生停止メソッドを実行

                game.set_timer(30); //タイマーの秒数を伝える
                var game_screen = new game(); //変数名 = new ページ名【xamlファイル名】();
                MainWindow.navigate = true; //ナビゲート許可
                NavigationService.Navigate(game_screen); //アクセス
                MainWindow.navigate = false; //ナビゲートロック

            };
            second60.MouseEnter += delegate (object sender, MouseEventArgs args)
            {
                second60.Background = title_color.b3_color();
                time60_expand.Begin(this);
            };
            second60.MouseLeave += delegate (object sender, MouseEventArgs args)
            {
                second60.Background = title_color.b3_color2();
                time60_shrink.Begin(this);
            };

            second60.MouseDown += async delegate (object sender, MouseButtonEventArgs args) //エンドレスボタンがクリックされたとき実行
            {
                second60.Background = title_color.b3_color2();

                System.Diagnostics.Debug.WriteLine("Selected: 60seconds");
                //オーディオリソースを取り出す
                System.IO.Stream strm = Properties.Resources.time_selected; //Properties.Resources.ファイル名（拡張子なし） リソースファイルの取得はStreamオブジェクトで取得する。
                ClickMe.Sound.soundeffects.SEplay(strm); //クラスライブラリ(.dll) で作成した関数にストリームオブジェクトを渡してSEを再生してもらう。

                fadeFX.Visibility = Visibility.Visible;
                select_menu_anim("timer_selected_anim");
                outro_fade_anim.Begin(this);
                await Task.Delay(TimeSpan.FromSeconds(1));
                soundplayer.stop(); //別クラスで作ったBGM再生停止メソッドを実行

                game.set_timer(60); //タイマーの秒数を伝える
                var game_screen = new game(); //変数名 = new ページ名【xamlファイル名】();
                MainWindow.navigate = true; //ナビゲート許可
                NavigationService.Navigate(game_screen); //アクセス
                MainWindow.navigate = false; //ナビゲートロック

            };

            back_button.Click += async delegate (object sender, RoutedEventArgs args)
            {
                System.Diagnostics.Debug.WriteLine("Selected: Back to gamemode menu");
                //オーディオリソースを取り出す
                System.IO.Stream strm = Properties.Resources.back; //Properties.Resources.ファイル名（拡張子なし） リソースファイルの取得はStreamオブジェクトで取得する。
                ClickMe.Sound.soundeffects.SEplay(strm); //クラスライブラリ(.dll) で作成した関数にストリームオブジェクトを渡してSEを再生してもらう。
                select_menu_anim("timer_sel_back_anim");
                await Task.Delay(TimeSpan.FromSeconds(1));
                select_time.Visibility = Visibility.Hidden;
            };


            end_button.Click += async delegate (object sender, RoutedEventArgs args)
            {
                    System.Diagnostics.Debug.WriteLine("Exiting...");
                    await MainWindow.end_app(); //MainWindowクラスファイルに作った関数を実行して終了
            };

            //Borderのズレの原因を調べるためにかつて使っていたコード。
            //test.Click += delegate (object sender, RoutedEventArgs args)
            //{
            //    System.Diagnostics.Debug.WriteLine("Test");
            //    var screen = new Page1(); //変数名 = new ページ名【xamlファイル名】();
            //    NavigationService.Navigate(screen); //アクセス
            //};

        }

    }
}
