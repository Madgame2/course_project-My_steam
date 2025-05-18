using Game_Net_DTOLib;
using My_steam_server.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net
{
    public class UploadData
    {
        private readonly Game_Net.ComunitationMannageer comunitation;

        public UploadData(ComunitationMannageer comunitation)
        {
            this.comunitation = comunitation;
        }

        public async Task<Guid> UploadMetadataAsync(ProjectUploadDto dto)
        {
            var uploadId = Guid.NewGuid();

            using var content = new MultipartFormDataContent();



            content.Add(new StringContent(dto.UserId), "UserId"); 
            content.Add(new StringContent(uploadId.ToString()), "uploadId");
            content.Add(new StringContent(dto.ProjectName), "projectName");
            content.Add(new StringContent(dto.Description), "description");
            content.Add(new StringContent(dto.Price.ToString(CultureInfo.InvariantCulture)), "price");

            content.Add(new StreamContent(dto.HeaderImage), "headerImage", "header.jpg");
            content.Add(new StreamContent(dto.LibHeader), "libHeader", "libHeader.jpg");
            content.Add(new StreamContent(dto.LibIcon), "libIcon", "libIcon.jpg");

            for (int i = 0; i < dto.Screenshots.Count; i++)
            {
                var screenshotStream = dto.Screenshots[i];
                var fileName = $"screenshot_{i + 1}.jpg";
                content.Add(new StreamContent(screenshotStream), "screenshots", fileName);
            }

            await comunitation.SendMultipartAsync<bool>("api/Projects/upload/metadata", Protocol.Http, content);

            return uploadId;
        }
        public async Task UploadZipInChunksAsync(Stream zipStream, Guid uploadId)
        {
            const int ChunkSize = 5 * 1024 * 1024;

            long totalSize = zipStream.Length;
            int chunkNumber = 0;
            byte[] buffer = new byte[ChunkSize];

            if (zipStream.CanSeek)
                zipStream.Position = 0;

            while(zipStream.Position < totalSize)
            {
                int bytesRead = await zipStream.ReadAsync(buffer, 0, ChunkSize);
                using var chunkContent = new ByteArrayContent(buffer, 0, bytesRead);
                using var multipart = new MultipartFormDataContent();

                multipart.Add(new StringContent(uploadId.ToString()), "uploadId");
                multipart.Add(new StringContent(chunkNumber.ToString()), "chunkNumber");
                multipart.Add(new StringContent(totalSize.ToString()), "totalSize");
                multipart.Add(chunkContent, "chunk", $"chunk_{chunkNumber}");

                await comunitation.SendMultipartAsync<bool>("api/Projects/upload/chunk", Protocol.Http, multipart);

                chunkNumber++;
            }

            var finish = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["uploadId"] = uploadId.ToString()
            });

            await comunitation.SendUrlEncodedContent("api/Projects/upload/finish", Protocol.Http, finish);
        }

        public async Task<Guid> UpdateMettadataAsync(ProjectUploadDto dto,long gameId)
        {
            var uploadId = Guid.NewGuid();

            using var content = new MultipartFormDataContent();



            content.Add(new StringContent(dto.UserId), "UserId");
            content.Add(new StringContent(gameId.ToString()), "GameId");
            content.Add(new StringContent(uploadId.ToString()), "uploadId");
            content.Add(new StringContent(dto.ProjectName), "projectName");
            content.Add(new StringContent(dto.Description), "description");
            content.Add(new StringContent(dto.Price.ToString(CultureInfo.InvariantCulture)), "price");

            content.Add(new StreamContent(dto.HeaderImage), "headerImage", "header.jpg");
            content.Add(new StreamContent(dto.LibHeader), "libHeader", "libHeader.jpg");
            content.Add(new StreamContent(dto.LibIcon), "libIcon", "libIcon.jpg");

            for (int i = 0; i < dto.Screenshots.Count; i++)
            {
                var screenshotStream = dto.Screenshots[i];
                var fileName = $"screenshot_{i + 1}.jpg";
                content.Add(new StreamContent(screenshotStream), "screenshots", fileName);
            }

            await comunitation.SendMultipartAsync<bool>("api/Projects/Update/metadata", Protocol.Http, content);

            return uploadId;
        }
        public async Task UpdateZipInChunksAsync(Stream zipStream,long gameId, Guid uploadId)
        {
            const int ChunkSize = 5 * 1024 * 1024;

            long totalSize = zipStream.Length;
            int chunkNumber = 0;
            byte[] buffer = new byte[ChunkSize];

            if (zipStream.CanSeek)
                zipStream.Position = 0;

            while (zipStream.Position < totalSize)
            {
                int bytesRead = await zipStream.ReadAsync(buffer, 0, ChunkSize);
                using var chunkContent = new ByteArrayContent(buffer, 0, bytesRead);
                using var multipart = new MultipartFormDataContent();

                multipart.Add(new StringContent(uploadId.ToString()), "uploadId");
                multipart.Add(new StringContent(chunkNumber.ToString()), "chunkNumber");
                multipart.Add(new StringContent(totalSize.ToString()), "totalSize");
                multipart.Add(chunkContent, "chunk", $"chunk_{chunkNumber}");

                await comunitation.SendMultipartAsync<bool>("api/Projects/upload/chunk", Protocol.Http, multipart);

                chunkNumber++;
            }

            var finish = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["GameID"]=gameId.ToString(),
                ["uploadId"] = uploadId.ToString()
            });

            await comunitation.SendUrlEncodedContent("api/Projects/Update/finish", Protocol.Http, finish);
        }
    }
}
