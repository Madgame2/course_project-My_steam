using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace My_steam_client.Scripts
{
    public class Tokens
    {
        public string JWT { get; set; }
        public string Refresh { get; set; }


        public static bool TryParse(string source, out Tokens tokens)
        {
            tokens = new Tokens();

            // Убираем фигурные скобки и лишние пробелы
            source = source.Trim().Trim('{', '}');

            // Теперь используем более точный regex
            var match = Regex.Match(source, @"AccessToken\s*=\s*(.*?),\s*RefreshToken\s*=\s*(.*)");

            if (!match.Success)
                return false;

            tokens.JWT = match.Groups[1].Value.Trim();
            tokens.Refresh = match.Groups[2].Value.Trim();

            return true;
        }
    }

    public static class TokenStorage
    {
        private static readonly string FolderPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "My_steam");

        private static readonly string FilePath = Path.Combine(FolderPath, "tokens.dat");
        public static void SaveTokens(Tokens tokens)
        {
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            var jwtData = Encoding.UTF8.GetBytes(tokens.JWT);
            var refreshData = Encoding.UTF8.GetBytes(tokens.Refresh);

            var Jwt_encripted = ProtectedData.Protect(jwtData, null, DataProtectionScope.CurrentUser);
            var refresh_encripted = ProtectedData.Protect(refreshData, null, DataProtectionScope.CurrentUser);

            using var fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            using var bw = new BinaryWriter(fs);

            bw.Write(Jwt_encripted.Length);
            bw.Write(Jwt_encripted);

            bw.Write(refresh_encripted.Length);
            bw.Write(refresh_encripted);
        }
        public static Tokens? LoadTokens()
        {
            if (!File.Exists(FilePath))
                return null;

            try
            {
                using var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                using var br = new BinaryReader(fs);

                int jwtLength = br.ReadInt32();
                byte[] jwtEncrypted = br.ReadBytes(jwtLength);
                byte[] jwtDecrypted = ProtectedData.Unprotect(jwtEncrypted, null, DataProtectionScope.CurrentUser);
                string jwt = Encoding.UTF8.GetString(jwtDecrypted);

                int refreshLength = br.ReadInt32();
                byte[] refreshEncrypted = br.ReadBytes(refreshLength);
                byte[] refreshDecrypted = ProtectedData.Unprotect(refreshEncrypted, null, DataProtectionScope.CurrentUser);
                string refresh = Encoding.UTF8.GetString(refreshDecrypted);

                return new Tokens { JWT = jwt, Refresh = refresh };
            }
            catch
            {
                // можно залогировать или удалить повреждённый файл
                return null;
            }
        }

        public static void DeleteTokens()
        {
            if(!File.Exists(FilePath)) return;
            File.Delete(FilePath);
        }
    }
}
