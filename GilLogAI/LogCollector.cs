using GilLogAI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GilLogAI
{
    public class LogCollector
    {
        private List<LineHolder> _singleLogLines;
        
        private Dictionary<int, List<SimilarLogHolder>> _similarLogDictionary;

        public LogCollector()
        {
            _singleLogLines = new List<LineHolder>();
            _similarLogDictionary = new Dictionary<int, List<SimilarLogHolder>>();
        }

        public void LoadNewLogs(List<string> logLines)
        {
            if (logLines == null)
            {
                return;
            }
            foreach (string line in logLines)
            {
                LineHolder logLine = new LineHolder(line);
                AddLogLine(logLine);
            }
        }

        private void AddLogLine(LineHolder logLine)
        {
            List<SimilarLogHolder> similarLogList;
            if (_similarLogDictionary.TryGetValue(logLine.SplitWords.Length, out similarLogList))
            { // check if the new entry is in known sentence
                foreach (var similarLog in similarLogList)
                {
                    if (similarLog.CheckSimilar(logLine.LineWithouDate))
                    {
                        similarLog.ConsumeLog(logLine);
                        return;
                    }
                }
            }
            for (int i = 0; i < _singleLogLines.Count; i++)
            { // check if there a sentence similar to the new one
                Regex regex;
                LineHolder.Similarity similar = _singleLogLines[i].CheckSimilar(logLine, out regex);
                if (similar == LineHolder.Similarity.Similar)
                {
                    SimilarLogHolder similarLogHolder = new SimilarLogHolder(regex, _singleLogLines[i]);
                    similarLogHolder.ConsumeLog(logLine);
                    if (similarLogList == null)
                    {
                        _similarLogDictionary.Add(logLine.SplitWords.Length, new List<SimilarLogHolder>());
                    }
                    _similarLogDictionary[logLine.SplitWords.Length].Add(similarLogHolder);
                    return;
                }
                if (similar == LineHolder.Similarity.Same)
                {
                    // added the line to the log as they are the same sentence in a diffrent date
                    return;
                }
            }
            _singleLogLines.Add(logLine);
        }

        public void PrintInvestigatorResult()
        {

            Console.Out.WriteLine("===");
            foreach (List<SimilarLogHolder> similarLogs in _similarLogDictionary.Values)
            {
                foreach (SimilarLogHolder similarLog in similarLogs)
                {
                    similarLog.PrintResult();
                }
            }
            Console.Out.WriteLine("===");
        }
    }
}
