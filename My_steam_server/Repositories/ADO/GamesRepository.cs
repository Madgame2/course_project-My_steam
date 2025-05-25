using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace My_steam_server.Repositories.ADO
{
    public class GamesRepository : IGamesRepository
    {
        private readonly string _connectionString;

        public GamesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            var games = new List<Game>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT g.Id, g.Name, g.UserId, g.Price, g.Description, 
                       g.HeaderImageSource, g.DownloadSource, g.ReleaseDate, g.Rating
                FROM Games g";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var game = new Game
                {
                    Id = reader.GetInt64(0),
                    Name = reader.GetString(1),
                    UserId = reader.GetString(2),
                    Price = reader.GetFloat(3),
                    Description = reader.GetString(4),
                    HeaderImageSource = reader.GetString(5),
                    DownloadSource = reader.GetString(6),
                    ReleaseDate = reader.GetDateTime(7),
                    ratinng = (Ratinng)reader.GetInt32(8),
                    imageSource = new List<Screenshot>(),
                    PurchaseOptions = new List<PurchaseOption>()
                };

                games.Add(game);
            }

            // Загрузка скриншотов и опций покупки для каждой игры
            foreach (var game in games)
            {
                game.imageSource = await GetScreenshotsAsync(game.Id);
                game.PurchaseOptions = await GetPurchaseOptionsAsync(game.Id);
            }

            return games;
        }

        public async Task<Game?> GetByIdAsync(long id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT g.Id, g.Name, g.UserId, g.Price, g.Description, 
                       g.HeaderImageSource, g.DownloadSource, g.ReleaseDate, g.Rating
                FROM Games g
                WHERE g.Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var game = new Game
                {
                    Id = reader.GetInt64(0),
                    Name = reader.GetString(1),
                    UserId = reader.GetString(2),
                    Price = reader.GetFloat(3),
                    Description = reader.GetString(4),
                    HeaderImageSource = reader.GetString(5),
                    DownloadSource = reader.GetString(6),
                    ReleaseDate = reader.GetDateTime(7),
                    ratinng = (Ratinng)reader.GetInt32(8),
                    imageSource = await GetScreenshotsAsync(id),
                    PurchaseOptions = await GetPurchaseOptionsAsync(id)
                };

                return game;
            }

            return null;
        }

        public async Task AddAsync(Game game)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            try
            {
                // Добавление игры
                string sql = @"
                    INSERT INTO Games (Name, UserId, Price, Description, HeaderImageSource, 
                                     DownloadSource, ReleaseDate, Rating)
                    VALUES (@Name, @UserId, @Price, @Description, @HeaderImageSource, 
                            @DownloadSource, @ReleaseDate, @Rating);
                    SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(sql, conn, transaction);
                cmd.Parameters.AddWithValue("@Name", game.Name);
                cmd.Parameters.AddWithValue("@UserId", game.UserId);
                cmd.Parameters.AddWithValue("@Price", game.Price);
                cmd.Parameters.AddWithValue("@Description", game.Description);
                cmd.Parameters.AddWithValue("@HeaderImageSource", game.HeaderImageSource);
                cmd.Parameters.AddWithValue("@DownloadSource", game.DownloadSource);
                cmd.Parameters.AddWithValue("@ReleaseDate", game.ReleaseDate);
                cmd.Parameters.AddWithValue("@Rating", (int)game.ratinng);

                var gameId = Convert.ToInt64(await cmd.ExecuteScalarAsync());

                // Добавление скриншотов
                foreach (var screenshot in game.imageSource)
                {
                    sql = "INSERT INTO Screenshots (GameId, ImageSource) VALUES (@GameId, @ImageSource)";
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@GameId", gameId);
                    cmd.Parameters.AddWithValue("@ImageSource", screenshot.ImageSource);
                    await cmd.ExecuteNonQueryAsync();
                }

                // Добавление опций покупки
                foreach (var option in game.PurchaseOptions)
                {
                    sql = @"
                        INSERT INTO PurchaseOptions (Price, PurchaseName, ImageLink, GameId)
                        VALUES (@Price, @PurchaseName, @ImageLink, @GameId);
                        SELECT SCOPE_IDENTITY();";

                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Price", option.Price);
                    cmd.Parameters.AddWithValue("@PurchaseName", option.PurchaseName);
                    cmd.Parameters.AddWithValue("@ImageLink", option.ImageLink);
                    cmd.Parameters.AddWithValue("@GameId", gameId);

                    var optionId = Convert.ToInt64(await cmd.ExecuteScalarAsync());

                    // Добавление полученных товаров
                    foreach (var good in option.GoodsReceived)
                    {
                        sql = "INSERT INTO GoodsReceived (GoodId, PurchaseOptionId) VALUES (@GoodId, @PurchaseOptionId)";
                        cmd.CommandText = sql;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@GoodId", good.GoodId);
                        cmd.Parameters.AddWithValue("@PurchaseOptionId", optionId);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(Game game)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            try
            {
                // Обновление игры
                string sql = @"
                    UPDATE Games 
                    SET Name = @Name, UserId = @UserId, Price = @Price, 
                        Description = @Description, HeaderImageSource = @HeaderImageSource,
                        DownloadSource = @DownloadSource, ReleaseDate = @ReleaseDate, 
                        Rating = @Rating
                    WHERE Id = @Id";

                using var cmd = new SqlCommand(sql, conn, transaction);
                cmd.Parameters.AddWithValue("@Id", game.Id);
                cmd.Parameters.AddWithValue("@Name", game.Name);
                cmd.Parameters.AddWithValue("@UserId", game.UserId);
                cmd.Parameters.AddWithValue("@Price", game.Price);
                cmd.Parameters.AddWithValue("@Description", game.Description);
                cmd.Parameters.AddWithValue("@HeaderImageSource", game.HeaderImageSource);
                cmd.Parameters.AddWithValue("@DownloadSource", game.DownloadSource);
                cmd.Parameters.AddWithValue("@ReleaseDate", game.ReleaseDate);
                cmd.Parameters.AddWithValue("@Rating", (int)game.ratinng);

                await cmd.ExecuteNonQueryAsync();

                // Удаление старых скриншотов
                sql = "DELETE FROM Screenshots WHERE GameId = @GameId";
                cmd.CommandText = sql;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@GameId", game.Id);
                await cmd.ExecuteNonQueryAsync();

                // Добавление новых скриншотов
                foreach (var screenshot in game.imageSource)
                {
                    sql = "INSERT INTO Screenshots (GameId, ImageSource) VALUES (@GameId, @ImageSource)";
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@GameId", game.Id);
                    cmd.Parameters.AddWithValue("@ImageSource", screenshot.ImageSource);
                    await cmd.ExecuteNonQueryAsync();
                }

                // Удаление старых опций покупки
                sql = "DELETE FROM PurchaseOptions WHERE GameId = @GameId";
                cmd.CommandText = sql;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@GameId", game.Id);
                await cmd.ExecuteNonQueryAsync();

                // Добавление новых опций покупки
                foreach (var option in game.PurchaseOptions)
                {
                    sql = @"
                        INSERT INTO PurchaseOptions (Price, PurchaseName, ImageLink, GameId)
                        VALUES (@Price, @PurchaseName, @ImageLink, @GameId);
                        SELECT SCOPE_IDENTITY();";

                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Price", option.Price);
                    cmd.Parameters.AddWithValue("@PurchaseName", option.PurchaseName);
                    cmd.Parameters.AddWithValue("@ImageLink", option.ImageLink);
                    cmd.Parameters.AddWithValue("@GameId", game.Id);

                    var optionId = Convert.ToInt64(await cmd.ExecuteScalarAsync());

                    // Добавление полученных товаров
                    foreach (var good in option.GoodsReceived)
                    {
                        sql = "INSERT INTO GoodsReceived (GoodId, PurchaseOptionId) VALUES (@GoodId, @PurchaseOptionId)";
                        cmd.CommandText = sql;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@GoodId", good.GoodId);
                        cmd.Parameters.AddWithValue("@PurchaseOptionId", optionId);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(long id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            try
            {
                // Удаление связанных записей
                string sql = "DELETE FROM GoodsReceived WHERE PurchaseOptionId IN (SELECT OptionId FROM PurchaseOptions WHERE GameId = @GameId)";
                using var cmd = new SqlCommand(sql, conn, transaction);
                cmd.Parameters.AddWithValue("@GameId", id);
                await cmd.ExecuteNonQueryAsync();

                sql = "DELETE FROM PurchaseOptions WHERE GameId = @GameId";
                cmd.CommandText = sql;
                await cmd.ExecuteNonQueryAsync();

                sql = "DELETE FROM Screenshots WHERE GameId = @GameId";
                cmd.CommandText = sql;
                await cmd.ExecuteNonQueryAsync();

                sql = "DELETE FROM Games WHERE Id = @GameId";
                cmd.CommandText = sql;
                await cmd.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<List<Screenshot>> GetScreenshotsAsync(long gameId)
        {
            var screenshots = new List<Screenshot>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = "SELECT Id, ImageSource FROM Screenshots WHERE GameId = @GameId";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@GameId", gameId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                screenshots.Add(new Screenshot
                {
                    Id = reader.GetInt64(0),
                    ImageSource = reader.GetString(1)
                });
            }

            return screenshots;
        }

        private async Task<List<PurchaseOption>> GetPurchaseOptionsAsync(long gameId)
        {
            var options = new List<PurchaseOption>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT po.OptionId, po.Price, po.PurchaseName, po.ImageLink
                FROM PurchaseOptions po
                WHERE po.GameId = @GameId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@GameId", gameId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var option = new PurchaseOption
                {
                    OptionId = reader.GetInt64(0),
                    Price = reader.GetFloat(1),
                    PurchaseName = reader.GetString(2),
                    ImageLink = reader.GetString(3),
                    GoodsReceived = new List<GoodReceived>()
                };

                options.Add(option);
            }

            // Загрузка полученных товаров для каждой опции
            foreach (var option in options)
            {
                sql = @"
                    SELECT gr.Id, gr.GoodId
                    FROM GoodsReceived gr
                    WHERE gr.PurchaseOptionId = @OptionId";

                cmd.CommandText = sql;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@OptionId", option.OptionId);

                using var goodsReader = await cmd.ExecuteReaderAsync();
                while (await goodsReader.ReadAsync())
                {
                    option.GoodsReceived.Add(new GoodReceived
                    {
                        Id = goodsReader.GetInt64(0),
                        GoodId = goodsReader.GetInt64(1)
                    });
                }
            }

            return options;
        }
    }
} 