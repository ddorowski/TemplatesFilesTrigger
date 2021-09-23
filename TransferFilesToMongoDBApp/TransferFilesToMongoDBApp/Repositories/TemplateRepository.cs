using System;
using System.IO;

namespace TransferFilesToMongoDBApp.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        public byte[] FileToByte(string fullPath)
        {
            byte[] bytes = File.ReadAllBytes(fullPath);
            return bytes;
        }

        public string GetFileName(string fullPath)
        {
            var fileName = Path.GetFileName(fullPath);
            return fileName;
        }
    }
}
