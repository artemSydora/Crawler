using System.Collections.Generic;
using System.Linq;
using ConsoleTables;
using Crawler.Logic.Models;

namespace Crawler.ConsoleApp
{
    class Display
    {
        private readonly ConsoleWrapper _consoleWrapper;

        public Display(ConsoleWrapper consoleWrapper)
        {
            _consoleWrapper = consoleWrapper;
        }

        public void ShowTable(string tableName, IEnumerable<Link> links, string columnName)
        {
            _consoleWrapper.WtiteLine(tableName);

            var count = 1;

            var table = new ConsoleTable(new string[] { columnName });
            table.Options.EnableCount = false;

            links.ToList()
                 .ForEach(link => table.AddRow($"{count++,4}) {link.Url}"));

            table.Write();
        }

        public void ShowTable(string tableName, IEnumerable<Ping> pings, string column1Name, string column2Name)
        {
            _consoleWrapper.WtiteLine(tableName);

            var count = 1;

            var table = new ConsoleTable(new string[] { column1Name, column2Name });
            table.Options.EnableCount = false;

            pings.ToList()
                 .ForEach(link => table.AddRow($"{ count++,4}) {link.Url}", $"{link.ResponseTime} ms."));

            table.Write();
        }
    }
}

