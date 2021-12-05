using EmptyCache.Lib;
using EmptyCache.Lib.Models;

namespace EmptyCache
{
    class Program
    {
        static void Main(string[] args)
        {
            Service s = new Service();
            s.LogEvent += Log;
            s.Execute();
        }

        static void Log(object sender, LogEventArgs e)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine(e);
        }
    }
}