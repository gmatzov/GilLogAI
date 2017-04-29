using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GilLogAI.Model
{
    public class LineHolder
    {
        private const string RegexString = @"\d{2}-\d{2}-\d{4} \d{2}:\d{2}:\d{2}\s*";
        // This regex will help to remove the date from a sentence
        private static Regex DateRegex = new Regex(RegexString);

        public int DifferentWordAt { get; private set; }
        public string Line { get; private set; }

        /// <summary>
        /// Will hold same sentence with a diffrent date
        /// </summary>
        public List<string> SameSentence { get; private set; }

        public string LineWithouDate { get; private set; }

        public string[] SplitWords{get; private set;}

        public LineHolder(string line)
        {
            SameSentence = null;
            Line = line;
            LineWithouDate = DateRegex.Replace(line, "");
            SplitWords = Regex.Split(LineWithouDate, @"[,\s]+");
        }

        public enum Similarity
        {
            Similar,
            NotSimilar,
            Same,
        };

        /// <summary>
        /// Check if two Lines are similar
        /// </summary>
        /// <param name="otherLine">The compared line</param>
        /// <param name="regex">regex that will help to determine other words</param>
        /// <returns>Similarity</returns>
        public Similarity CheckSimilar(LineHolder otherLine, out Regex regex)
        {
            regex = null;
            if ((otherLine == null) || (SplitWords.Length != otherLine.SplitWords.Length))
            {
                return Similarity.NotSimilar;
            }
            int differentWords = 0;
            int differentWordAt = 0;
            for (int i = 0; i < SplitWords.Length; i++)
            { //check how many similar words exists.
                if (!SplitWords[i].Equals(otherLine.SplitWords[i]))
                {
                    differentWords++;
                    differentWordAt = i;
                    if (differentWords > 1)
                    {
                        return Similarity.NotSimilar;
                    }
                }
            }
            if (differentWords == 0) // the same sentence
            {
                if (SameSentence == null)
                {
                    SameSentence = new List<string>();
                }
                SameSentence.Add(otherLine.Line);
                return Similarity.Same;
            }
            else // must be 1
            {
                DifferentWordAt = differentWordAt;
                otherLine.DifferentWordAt = differentWordAt; 
                regex = CreateRegexForSimilar();
                return Similarity.Similar;
            }
        }

        private Regex CreateRegexForSimilar()
        {
            List<string> sentence = new List<string>();
            for (int i = 0; i < SplitWords.Length; i++)
            {
                if (i == DifferentWordAt)
                {
                    sentence.Add("\\w+");                    
                }
                else
                {
                    sentence.Add(SplitWords[i]);
                }
            }
            return new Regex(string.Join(" ", sentence));
        }
    }
}
