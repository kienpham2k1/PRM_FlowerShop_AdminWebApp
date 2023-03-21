using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using BusinessObject.IService;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class LocalFileUploadService : IUploadFileService
    {
        private readonly IHostingEnvironment env;
        public LocalFileUploadService(IHostingEnvironment _env)
        {
            this.env = _env;
        }
        public async Task<string> UploadFile(IFormFile file)
        {
            var filePath = Path.Combine(env.ContentRootPath, @"wwwroot\images", file.FileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;
        }
        public void DeleteFolder(string path)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
