namespace My_steam_server.Interfaces
{
    public interface IResources
    {

        public Task<Stream?> GetMarkdownStreamAsync(string FileName);

        public Task<Stream?> GetLibsStaticFiles(long GameId);
    }
}
