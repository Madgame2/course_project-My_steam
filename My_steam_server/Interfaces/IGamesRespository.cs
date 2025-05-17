namespace My_steam_server.Interfaces
{
    public interface IGamesRespository
    {
        string getRepositoryPath();

        Task<string> GetGameFilePathAsync(int gameId);
        Task<bool> GameExistsAsync(int gameId);

        Task<long> GetUncompressedSize(long gameId);
    }
}
