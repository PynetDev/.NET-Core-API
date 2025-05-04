namespace WebApplication1.Logging
{
    public class LogToServerMemory : IMyLogger
    {
        public void log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("LogToServerMemory");
        }
    }
}
