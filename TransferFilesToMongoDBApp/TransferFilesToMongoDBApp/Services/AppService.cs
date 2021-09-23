using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using TransferFilesToMongoDBApp.Domain;
using TransferFilesToMongoDBApp.Repositories;

namespace TransferFilesToMongoDBApp.Services
{
    public class AppService : BackgroundService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ITemplateService _templateService;
        private readonly ITemplateRepository _templateRepository;
        private readonly IApplicationLifetime _applicationLifetime;
        private readonly AppSettings _appSettings;

        public AppService(IDatabaseService databaseService, ITemplateService templateService,
                          ITemplateRepository templateRepository, IApplicationLifetime applicationLifetime, IOptions<AppSettings> appSettings)
        {
            _databaseService = databaseService;
            _templateRepository = templateRepository;
            _templateService = templateService;
            _applicationLifetime = applicationLifetime;
            _appSettings = appSettings.Value;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("\nSearchForFiles\n");
            var files = _templateService.SearchForFiles(_appSettings.SourcePathToFolder);

            foreach (var file in files)
            {
                Console.WriteLine("\nfull path: " + file);

                Console.WriteLine("\nFile to bytes");
                byte[] bytes = _templateRepository.FileToByte(file);

                Console.WriteLine("Insert or update template");
                var fileName = _templateRepository.GetFileName(file);
                _databaseService.InsertOrUpdateTemplate(fileName, bytes);
            }
            _applicationLifetime.StopApplication();
            return Task.CompletedTask;
        }
    }
}
