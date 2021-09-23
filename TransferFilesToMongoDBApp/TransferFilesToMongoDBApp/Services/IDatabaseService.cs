using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;

namespace TransferFilesToMongoDBApp.Services
{
    public interface IDatabaseService
    {
        void ReadCollection(List<GridFSFileInfo> collection);
        void UpdateTemplate(string fileName, byte[] bytes);
        void InsertOrUpdateTemplate(string fileName, byte[] bytes);
    }
}
