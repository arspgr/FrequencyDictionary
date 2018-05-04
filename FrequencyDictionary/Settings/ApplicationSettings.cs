using System;
using FrequencyDictionary.Settings.Interfaces;

namespace FrequencyDictionary.Settings
{
    public class ApplicationSettings : IFileNameSettings
    {
        public ApplicationSettings(string inputFileName, string outputFileName)
        {
            InputFileName = inputFileName;
            OutputFileName = outputFileName ?? $"{AppDomain.CurrentDomain.BaseDirectory}/result.txt";
        }

        public string InputFileName { get; }
        public string OutputFileName { get; }
    }
}