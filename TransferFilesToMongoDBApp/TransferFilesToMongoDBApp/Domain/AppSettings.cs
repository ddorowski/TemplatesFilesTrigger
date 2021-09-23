namespace TransferFilesToMongoDBApp.Domain
{
    public class AppSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string SourcePathToFolder { get; set; }
    }
}
