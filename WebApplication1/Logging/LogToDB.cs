namespace WebApplication1.Logging
{
    public class LogToDB : IMyLogger
    {
        public void log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("LogToDB");
        }
    }
}
