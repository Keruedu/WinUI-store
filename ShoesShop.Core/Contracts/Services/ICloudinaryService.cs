using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
namespace ShoesShop.Core.Contracts.Services;
public interface ICloudinaryService
{
    Task<string> UploadImageAsync(string filePath);
}
