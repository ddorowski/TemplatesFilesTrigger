using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System.Collections.Generic;

namespace TransferFilesToMongoDBApp.Repositories
{
    public interface IDatabaseRepository
    {
        List<GridFSFileInfo> GetCollection();
        void InsertTemplate(string fileName, byte[] bytes);
        void DeleteTemplate(BsonValue id);
    }
}
