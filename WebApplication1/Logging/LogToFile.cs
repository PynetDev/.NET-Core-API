namespace WebApplication1.Logging
{
    public class LogToFile : IMyLogger
    {
        public void log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("LogToFile");
        }
    }
}
