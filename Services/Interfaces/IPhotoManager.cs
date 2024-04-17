using ContactHub.Model.DTOs;

namespace ContactHub.Services.Interfaces
{
    public interface IPhotoManager
    {
        Task<PhotoResult> UploadImage(IFormFile file);
        Task<bool> DeleteImage(string publicId);
    }
}
