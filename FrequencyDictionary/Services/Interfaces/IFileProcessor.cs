using System.Collections.Generic;

namespace FrequencyDictionary.Services.Interfaces
{
    public interface IFileProcessor
    {
        void Process(string fileName);
        IDictionary<string, int> GetWordsDictionary();
    }
}