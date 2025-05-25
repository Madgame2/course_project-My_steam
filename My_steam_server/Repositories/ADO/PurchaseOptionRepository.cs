using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace My_steam_server.Repositories.ADO
{
    public class PurchaseOptionRepository : IPurchaseOptionRepository
    {
        private readonly string _connectionString;

        public PurchaseOptionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<PurchaseOption>> GetAllAsync()
        {
            var options = new List<PurchaseOption>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT po.OptionId, po.Price, po.PurchaseName, po.ImageLink, po.GameId
                FROM PurchaseOptions po";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var option = new PurchaseOption
                {
                    OptionId = reader.GetInt64(0),
                    Price = reader.GetFloat(1),
                    PurchaseName = reader.GetString(2),
                    ImageLink = reader.GetString(3),
                    GameId = reader.GetInt64(4),
                    GoodsReceived = new List<GoodReceived>()
                };

                options.Add(option);
            }

            // Загрузка полученных товаров для каждой опции
            foreach (var option in options)
            {
                option.GoodsReceived = await GetGoodsReceivedAsync(option.OptionId);
            }

            return options;
        }

        public async Task<PurchaseOption?> GetByIdAsync(long id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT po.OptionId, po.Price, po.PurchaseName, po.ImageLink, po.GameId
                FROM PurchaseOptions po
                WHERE po.OptionId = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var option = new PurchaseOption
                {
                    OptionId = reader.GetInt64(0),
                    Price = reader.GetFloat(1),
                    PurchaseName = reader.GetString(2),
                    ImageLink = reader.GetString(3),
                    GameId = reader.GetInt64(4),
                    GoodsReceived = await GetGoodsReceivedAsync(id)
                };

                return option;
            }

            return null;
        }

        public async Task<IEnumerable<PurchaseOption>> GetByGameIdAsync(long gameId)
        {
            var options = new List<PurchaseOption>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT po.OptionId, po.Price, po.PurchaseName, po.ImageLink, po.GameId
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
                    GameId = reader.GetInt64(4),
                    GoodsReceived = new List<GoodReceived>()
                };

                options.Add(option);
            }

            // Загрузка полученных товаров для каждой опции
            foreach (var option in options)
            {
                option.GoodsReceived = await GetGoodsReceivedAsync(option.OptionId);
            }

            return options;
        }

        public async Task AddAsync(PurchaseOption option)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            try
            {
                string sql = @"
                    INSERT INTO PurchaseOptions (Price, PurchaseName, ImageLink, GameId)
                    VALUES (@Price, @PurchaseName, @ImageLink, @GameId);
                    SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(sql, conn, transaction);
                cmd.Parameters.AddWithValue("@Price", option.Price);
                cmd.Parameters.AddWithValue("@PurchaseName", option.PurchaseName);
                cmd.Parameters.AddWithValue("@ImageLink", option.ImageLink);
                cmd.Parameters.AddWithValue("@GameId", option.GameId);

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

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(PurchaseOption option)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            try
            {
                string sql = @"
                    UPDATE PurchaseOptions 
                    SET Price = @Price, PurchaseName = @PurchaseName, 
                        ImageLink = @ImageLink, GameId = @GameId
                    WHERE OptionId = @OptionId";

                using var cmd = new SqlCommand(sql, conn, transaction);
                cmd.Parameters.AddWithValue("@OptionId", option.OptionId);
                cmd.Parameters.AddWithValue("@Price", option.Price);
                cmd.Parameters.AddWithValue("@PurchaseName", option.PurchaseName);
                cmd.Parameters.AddWithValue("@ImageLink", option.ImageLink);
                cmd.Parameters.AddWithValue("@GameId", option.GameId);

                await cmd.ExecuteNonQueryAsync();

                // Удаление старых полученных товаров
                sql = "DELETE FROM GoodsReceived WHERE PurchaseOptionId = @OptionId";
                cmd.CommandText = sql;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@OptionId", option.OptionId);
                await cmd.ExecuteNonQueryAsync();

                // Добавление новых полученных товаров
                foreach (var good in option.GoodsReceived)
                {
                    sql = "INSERT INTO GoodsReceived (GoodId, PurchaseOptionId) VALUES (@GoodId, @PurchaseOptionId)";
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@GoodId", good.GoodId);
                    cmd.Parameters.AddWithValue("@PurchaseOptionId", option.OptionId);
                    await cmd.ExecuteNonQueryAsync();
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
                string sql = "DELETE FROM GoodsReceived WHERE PurchaseOptionId = @OptionId";
                using var cmd = new SqlCommand(sql, conn, transaction);
                cmd.Parameters.AddWithValue("@OptionId", id);
                await cmd.ExecuteNonQueryAsync();

                sql = "DELETE FROM PurchaseOptions WHERE OptionId = @OptionId";
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

        private async Task<List<GoodReceived>> GetGoodsReceivedAsync(long optionId)
        {
            var goods = new List<GoodReceived>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT gr.Id, gr.GoodId
                FROM GoodsReceived gr
                WHERE gr.PurchaseOptionId = @OptionId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@OptionId", optionId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                goods.Add(new GoodReceived
                {
                    Id = reader.GetInt64(0),
                    GoodId = reader.GetInt64(1)
                });
            }

            return goods;
        }
    }
} 