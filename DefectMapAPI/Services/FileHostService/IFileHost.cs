using DefectMapAPI.Models.File;

namespace DefectMapAPI.Services.FileHostService
{
    public interface IFileHost
    {
        string UploadsDirectoryPath { get; init; }
        long MaxFileSize { get; init; }

        Task<UploadedFile> Upload(Stream fileStream, string fileName, string contentType);
        Task Delete(UploadedFile file);
    }
}
