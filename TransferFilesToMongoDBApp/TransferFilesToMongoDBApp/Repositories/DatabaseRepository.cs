using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Collections.Generic;
using System.Linq;
using TransferFilesToMongoDBApp.Domain;

namespace TransferFilesToMongoDBApp.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly GridFSBucket _bucket;
        private readonly AppSettings _databaseSettings;

        public DatabaseRepository(IOptions<AppSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var db = client.GetDatabase(_databaseSettings.DatabaseName);

            var BucketOptions = new GridFSBucketOptions()
            {
                BucketName = _databaseSettings.CollectionName,
                ChunkSizeBytes = 1048576,
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Secondary
            };
            var bucket = new GridFSBucket(db, BucketOptions);
            _bucket = bucket;
        }

        public List<GridFSFileInfo> GetCollection()
        {
            var collection = _bucket.Find(new BsonDocument()).ToList();

            return collection;
        }

        public void InsertTemplate(string fileName, byte[] bytes)
        {
            _bucket.UploadFromBytes(fileName, bytes);
        }

        public void DeleteTemplate(BsonValue id)
        {
            _bucket.Delete(id);         
        }
    }
}
