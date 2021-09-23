using TransferFilesToMongoDBApp.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TransferFilesToMongoDBApp.Services
{
    class DatabaseService : IDatabaseService
    {
        private readonly IDatabaseRepository _databaseRepository;
        public DatabaseService(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        public void ReadCollection(List<GridFSFileInfo> collection)
        {
            var itemsCount = collection.Count();
            if (itemsCount == 0)
            {
                Console.WriteLine("Collection is empty.");
            }
            else
            {
                foreach (GridFSFileInfo doc in collection)
                {
                    Console.WriteLine(doc.Filename + "\n");
                }
            }
        }

        public void UpdateTemplate(string fileName, byte[] bytes)
        {
            var files = _databaseRepository.GetCollection();
            var filesCount = files.Count();

            if(filesCount == 0)
            {
                throw new Exception("Collection is empty. Can't update.");
            }
            else
            {
                var file = files.Find(x => x.Filename.ToString() == fileName);
                var id = file.Id;
                _databaseRepository.DeleteTemplate(id);
                _databaseRepository.InsertTemplate(fileName, bytes);             
            }
        }

        public void InsertOrUpdateTemplate(string fileName, byte[] bytes)
        {
            var collection = _databaseRepository.GetCollection();
            var files = collection.FindAll(x => x.Filename == fileName);
            var filesCount = files.Count();
            if(filesCount == 0)
            {
                _databaseRepository.InsertTemplate(fileName, bytes);
            }
            else if(filesCount == 1)
            {
                UpdateTemplate(fileName, bytes);
            }
            else
            {
                throw new Exception("There are duplicates in the database.");
            }
        }
    }
}
