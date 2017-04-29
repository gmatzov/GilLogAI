using GilLogAI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GilLogAI
{
    public class SimilarLogHolder
    {

        private List<string> _sentences;
        // regex to check if the sentences are similiar
        private Regex _similarCheck;
        // The chnging words
        private HashSet<string> _changingWords;
        // The changing word place
        private int _changingWordAt;

        public SimilarLogHolder(Regex similarCheck, LineHolder logLine)
        {
            _changingWordAt = logLine.DifferentWordAt;
            _similarCheck = similarCheck;
            _sentences = new List<string>();
            _changingWords = new HashSet<string>();
            _changingWords.Add(logLine.SplitWords[_changingWordAt]);
            _sentences.Add(logLine.Line);
            if (logLine.SameSentence != null)
            {
                _sentences.AddRange(logLine.SameSentence);
            }
        }

        /// <summary>
        /// check if the sentence without the date is similar
        /// </summary>
        /// <param name="line">sentence without the date</param>
        /// <returns>similar or not</returns>
        public bool CheckSimilar(string line)
        {
            return _similarCheck.Match(line).Success;
        }

        /// <summary>
        /// Consume a similar log
        /// </summary>
        /// <param name="logLine">The log that we are going to consume</param>
        public void ConsumeLog(LineHolder logLine)
        {
            _sentences.Add(logLine.Line);
            _changingWords.Add(logLine.SplitWords[_changingWordAt]);
            if (logLine.SameSentence != null)
            {
                _sentences.AddRange(logLine.SameSentence);
            }
        }

        public void PrintResult()
        {
            foreach (string line in _sentences)
            {
                Console.Out.WriteLine(line);
            }
            Console.Out.WriteLine("The changing word was: " + string.Join(", ", _changingWords));

            Console.Out.WriteLine("");
        }
    }
}
