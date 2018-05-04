using System.Collections.Concurrent;
using System.Linq;
using FrequencyDictionary.Services.Interfaces;

namespace FrequencyDictionary.Services
{
    public class LineProcessor : ILineProcessor
    {
        public void ProcessLine(string line, ConcurrentDictionary<string, int> wordsDict)
        {
            var words = line.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s));
            foreach (var word in words)
            {
                wordsDict.AddOrUpdate(word, 1, (s, i) => i + 1);
            }
        }
    }
}