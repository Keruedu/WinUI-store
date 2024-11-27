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

    public async Task<string> UploadImageAsync(string filePath)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(filePath),
            Folder = "ShoesShop"
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
