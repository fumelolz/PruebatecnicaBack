using PruebatecnicaBack.Application.Common.Interfaces.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using Newtonsoft.Json;

namespace PruebatecnicaBack.Infrastructure.Storage
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalFileStorage(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFile(string route, string container)
        {
            if (string.IsNullOrEmpty(route)) return Task.CompletedTask;

            // Deserializar la cadena JSON en un objeto ImageUrls
            var imageUrls = JsonConvert.DeserializeObject<ImageUrls>(route);
            Delete(imageUrls.Webp, container);
            Delete(imageUrls.WebpThumbnail, container);
            Delete(imageUrls.Thumbnail, container);
            Delete(imageUrls.Original, container);

            return Task.CompletedTask;
        }

        private void Delete(string route, string container)
        {
            var fileName = Path.GetFileName(route);
            string directoryFile = Path.Combine(_env.WebRootPath, container, fileName);

            if (File.Exists(directoryFile))
            {
                File.Delete(directoryFile);
            }
        }

        public async Task<string> EditFile(byte[] content, string extension, string container, string route, string contentType)
        {
            await DeleteFile(route, container);
            return await SaveFile(content, extension, container, contentType);
        }

        public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
        {
            // Validar formatos permitidos
            var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowedExtensions.Contains(extension.ToLower()))
                throw new InvalidOperationException("Formato de archivo no permitido.");

            // Crear subcarpeta con fecha (ejemplo: /imagenes/2025/02/07/)
            string dateFolder = DateTime.UtcNow.ToString("yyyy/MM/dd");
            string folder = Path.Combine(_env.WebRootPath, container, dateFolder);
            Directory.CreateDirectory(folder);

            string fileName = $"{Guid.NewGuid()}{extension}";
            string filePath = Path.Combine(folder, fileName);
            string webpFileName = $"{Path.GetFileNameWithoutExtension(fileName)}.webp";
            string webpFilePath = Path.Combine(folder, webpFileName);

            string thumbFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_thumb{extension}";
            string thumbFilePath = Path.Combine(folder, thumbFileName);
            string webpThumbFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_thumb.webp";
            string webpThumbFilePath = Path.Combine(folder, webpThumbFileName);

            using (var image = Image.Load(content))
            {
                // 📌 Redimensionar imagen principal (máx. 800x800px)
                int maxWidth = 800, maxHeight = 800;
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(maxWidth, maxHeight)
                }));

                await SaveImage(image, filePath, extension);
                await SaveImage(image, webpFilePath, ".webp");

                // 📌 Crear thumbnail (150x150px)
                using (var thumbnail = image.Clone(x => x.Resize(150, 150)))
                {
                    await SaveImage(thumbnail, thumbFilePath, extension);
                    await SaveImage(thumbnail, webpThumbFilePath, ".webp");
                }
            }

            // 🌐 Generar URL absoluta
            var urlActual = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            string fileUrl = $"{urlActual}/{container}/{dateFolder}/{fileName}".Replace("\\", "/");
            string webpUrl = $"{urlActual}/{container}/{dateFolder}/{webpFileName}".Replace("\\", "/");
            string thumbUrl = $"{urlActual}/{container}/{dateFolder}/{thumbFileName}".Replace("\\", "/");
            string webpThumbUrl = $"{urlActual}/{container}/{dateFolder}/{webpThumbFileName}".Replace("\\", "/");

            return $"{{ \"original\": \"{fileUrl}\", \"webp\": \"{webpUrl}\", \"thumbnail\": \"{thumbUrl}\", \"webpThumbnail\": \"{webpThumbUrl}\" }}";
        }

        private async Task SaveImage(Image image, string filePath, string extension)
        {
            await using var outputStream = new FileStream(filePath, FileMode.Create);
            switch (extension.ToLower())
            {
                case ".png":
                    await image.SaveAsPngAsync(outputStream, new PngEncoder { CompressionLevel = PngCompressionLevel.DefaultCompression });
                    break;
                case ".webp":
                    await image.SaveAsWebpAsync(outputStream, new WebpEncoder { Quality = 80 });
                    break;
                default:
                    await image.SaveAsJpegAsync(outputStream, new JpegEncoder { Quality = 80 });
                    break;
            }
        }
    }
}