using DefectMapAPI.Services.FileHostService;
using DefectMapAPI.Services.Repositories.File;
using Microsoft.AspNetCore.Mvc;

namespace DefectMapAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class FileController : ControllerBase
    {
        readonly IWebHostEnvironment env;
        readonly IFileHost fileHost;
        readonly IFileRepository fileRepository;

        public FileController(
                IWebHostEnvironment env,
                IFileHost fileHost,
                IFileRepository fileRepository
            )
        {
            this.fileRepository = fileRepository;
            this.fileHost = fileHost;
            this.env = env;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var file = await fileRepository.GetAsync(id);

            if (file is null)
            {
                return NotFound("File not found.");
            }

            var path = Path.Combine(env.ContentRootPath,
                fileHost.UploadsDirectoryPath,
                file.StoredFileName);

            var ms = new MemoryStream();

            using var fileStream = new FileStream(path, FileMode.Open);
            await fileStream.CopyToAsync(ms);

            ms.Position = 0;

            return File(ms, file.ContentType, file.FileName);
        }
    }
}
