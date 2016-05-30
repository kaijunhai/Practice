using System;
using LogEntryDemo.Logger;

namespace LogEntryDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new Log4NetLogger();
            Console.WriteLine("Writing Log 1");
            logger.Error("log error 1");
            logger.Informational("log infor 1");
            Console.ReadKey();
        }
    }
}
