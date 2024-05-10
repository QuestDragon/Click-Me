using DiscordRPC;
using DiscordRPC.Logging;

namespace ClickMe.DiscordRPC
{
    public class RP //public class 別プロジェクトからでも変数やメソッドを使用可能
    {
        private static DiscordRpcClient client;
        static RichPresence presence = new RichPresence();


        // アプリケーションの初回起動時に呼び出されます。
        //たとえば、メイン ループの直前で、Unity の OnEnable をオンにします。
        public static void Initialize()
        {
            /*
            Discord クライアントを作成する
            注: Unity3D を使用している場合は、完全なコンストラクターを使用してパイプ接続を定義する必要があります。
            */
            client = new DiscordRpcClient("1077046242359652383");

            //ロガーを設定する
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            //イベントを登録する
            client.OnReady += (sender, e) =>
            {
                Console.WriteLine($"ユーザー {e.User.Username} から Ready を受け取りました");
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine($"アップデートを取得しました! {e.Presence}");
            };

            //RPC に接続する
            client.Initialize();

            //アイドルステータスに設定
            idle();

        }

        // アプリケーションの終了時に呼び出されます。
        //たとえば、メイン ループの直後、OnDisable for unity.
        public static void Deinitialize()
        {
            client.Dispose();
        }

        //リッチプレゼンスを設定
        //コード内の任意の場所で、これを何度でも呼び出します。
        public static void idle()
        {
            presence.Details = "アイドル状態";
            presence.State = null;

            presence.Buttons = new Button[]
            {
                new Button() { Label = "ダウンロード(準備中)", Url = "https://github.com/QuestDragon" },
                new Button() { Label = "サポートサーバー", Url = "https://discord.gg/Z63n9htNTv" }
            };

            presence.Assets = new Assets()
            {
                LargeImageKey = "discorddevicon",
                LargeImageText = "Click Me!! - Battle Royale",
                SmallImageKey = "idle",
                SmallImageText = "idle"
            };
            presence.Timestamps = null;

            //用意したRPCを適用
            client.SetPresence(presence);

        }

        public static void uptimer()
        {
            presence.Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow
                //End = DateTime.UtcNow + TimeSpan.FromSeconds(15)
            };
        }
        public static void downtimer(int time)
        {
            presence.Timestamps = new Timestamps()
            {
                //Start = DateTime.UtcNow
                End = DateTime.UtcNow + TimeSpan.FromSeconds(time)
            };
        }


        public static void endless(int count)
        {
            //RPCオブジェクトを用意
            presence.Details = "エンドレスモードをプレイ中";
            presence.State = $"{count} 回クリックしました";

            presence.Buttons = new Button[]
            {
                new Button() { Label = "ダウンロード", Url = "https://lachee.dev/" },
                new Button() { Label = "サポートサーバー", Url = "https://discord.gg/Z63n9htNTv" }
            };

            presence.Assets = new Assets()
            {
                LargeImageKey = "discorddevicon",
                LargeImageText = "Click Me!! - Battle Royale",
                SmallImageKey = "endless",
                SmallImageText = "playing endless mode"
            };

            //用意したRPCを適用
            client.SetPresence(presence);

        }
        public static void time_attack(int count, int time)
        {

            presence.Details = $"{time}秒タイムアタックモードをプレイ中";
            presence.State = $"{count} 回クリックしました";

            presence.Buttons = new Button[]
            {
                new Button() { Label = "ダウンロード", Url = "https://lachee.dev/" },
                new Button() { Label = "サポートサーバー", Url = "https://discord.gg/Z63n9htNTv" }
            };

            presence.Assets = new Assets()
            {
                LargeImageKey = "discorddevicon",
                LargeImageText = "Click Me!! - Battle Royale",
                SmallImageKey = "time_attack",
                SmallImageText = $"playing time attack mode (class {time}s)"
            };

            client.SetPresence(presence);

        }

        public static void tournament(int count, int time, string phase)
        {

            presence.Details = $"{time}秒トーナメントモードをプレイ中";
            presence.State = $"{count} 回クリックしました / {phase}戦";

            presence.Buttons = new Button[]
            {
                new Button() { Label = "ダウンロード", Url = "https://lachee.dev/" },
                new Button() { Label = "サポートサーバー", Url = "https://discord.gg/Z63n9htNTv" }
            };

            presence.Assets = new Assets()
            {
                LargeImageKey = "discorddevicon",
                LargeImageText = "Click Me!! - Battle Royale",
                SmallImageKey = "tournament",
                SmallImageText = $"playing tournament mode (class {time}s)"
            };

            client.SetPresence(presence);

        }

    }
}