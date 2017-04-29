using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GilLogAI
{
    public class FileReader : IDisposable
    {
        private const Int32 _bufferSize = 4096;
        private readonly int _linesToRead;
        private FileStream _fileStream;
        private StreamReader _streamReader;
        private string _fileName;

        public FileReader(string fileName, int linesToRead)
        {
            _fileName = fileName;
            _linesToRead = linesToRead;
        }

        public void OpenFile()
        {
            Dispose();
            _fileStream = File.OpenRead(_fileName);
            _streamReader = new StreamReader(_fileStream, Encoding.UTF8, true, _bufferSize);
        }

        /// <summary>
        /// read part of the files line by line
        /// </summary>
        /// <param name="lines">The lines that were read</param>
        /// <returns>Threre any more lines to read</returns>
        public bool ReadLines(out List<string> lines)
        {
            lines =new List<string>();
            for (int i = 0; i < _linesToRead; i++)
            {
                string line = _streamReader.ReadLine();
                if (line == null)
                {
                    return false;
                }
                lines.Add(line);
            }
            return true;
        }

        public void Dispose()
        {
            if (_fileStream != null)
            {
                _fileStream.Dispose();
                _fileStream = null;
            }
            if (_streamReader != null)
            {
                _streamReader.Dispose();
                _streamReader = null;
            }
        }
    }
}
