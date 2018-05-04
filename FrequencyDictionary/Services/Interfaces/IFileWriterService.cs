using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrequencyDictionary.Services.Interfaces
{
    public interface IFileWriterService
    {
        Task WriteAsync(IDictionary<string, int> words, string fileName);
    }
}