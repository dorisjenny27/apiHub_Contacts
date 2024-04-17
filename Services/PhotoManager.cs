using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactHub.Model.DTOs;
using ContactHub.Services.Interfaces;

namespace ContactHub.Services
{
    public class PhotoManager : IPhotoManager
    {
        private readonly Cloudinary _cloudinary;

        public PhotoManager(IConfiguration config)
        {
          var cloudName = config.GetSection("CloudinarySettings:CloudName").Value;
            var apiKey = config.GetSection("CloudinarySettings:ApiKey").Value;
            var apiSecret = config.GetSection("CloudinarySettings:ApiSecret").Value;

            Account account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }
        public async Task<bool> DeleteImage(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            if (result.Result.ToLower().Equals("ok"))
                return true;
            return false;
        }

        public async Task<PhotoResult> UploadImage(IFormFile file)
        {
            var allowedTypes = new List<string> { "image/png", "image/jpg", "image/jpeg" };
            //Validate the image size
            if(file.Length > 0 && file.Length <= (2 * 1024 * 1024))
            {
                //Validate the type
                if(allowedTypes.Any(x => x.ToLower().Equals(file.ContentType.ToLower())))
                {
                    var uploadResult = new ImageUploadResult();
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, stream),
                            Transformation = new Transformation().Width("200").Height("200").Crop("fill").Gravity("face")
                        };
                        uploadResult = _cloudinary.Upload(uploadParams);
                    }
                    if (uploadResult.Url != null)
                        return new PhotoResult
                        {
                            IsSuccess = true,
                            Message = "Uploaded successfully",
                            PublicId = uploadResult.PublicId,
                            PhotoUrl = uploadResult.Url.ToString()
                        };
                    return new PhotoResult
                    {
                        IsSuccess = false,
                        Message = "Upload failed"
                    };
                }
                else
                {
                    return new PhotoResult
                    {
                        IsSuccess = false,
                        Message = "Invalid file cannot type",
                    };
                }
            }
            else
            {
                return new PhotoResult
                {
                    IsSuccess = false,
                    Message = "Invalid size",
                };
            }
        }
    }
}
