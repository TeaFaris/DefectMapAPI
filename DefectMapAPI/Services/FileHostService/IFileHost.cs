using DefectMapAPI.Models.File;

namespace DefectMapAPI.Services.FileHostService
{
    public interface IFileHost
    {
        Task<UploadedFile> Upload(Stream fileStream, string fileName, string contentType);
        Task Delete(UploadedFile file);
    }
}
