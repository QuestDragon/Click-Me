namespace clickme.logger
{
    public class log_system
    {
        public static void logger(string message)
        {
            Console.WriteLine(message);
        }

        public static void debugger(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}