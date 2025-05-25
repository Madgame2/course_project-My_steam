using My_steam_server.Interfaces;
using My_steam_server.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace My_steam_server.Repositories.ADO
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = new List<User>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT u.Id, u.Email 
                FROM Users u";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var userId = reader.GetString(0);
                users.Add(new User
                {
                    Id = userId,
                    Email = reader.GetString(1),
                    CartItems = await GetCartAsync(userId)
                });
            }

            return users;
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT u.Id, u.Email 
                FROM Users u 
                WHERE u.Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetString(0),
                    Email = reader.GetString(1),
                    CartItems = await GetCartAsync(id)
                };
            }

            return null;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT u.Id, u.Email 
                FROM Users u 
                WHERE u.Email = @Email";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var id = reader.GetString(0);
                return new User
                {
                    Id = id,
                    Email = reader.GetString(1),
                    CartItems = await GetCartAsync(id)
                };
            }

            return null;
        }

        public async Task AddUserAsync(User user)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                INSERT INTO Users (Id, Email) 
                VALUES (@Id, @Email)";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@Email", user.Email);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task AddToCartAsync(string userId, PurchaseOption purchaseOption)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            try
            {
                // Проверка наличия
                string checkSql = @"
                    SELECT COUNT(*) 
                    FROM CartItems 
                    WHERE UserId = @UserId AND PurchaseOptionId = @OptionId";

                using (var checkCmd = new SqlCommand(checkSql, conn, transaction))
                {
                    checkCmd.Parameters.AddWithValue("@UserId", userId);
                    checkCmd.Parameters.AddWithValue("@OptionId", purchaseOption.OptionId);

                    var exists = (int)await checkCmd.ExecuteScalarAsync() > 0;
                    if (exists)
                        throw new ArgumentException("Товар уже в корзине.");
                }

                // Добавление
                string insertSql = @"
                    INSERT INTO CartItems (UserId, PurchaseOptionId) 
                    VALUES (@UserId, @OptionId)";

                using var cmd = new SqlCommand(insertSql, conn, transaction);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@OptionId", purchaseOption.OptionId);
                await cmd.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RemoveFromCartAsync(string userId, long purchaseOptionId)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            try
            {
                string sql = @"
                    DELETE FROM CartItems 
                    WHERE UserId = @UserId AND PurchaseOptionId = @OptionId";

                using var cmd = new SqlCommand(sql, conn, transaction);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@OptionId", purchaseOptionId);
                await cmd.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<CartItem>> GetCartAsync(string userId)
        {
            var cart = new List<CartItem>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                SELECT ci.PurchaseOptionId, po.OptionId, po.Price, po.PurchaseName, po.ImageLink,
                       gr.Id as GoodReceivedId, gr.GoodId
                FROM CartItems ci
                JOIN PurchaseOptions po ON ci.PurchaseOptionId = po.OptionId
                LEFT JOIN GoodsReceived gr ON gr.PurchaseOptionId = po.OptionId
                WHERE ci.UserId = @UserId";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var cartItem = new CartItem
                {
                    UserId = userId,
                    PurchaseOptionId = reader.GetInt64(0),
                    PurchaseOption = new PurchaseOption
                    {
                        OptionId = reader.GetInt64(1),
                        Price = reader.GetFloat(2),
                        PurchaseName = reader.GetString(3),
                        ImageLink = reader.GetString(4),
                        GoodsReceived = new List<GoodReceived>()
                    }
                };

                if (!reader.IsDBNull(5))
                {
                    cartItem.PurchaseOption.GoodsReceived.Add(new GoodReceived
                    {
                        Id = reader.GetInt64(5),
                        GoodId = reader.GetInt64(6)
                    });
                }

                cart.Add(cartItem);
            }

            return cart;
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }
    }
}
