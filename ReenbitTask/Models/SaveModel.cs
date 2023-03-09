using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ReenbitTask.Models
{
    public class SaveModel
    {
        [EmailAddress(ErrorMessage = "Please enter your email")]
        public string Email { get; set; }
        public IFormFile File { get; set; }

    }
}
