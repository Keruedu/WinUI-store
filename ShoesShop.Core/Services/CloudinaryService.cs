using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ShoesShop.Core.Contracts.Services;
using System;
using System.Threading.Tasks;

namespace ShoesShop.Core.Services;
public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService()
    {
        _cloudinary = new Cloudinary(new Account(
            "dyocg3k6j", //cloud name
            "195462783562453", //api key
            "KihyupE1BS3AHVPGYQVU1pxfM0Y" // api secret
        ));
    }

    public async Task<string> UploadImageAsync(string filePath, string uploadType)
    {
        // Validate the uploadType parameter
        if (string.IsNullOrEmpty(uploadType) || (uploadType != "user" && uploadType != "shoes"))
        {
            throw new ArgumentException("Invalid upload type. Must be either 'user' or 'shoes'.", nameof(uploadType));
        }

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(filePath),
            Folder = $"ShoesShop/{uploadType}" // Separate folders for user and shoes
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return uploadResult.SecureUrl.AbsoluteUri;
        }
        else
        {
            throw new Exception($"Upload failed: {uploadResult.Error?.Message}");
        }
    }
}
