using System.Threading.Tasks;
using FrequencyDictionary.Services.Interfaces;
using FrequencyDictionary.Settings.Interfaces;
using FrequencyDictionary.Validators.Interfaces;
using NLog;

namespace FrequencyDictionary.Services
{
    public class MainService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly IFileNameSettings _fileNameSettings;
        private readonly IFileExistsValidator _validator;
        private readonly IFileProcessor _fileProcessor;
        private readonly IFileWriterService _fileWriter;

        public MainService(IFileNameSettings fileNameSettings, IFileExistsValidator validator,
            IFileProcessor fileProcessor, IFileWriterService fileWriter)
        {
            _fileNameSettings = fileNameSettings;
            _validator = validator;
            _fileProcessor = fileProcessor;
            _fileWriter = fileWriter;
        }

        public async Task Start()
        {
            Log.Info("Валидация входных параметров");
            _validator.CheckFileExists(_fileNameSettings.InputFileName);

            _fileProcessor.Process(_fileNameSettings.InputFileName);
            await _fileWriter.WriteAsync(_fileProcessor.GetWordsDictionary(), _fileNameSettings.OutputFileName);
        }
    }
}