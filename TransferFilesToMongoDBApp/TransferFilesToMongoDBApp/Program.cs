using System;
using CommandLine;
using TransferFilesToMongoDBApp.Repositories;
using TransferFilesToMongoDBApp.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransferFilesToMongoDBApp.Domain;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace TransferFilesToMongoDBApp
{
    class Program
    {
        public class ProgramOptions
        {
            [Option('c', "connectionString", Required = true, HelpText = "Connection string to connect to database")]
            public string ConnectionString { get; set; }

            [Option('p', "PathToFolder", Required = false, HelpText = "Path to folder with templates")]
            public string SourcePathToFolder { get; set; }
        }

        public static IConfiguration Configuration = GetConfiguration();
        public static AppSettings appSettings = Configuration.GetSection("App:AppSettings")
               .Get<AppSettings>();

        private static void CheckProgramArguments(string[] args)
            {
                var resultWithParsed = Parser.Default.ParseArguments<ProgramOptions>(args).WithParsed<ProgramOptions>(o =>
                {
                    if (o.ConnectionString != appSettings.ConnectionString)
                    {
                        throw new Exception("Connection String is invalid");
                    }
                    else
                    {
                        Console.WriteLine("Connection String is correct");
                    }
                    if(o.SourcePathToFolder != null)
                    {
                        Configuration["App:AppSettings:SourcePathToFolder"] = o.SourcePathToFolder;
                    }          
                });
                var resultWithNotParsed = Parser.Default.ParseArguments<ProgramOptions>(args).WithNotParsed<ProgramOptions>(o =>
                {
                    throw new Exception("Connection String is required!");
                });
            }

        static async Task<int> Main(string[] args)
        {
            Console.WriteLine("\nCheck program arguments\n");

            CheckProgramArguments(args);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await CreateHostBuilder(args).Build().RunAsync(cancellationTokenSource.Token);

            return 0;
        }
        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(Configuration);
                    services.AddOptions().Configure<AppSettings>(Configuration.GetSection("App:AppSettings"));
                    services.AddScoped<IDatabaseRepository, DatabaseRepository>();
                    services.AddScoped<IDatabaseService, DatabaseService>();
                    services.AddScoped<ITemplateRepository, TemplateRepository>();
                    services.AddScoped<ITemplateService, TemplateService>();
                    services.AddHostedService<AppService>();
                });
    }
}
