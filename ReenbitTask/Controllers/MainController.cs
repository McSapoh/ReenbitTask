using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReenbitTask.Models;
using ReenbitTask.Services;
using System.Threading.Tasks;

namespace ReenbitTask.Controllers
{
    public class MainController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        private readonly ILogger<MainController> _logger;

        [BindProperty]
        private SaveModel model { get; set; }
        public MainController(IEmailService emailService, IFileService fileService, ILogger<MainController> logger)
        {
            _emailService = emailService;
            _fileService = fileService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            model = new SaveModel();
            return View(model);
        }

        /// <summary>
        /// Saves file to file system.
        /// </summary>
        /// <response code="200">Successfully saved</response>
        /// <response code="400">Error while saving</response>
        /// <response code="409">Model is not valid</response>
        /// <response code="500">Email was not send</response>
        [HttpPost("Save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveAndSend([FromForm] SaveModel model)
        {
            _logger.LogInformation($"POST {this}.SaveAndSend called.");

            // Validating model.
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"POST {this}.SaveAndSend model is not valid.");
                return Conflict(ModelState);
            }

            // Saving file.
            var serviceResult = await _fileService.SaveFile(model.File);

            // Validating serviceResult.
            if (!serviceResult)
            {
                _logger.LogInformation($"POST {this}.SaveAndSend file was not send.");
                return BadRequest();
            }

            // Sending email.
            serviceResult = await _emailService.SendEmail(model.Email);

            // Validating serviceResult.
            _logger.LogInformation($"POST {this}.SaveAndSend finished.");
            if (serviceResult)
                return Ok();
            else
                return StatusCode(500);
        }
    }
}
