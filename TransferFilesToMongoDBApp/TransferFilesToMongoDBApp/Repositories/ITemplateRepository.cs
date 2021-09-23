using System;

namespace TransferFilesToMongoDBApp.Repositories
{
    public interface ITemplateRepository
    {
        byte[] FileToByte(string fullPath);
        string GetFileName(string fullPath);
    }
}
