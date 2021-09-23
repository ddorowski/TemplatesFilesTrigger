using System;
using System.IO;
using System.Linq;
using MongoDB.Driver;

namespace TransferFilesToMongoDBApp.Services
{
    class TemplateService : ITemplateService
    {
        public string[] SearchForFiles(string sourcePathToFolder)
        {
            if (!Directory.Exists(sourcePathToFolder))
            {
                throw new DirectoryNotFoundException($"Invalid path: {sourcePathToFolder}");
            }
            else
            {
                var files = Directory.GetFiles(sourcePathToFolder);
                var filesCount = files.Count();
                if (filesCount == 0)
                {
                    Console.WriteLine("There are no files in this directory");
                    return new string[] { };
                }
                else
                {                                      
                        return files;                  
                }
            }
        }
    }
}
