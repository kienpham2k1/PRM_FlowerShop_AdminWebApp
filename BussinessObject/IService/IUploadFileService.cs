using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface IUploadFileService
    {
        Task<string> UploadFile(IFormFile file);
        public void DeleteFolder(string path);
    }
}
