using System;

namespace Crawler.ConsoleApp
{
    public class ConsoleWrapper
    {
        public virtual string ReadLine()
        {
            var input = Console.ReadLine();

            return input;
        }

        public virtual void WtiteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
