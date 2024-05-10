using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Click_Me_BattleRoyal
{
    internal class title_color
    {
        // どこで使えるか メンバの種類 持つ型 名前(あれば引数として使う型 その変数名)
        public static LinearGradientBrush b1_color() 
        {
            // ポイント時の背景を描くためのグラデーション素材、LinearGradientBrushを作成します。
            LinearGradientBrush b1_color = new LinearGradientBrush(
                Color.FromRgb(255, 98, 98), //始点
                Color.FromRgb(255, 178, 252), //終点
                new Point(0.5, 0), //始点の位置x,y
                new Point(0.5, 1)); //終点の位置x,y

            return b1_color; //呼び出し元に返す中身

        }

        public static LinearGradientBrush b1_color2()
        {
            // 非ポイント時の背景を描くためのグラデーション素材、LinearGradientBrushを作成します。
            LinearGradientBrush b1_color2 = new LinearGradientBrush(
                Color.FromRgb(255, 178, 252),
                Color.FromRgb(255, 98, 98),
                new Point(0.5, 0), //始点の位置x,y (%)
                new Point(0.5, 1)); //終点の位置x,y (%)

            return b1_color2; //呼び出し元に返す中身

        }

        public static LinearGradientBrush b2_color()
        {
            // ポイント時の背景を描くためのグラデーション素材、LinearGradientBrushを作成します。
            LinearGradientBrush b2_color = new LinearGradientBrush(
                Color.FromRgb(139, 255, 88),
                Color.FromRgb(219, 255, 0),
                new Point(0.5, 0), //始点の位置x,y
                new Point(0.5, 1)); //終点の位置x,y

            return b2_color; //呼び出し元に返す中身

        }

        public static LinearGradientBrush b2_color2()
        {
            // ポイント時の背景を描くためのグラデーション素材、LinearGradientBrushを作成します。
            LinearGradientBrush b2_color2 = new LinearGradientBrush(
                Color.FromRgb(219, 255, 0), 
                Color.FromRgb(139, 255, 88), 
                new Point(0.5, 0), //始点の位置x,y
                new Point(0.5, 1)); //終点の位置x,y

            return b2_color2; //呼び出し元に返す中身

        }

        public static LinearGradientBrush b3_color()
        {
            // ポイント時の背景を描くためのグラデーション素材、LinearGradientBrushを作成します。
            LinearGradientBrush b3_color = new LinearGradientBrush(
                Color.FromRgb(82, 86, 255), 
                Color.FromRgb(58, 193, 255), 
                new Point(0.5, 0), //始点の位置x,y
                new Point(0.5, 1)); //終点の位置x,y

            return b3_color; //呼び出し元に返す中身

        }

        public static LinearGradientBrush b3_color2()
        {
            // ポイント時の背景を描くためのグラデーション素材、LinearGradientBrushを作成します。
            LinearGradientBrush b3_color2 = new LinearGradientBrush(
                Color.FromRgb(58, 193, 255),
                Color.FromRgb(82, 86, 255),
                new Point(0.5, 0), //始点の位置x,y
                new Point(0.5, 1)); //終点の位置x,y

            return b3_color2; //呼び出し元に返す中身

        }
    }
}
