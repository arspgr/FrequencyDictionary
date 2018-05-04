using System.Collections.Concurrent;

namespace FrequencyDictionary.Services.Interfaces
{
    public interface ILineProcessor
    {
        void ProcessLine(string line, ConcurrentDictionary<string, int> wordsDict);
    }
}