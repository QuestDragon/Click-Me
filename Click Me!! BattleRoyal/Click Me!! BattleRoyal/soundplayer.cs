using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Click_Me_BattleRoyal
{
    internal class soundplayer //internal = 同プロジェクト内で使用可能、 public = 別プロジェクトからでも使用可能
    {
        static System.Media.SoundPlayer? bgm = null; //BGM再生用オブジェクトを作成。入れるものがないのでNull許容型。（SoundPlayerクラスは性質上1つの音声ファイルしか一度に再生できない）

        public static void titleBGM()
        {
            //オーディオリソースを取り出す
            //(リソースへのアクセス：ソリューションプロパティ【プロジェクト＞Click Me!! BattleRoyalのプロパティ】＞リソース) ※Resources.resxの保存を忘れずに…保存し忘れると文法エラーになる
            System.IO.Stream strmbgm = Properties.Resources.Title_BGM; //Properties.Resources.ファイル名（拡張子なし）リソースファイルの取得はStreamオブジェクトで取得する。
            play(strmbgm);
        }
        public static void gameBGM()
        {
            //オーディオリソースを取り出す
            //(リソースへのアクセス：ソリューションプロパティ【プロジェクト＞Click Me!! BattleRoyalのプロパティ】＞リソース) ※Resources.resxの保存を忘れずに…保存し忘れると文法エラーになる
            System.IO.Stream strmbgm = Properties.Resources.BATTLE; //Properties.Resources.ファイル名（拡張子なし）リソースファイルの取得はStreamオブジェクトで取得する。
            play(strmbgm);
        }
        public static void winBGM()
        {
            //オーディオリソースを取り出す
            //(リソースへのアクセス：ソリューションプロパティ【プロジェクト＞Click Me!! BattleRoyalのプロパティ】＞リソース) ※Resources.resxの保存を忘れずに…保存し忘れると文法エラーになる
            System.IO.Stream strmbgm = Properties.Resources.winner; //Properties.Resources.ファイル名（拡張子なし）リソースファイルの取得はStreamオブジェクトで取得する。
            play(strmbgm);
        }
        public static void loseBGM()
        {
            //オーディオリソースを取り出す
            //(リソースへのアクセス：ソリューションプロパティ【プロジェクト＞Click Me!! BattleRoyalのプロパティ】＞リソース) ※Resources.resxの保存を忘れずに…保存し忘れると文法エラーになる
            System.IO.Stream strmbgm = Properties.Resources.Moment; //Properties.Resources.ファイル名（拡張子なし）リソースファイルの取得はStreamオブジェクトで取得する。
            play(strmbgm);
        }

        static void play(Stream strm)
        {
            bgm = new System.Media.SoundPlayer(strm);
            bgm.PlayLooping();
        }

        public static void stop()
        {
            try
            {
                bgm.Stop();
            } 
            catch (NullReferenceException)  //何も再生されてなかった場合
            {
                System.Diagnostics.Debug.WriteLine("BGMは再生されていませんでした。");
            }
            
        }


    }
}
