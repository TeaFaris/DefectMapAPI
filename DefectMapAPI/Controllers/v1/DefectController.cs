using DefectMapAPI.Models.Defect;
using DefectMapAPI.Models.DefectModel;
using DefectMapAPI.Models.File;
using DefectMapAPI.Models.UserModel;
using DefectMapAPI.Services.FileHostService;
using DefectMapAPI.Services.Repositories.Defect;
using DefectMapAPI.Services.Repositories.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DefectMapAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DefectController : ControllerBase
    {
        readonly IDefectRepository defectRepository;
        readonly IFileRepository fileRepository;
        readonly IFileHost fileHost;
        public DefectController(
                IDefectRepository defectRepository,
                IFileHost fileHost,
                IFileRepository fileRepository
            )
        {
            this.fileHost = fileHost;
            this.fileRepository = fileRepository;
            this.defectRepository = defectRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allDefects = await defectRepository.GetAllAsync();

            var allDefectsDTO = new List<DefectDTO>(allDefects.Count());

            foreach (var defect in allDefects)
            {
                allDefectsDTO.Add(await defect.ToDTO(fileRepository));
            }

            return Ok(allDefectsDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var defect = await defectRepository.GetAsync(id);

            if(defect is null)
            {
                return NotFound($"No defects with id {id} was found.");
            }

            return Ok(await defect.ToDTO(fileRepository));
        }

        [HttpGet("find")]
        public async Task<IActionResult> Find([FromQuery] string name)
        {
            var foundDefects = await defectRepository.FindAsync(x => x.Name == name);

            return Ok(foundDefects);
        }

        [HttpPost("UploadPhotos")]
        [Authorize(Roles = "User,Administrator")]
        public async Task<IActionResult> UploadDefectPhoto([FromForm] IList<IFormFile> photos)
        {
            if (!photos.Any() || photos.Any(x => x.ContentType.ToLower() is not "image/png" and not "image/jpeg"))
            {
                return new UnsupportedMediaTypeResult();
            }

            var uploadedPhotos = new List<UploadedFile>(photos.Count);

            foreach (var photo in photos)
            {
                uploadedPhotos.Add(
                    await fileHost.Upload(
                        photo.OpenReadStream(),
                        photo.FileName,
                        photo.ContentType
                    ));
            }

            await fileRepository.AddRangeAsync(uploadedPhotos);
            await fileRepository.SaveAsync();

            return Ok(uploadedPhotos);
        }

        [HttpPost]
        [Authorize(Roles = "User,Administrator")]
        public async Task<IActionResult> Create(DefectDTO defect)
        {
            if(defect.Photos.Count == 0)
            {
                return BadRequest("Defect must include atleast one photo.");
            }

            await defectRepository.AddAsync(new Defect
            {
                Id = defect.Id,
                Name = defect.Name,
                Description = defect.Description,
                OwnerId = defect.Owner.Id,
                PhotosIds = defect.Photos.ConvertAll(x => x.Id),
                HorizontalAccuracy = defect.HorizontalAccuracy,
                Latitude = defect.Latitude,
                Longitude = defect.Longitude,
                VerticalAccuracy = defect.VerticalAccuracy
            });

            await defectRepository.SaveAsync();

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(DefectDTO defect)
        {
            if (defect.Photos.Count == 0)
            {
                return BadRequest("Defect must include atleast one photo.");
            }

            var defectToEdit = await defectRepository.GetAsync(defect.Id);
            
            if(defectToEdit is null)
            {
                return NotFound($"No defect with id {defect.Id} was found.");
            }

            defectToEdit.Name = defect.Name;
            defectToEdit.Description = defect.Description;
            defectToEdit.PhotosIds = defect.Photos.ConvertAll(x => x.Id);
            defect.Latitude = defect.Latitude;
            defect.Longitude = defect.Longitude;
            defect.HorizontalAccuracy = defect.HorizontalAccuracy;
            defect.VerticalAccuracy = defect.VerticalAccuracy;

            await defectRepository.UpdateAsync(defectToEdit);

            await defectRepository.SaveAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var defectToRemove = await defectRepository.GetAsync(id);

            if(defectToRemove is null)
            {
                return NotFound($"No defect with id {id} was found.");
            }

            await defectRepository.RemoveAsync(defectToRemove);
            await defectRepository.SaveAsync();

            return Ok();
        }
    }
}
