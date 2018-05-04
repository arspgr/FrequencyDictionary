using System.IO;
using FrequencyDictionary.Validators.Interfaces;

namespace FrequencyDictionary.Validators
{
    /// <summary>
    /// Валидация существования файла
    /// </summary>
    /// <exception cref="FileNotFoundException"></exception>
    public class FileExistsValidator : IFileExistsValidator
    {
        public void CheckFileExists(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
                throw new FileNotFoundException($"Файл [{fileName}] не найден.");
        }
    }
}