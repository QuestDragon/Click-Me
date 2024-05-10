using NAudio.Wave;
using System;

namespace ClickMe.Sound
{
    public class soundeffects  //internal = 同プロジェクト内で使用可能、 public = 別プロジェクトからでも使用可能
    {
        private NAudio.Wave.WaveOut? player; //効果音再生用オブジェクト (?を型の最後につけてNull許容型にしている)

        public static void attack_SE(int comBoxSelIndex)
        {
            //配列（リスト型）宣言： 「型の種類[] 名前 = new 型の種類[数];
            Stream[] attack_sounds = new Stream[26]; //入れられる数量は1から数えるが、リストの番号指定(添字という)は0から行う。その点に注意。

            //いざ、配列に音声ファイルを格納だ！
            attack_sounds[0] = Properties.SE_Resources.Click;
            attack_sounds[1] = Properties.SE_Resources.attack;
            attack_sounds[2] = Properties.SE_Resources.attack2;
            attack_sounds[3] = Properties.SE_Resources.attack3;
            attack_sounds[4] = Properties.SE_Resources.Attacked;
            attack_sounds[5] = Properties.SE_Resources.Attacking;
            attack_sounds[6] = Properties.SE_Resources.bakyun;
            attack_sounds[7] = Properties.SE_Resources.bishi1;
            attack_sounds[8] = Properties.SE_Resources.bishi2;
            attack_sounds[9] = Properties.SE_Resources.bomb;
            attack_sounds[10] = Properties.SE_Resources.bomb1;
            attack_sounds[11] = Properties.SE_Resources.bomb2;
            attack_sounds[12] = Properties.SE_Resources.Cut1;
            attack_sounds[13] = Properties.SE_Resources.Cut2;
            attack_sounds[14] = Properties.SE_Resources.glass1;
            attack_sounds[15] = Properties.SE_Resources.glass2;
            attack_sounds[16] = Properties.SE_Resources.glass3;
            attack_sounds[17] = Properties.SE_Resources.mario_attack;
            attack_sounds[18] = Properties.SE_Resources.mario_coin;
            attack_sounds[19] = Properties.SE_Resources.Money_Register;
            attack_sounds[20] = Properties.SE_Resources.oh;
            attack_sounds[21] = Properties.SE_Resources.pistol;
            attack_sounds[22] = Properties.SE_Resources.pon;
            attack_sounds[23] = Properties.SE_Resources.punch;
            attack_sounds[24] = Properties.SE_Resources.shotgun;
            attack_sounds[25] = Properties.SE_Resources.yeah;

            if (comBoxSelIndex == 1)
            {
                Random random = new Random();
                int rnd = random.Next(attack_sounds.Length);
                SEplay(attack_sounds[rnd]); //再生
            }
            else
            {
                SEplay(attack_sounds[comBoxSelIndex - 2]); //2個位置がズレてるのでそのズレを補正
            }
        }

        public static void SEplay(Stream strm)
        {
            var sound = new WaveFileReader(strm); //Streamオブジェクトを読み込む。今回は拡張子がWAVEファイルなのでWaveFileReaderメソッド。（MP3の場合はMp3FileReaderメソッドが使える）
            var SE_player = new WaveOutEvent(); //音声ファイルコントロール用オブジェクトを作成
            SE_player.Init(sound);
            SE_player.Play();

        }

    }
}