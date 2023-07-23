using DefectMapAPI.Models.File;
using System.Net;

namespace DefectMapAPI.Services.FileHostService
{
    public class FileHost : IFileHost
    {
        public const long MaxFileSize = 1024 * 1024 * 8;

        private readonly ILogger<FileHost> logger;
        private readonly IWebHostEnvironment env;

        public FileHost(ILogger<FileHost> logger,
            IWebHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
        }

        public async Task<UploadedFile> Upload(Stream fileStream, string fileName, string contentType)
        {
            var uploadResult = new UploadedFile();

            string trustedFileNameForFileStorage;
            var untrustedFileName = fileName;
            uploadResult.FileName = untrustedFileName;
            var trustedFileNameForDisplay =
                WebUtility.HtmlEncode(untrustedFileName);

            if (fileStream.Length == 0)
            {
                logger.LogInformation("{FileName} length is 0.",
                    trustedFileNameForDisplay);

                throw new IOException("File size is 0.");
            }
            else if (fileStream.Length > MaxFileSize)
            {
                logger.LogInformation("{FileName} of {Length} bytes is " +
                    "larger than the limit of {Limit} bytes.",
                    trustedFileNameForDisplay, fileStream.Length, MaxFileSize);

                throw new IOException("File size larger than the limit.");
            }

            try
            {
                trustedFileNameForFileStorage = Path.GetRandomFileName();
                var path = Path.Combine(env.ContentRootPath,
                    "Uploads",
                    trustedFileNameForFileStorage);

                await using FileStream fs = new(path, FileMode.Create);
                await fileStream.CopyToAsync(fs);

                logger.LogInformation("{FileName} saved at {Path}",
                    trustedFileNameForDisplay, path);
                uploadResult.StoredFileName = trustedFileNameForFileStorage;
                uploadResult.ContentType = contentType;
            }
            catch (IOException ex)
            {
                logger.LogError("{FileName} error on upload: {Message}",
                    trustedFileNameForDisplay, ex.Message);
                throw;
            }

            return uploadResult;
        }
    }
}
