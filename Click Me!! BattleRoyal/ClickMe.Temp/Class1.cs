namespace ClickMe.Temp
{
    public class temp
    {
        public static bool BGM = true;
        public static int Sound = 0;
        public static void doBGM(bool? set) //型の末尾に?を入れるとNull許容型になる。
        {
            BGM = set ?? true; // ??演算子：左辺がNullの場合右辺の値を入れる という演算子。IsCheckedがNull許容型のため、Nullだった場合はtrueを非許容型に入れる。Nullでなければ普通にset変数の値が入る。
        }

        public static void doSound(int set)
        {
            Sound = set;
        }
    }
}