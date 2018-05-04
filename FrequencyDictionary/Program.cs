using System;
using System.Threading.Tasks;
using Autofac;
using FrequencyDictionary.Services;
using FrequencyDictionary.Services.Interfaces;
using FrequencyDictionary.Settings;
using FrequencyDictionary.Settings.Interfaces;
using FrequencyDictionary.Validators;
using FrequencyDictionary.Validators.Interfaces;
using NLog;

namespace FrequencyDictionary
{
    internal static class Program
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            try
            {
                CheckInputArguments(args);
                var mainService = Initialize(args);
                mainService.Start().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Log.Error(e, "Ошибка");
            }

            Console.ReadLine();
        }

        private static void CheckInputArguments(string[] args)
        {
            if (args.Length == 0)
                throw new ArgumentNullException(nameof(args), "Input arguments is null");
        }

        private static MainService Initialize(string[] args)
        {
            Log.Info("Инициализация началась");

            var builder = new ContainerBuilder();

            builder.RegisterType<FileExistsValidator>().As<IFileExistsValidator>();
            builder.RegisterType<MainService>();
            builder.RegisterType<FileProcessor>().As<IFileProcessor>();
            builder.RegisterType<FileWriterService>().As<IFileWriterService>();
            builder.RegisterType<LineProcessor>().As<ILineProcessor>();
            var settings = new ApplicationSettings(args[0], args.Length == 1 ? null : args[1]);
            builder.RegisterInstance(settings).As<IFileNameSettings>();

            var container = builder.Build();

            Log.Info("Инициализация завершена");

            return container.Resolve<MainService>();
        }
    }
}
