using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ReenbitTask.Services
{
    public interface IFileService
    {
        public Task<bool> SaveFile(IFormFile file);
    }
}
