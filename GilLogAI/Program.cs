using System;
using System.Collections.Generic;

namespace GilLogAI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogCollector logCollector = new LogCollector();
            string path = Environment.CurrentDirectory;
            string file = "\\..\\..\\logfile.txt";
            int linesToRead = 128;
            ConsumeLog(logCollector, path + file, linesToRead);
            linesToRead = 2;
            logCollector = new LogCollector();
            ConsumeLog(logCollector, path + file, linesToRead);


            logCollector = new LogCollector();

            file = "\\..\\..\\logfilePart1.txt";
            ConsumeLog(logCollector, path + file, linesToRead);
            file = "\\..\\..\\logfilePart2.txt";
            ConsumeLog(logCollector, path + file, linesToRead);
        }

        public static void ConsumeLog(LogCollector logCollector, string file, int linesToRead)
        {
            using (var fileReader = new FileReader(file, linesToRead))
            {
                fileReader.OpenFile();
                List<string> lines;
                bool haveLines = true;
                while (haveLines)
                {
                    haveLines = fileReader.ReadLines(out lines);
                    logCollector.LoadNewLogs(lines);
                }
            }
            logCollector.PrintInvestigatorResult();
        }
    }
}
