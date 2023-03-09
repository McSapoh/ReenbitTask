using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReenbitTask.Services
{
    public class FileService : IFileService
    {
        public async Task<bool> SaveFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    #region Getting random string
                    var random = new Random();
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    var randomSting = new string(Enumerable.Repeat(chars, 12)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
                    #endregion

                    var fileName = randomSting + Path.GetExtension(file.FileName);
                    var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                    string filePath = Path.Combine(path + "\\Files\\", fileName);
                    using Stream fileStream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(fileStream);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
