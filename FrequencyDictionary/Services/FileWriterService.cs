using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrequencyDictionary.Services.Interfaces;
using NLog;

namespace FrequencyDictionary.Services
{
    public class FileWriterService : IFileWriterService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public async Task WriteAsync(IDictionary<string, int> words, string fileName)
        {
            Log.Info($"Запись результата в файл: [{fileName}]");
            var encoding = Encoding.GetEncoding("Windows-1251");
            var parallelism = Environment.ProcessorCount;
            using (var stream = File.OpenWrite(fileName))
            {
                var tasks = new List<Task>();
                var partSize = words.Count / parallelism + 1;
                for (var i = 0; i < parallelism; i++)
                {
                    var textPart = string.Join("\n",
                        words
                            .OrderByDescending(pair => pair.Value)
                            .Take(partSize * (i + 1)).Skip(partSize * i)
                            .Select(pair => $"{pair.Key},{pair.Value}"));

                    if (!string.IsNullOrWhiteSpace(textPart) && i != 0) textPart = $"\n{textPart}";

                    var bytes = encoding.GetBytes(textPart);
                    if (bytes.Length != 0)
                        tasks.Add(stream.WriteAsync(bytes, 0, bytes.Length));
                }

                await Task.WhenAll(tasks);
            }
            Log.Info("Запись завершена");
        }
    }
}