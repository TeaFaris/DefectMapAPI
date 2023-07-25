using DefectMapAPI.Models.File;
using DefectMapAPI.Models.UserModel;
using DefectMapAPI.Services.Repositories.File;

namespace DefectMapAPI.Models.DefectModel
{
    public static class DefectMapper
    {
        public static async Task<DefectDTO> ToDTO(this Defect.Defect defect, IFileRepository fileRepository)
        {
            var photos = new List<UploadedFile>(defect.PhotosIds.Count);

            foreach(var photoId in defect.PhotosIds)
            {
                photos.Add((await fileRepository.GetAsync(photoId))!);
            }

            return new DefectDTO
            {
                Id = defect.Id,
                Description = defect.Description,
                HorizontalAccuracy = defect.HorizontalAccuracy,
                Latitude = defect.Latitude,
                Longitude = defect.Longitude,
                Name = defect.Name,
                Owner = defect.Owner.ToDTO(),
                VerticalAccuracy = defect.VerticalAccuracy,
                Photos = photos
            };
        }
    }
}
