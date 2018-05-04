using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FrequencyDictionary.Services.Interfaces;
using NLog;

namespace FrequencyDictionary.Services
{
    public class FileProcessor : IFileProcessor
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ConcurrentDictionary<string, int> _words = new ConcurrentDictionary<string, int>();

        private readonly ILineProcessor _lineProcessor;

        public FileProcessor(ILineProcessor lineProcessor)
        {
            _lineProcessor = lineProcessor;
        }

        public void Process(string fileName)
        {
            Log.Info("Чтение входного файла.");
            Parallel.ForEach(File.ReadLines(fileName, Encoding.GetEncoding("Windows-1251")),
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                }, line => _lineProcessor.ProcessLine(line, _words));
            Log.Info("Чтение входного файла завершено.");
        }

        public IDictionary<string, int> GetWordsDictionary() => _words;
    }
}