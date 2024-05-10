using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickMe.Tournament
{
    public class Ai_Status  //internal = 同プロジェクト内で使用可能、 public = 別プロジェクトからでも使用可能
    {
        public static List<double> COM_Base(int range) //基礎能力ジェネレータ
        {
            //結果格納用リスト
            var com_bases = new List<double>();

            for(int i = 1; i <= range; i++)
            {
                Random com_level = new Random();
                int per_data10x = com_level.Next(1, 200); //秒間1回～20倍叩ける
                double per_data = per_data10x / 10;
                com_bases.Add(per_data);
            }
            return com_bases; //結果格納用リストを返り値にする
        }

        public static List<int> COM_Score(List<double> Base, int time) //各試合でのスコアデータ生成
        {
            //結果格納用リスト
            var com_scores = new List<int>();

            for (int i = 0; i < Base.Count; i++) //リストの数だけ繰り返す(添字誤差のため0カウントスタートとより小さいで対応)
            {
                Random com_level = new Random();
                int plus_data = com_level.Next(1, 10); //1～10回の誤差
                double base_data = Base[i] * time; //基礎能力と所要時間からスコアを計算
                int score_data = (int)base_data + plus_data; //base_dataをIntに変換

                //ランダム算出したスコア（score_data）がすでにcom_scoresのリストに含まれるかインデックス番号を検索する。
                if (0 < com_scores.IndexOf(score_data) + 1) //返ってきた数字に+1して0より大きければ存在する。存在しない場合は-1が帰るため+1して0になる。0の場合はリストに追加。
                {
                    i -= 1; //iを減らしてもう1回引き直す
                }
                else
                {
                    com_scores.Add(score_data);
                }

            }
            return com_scores; //結果格納用リストを返り値にする

        }

        public static int COM_ReScore(List<double> Base, int time) //サドンデス試合でのスコアデータ再生成
        {

            Random com_level = new Random();
            int plus_data = com_level.Next(1, 10); //1～10回の誤差
            double base_data = Base[0] * time; //基礎能力と所要時間からスコアを計算
            int score_data = (int)base_data + plus_data; //base_dataをIntに変換

            return score_data; //結果を返り値にする

        }

        public static List<int> loser_index = new List<int>();
        public static void COM_delete(int delete_count, List<int> score, ref List<string> name, ref List<double> Base) //void = 戻り値なし、敗者のCOMを消す 引数の型の前に「ref」をつけると参照渡しになる。参照渡しにするとメソッド内の処理で呼び出し元の変数の値を変更できる。
        {
            //delete_countはJudge_Levelと同じ数になっている。1回目のジャッジは1回目のCOM削除と同じになるため同期する。
            //比較用
            var score1 = 0;
            var score2 = 0;
            if (delete_count == 1)
            {
                loser_index.Clear(); //初回実行時は初期化
            }

            if(score.Count > 5)
            {
                score1 = score[5];
                score2 = score[6];
                if (score1 > score2)
                {
                    if(delete_count == 1)
                    {
                        loser_index.Add(6);
                    }
                    
                    name.RemoveAt(6);
                    Base.RemoveAt(6);
                }
                else
                {
                    if (delete_count == 1)
                    {
                        loser_index.Add(5);
                    }
                    name.RemoveAt(5);
                    Base.RemoveAt(5);
                }

            }

            if(score.Count > 3)
            {
                score1 = score[3];
                score2 = score[4];
                if (score1 > score2)
                {
                    if (delete_count == 1)
                    {
                        loser_index.Add(4);
                    }
                    name.RemoveAt(4);
                    Base.RemoveAt(4);
                }
                else
                {
                    if (delete_count == 1)
                    {
                        loser_index.Add(3);
                    }
                    name.RemoveAt(3);
                    Base.RemoveAt(3);
                }
            }

            score1 = score[1];
            score2 = score[2];
            if (score1 > score2)
            {
                if (delete_count == 1)
                {
                    loser_index.Add(2);
                }
                name.RemoveAt(2);
                Base.RemoveAt(2);
            }
            else
            {
                if (delete_count == 1)
                {
                    loser_index.Add(1);
                }
                name.RemoveAt(1);
                Base.RemoveAt(1);
            }

            //この処理が行われるということはプレイヤーは勝ったということ。つまり対戦相手を削除
            if (delete_count == 1)
            {
                loser_index.Add(0);
            }
            name.RemoveAt(0);
            Base.RemoveAt(0);

        }
    }
}
