using System.Threading.Tasks;

namespace ReenbitTask.Services
{
    public interface IEmailService
    {
        public Task<bool> SendEmail(string email);
    }
}
