namespace TransferFilesToMongoDBApp.Services
{
    public interface ITemplateService
    {
        string[] SearchForFiles(string sourcePath);
    }
}
