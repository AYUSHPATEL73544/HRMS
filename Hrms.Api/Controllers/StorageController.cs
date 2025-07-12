using Hrms.Core.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hrms.Api.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public StorageController( IStorageService storageService,
            IWebHostEnvironment hostingEnvironment)
        {
            _storageService = storageService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("upload-single")]
        public async Task<IActionResult> Upload(IFormFile file, [FromQuery] string key)
        {
            string documentsPath = Path.Combine("documents"); // Relative path within wwwroot
            string storagePath = Path.Combine(_hostingEnvironment.WebRootPath, documentsPath);
          
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            using (var stream = file.OpenReadStream())
            {
                await _storageService.UploadAsync(stream, key, storagePath);
                return Ok();
            }

        }

        [HttpDelete("delete-single/{key}")]
        public IActionResult DeleteSingle([FromRoute] string key)
        {
            string documentsPath = Path.Combine("documents"); // Relative path within wwwroot
            string storagePath = Path.Combine(_hostingEnvironment.WebRootPath, documentsPath);

            _storageService.Delete(key, storagePath);

            return Ok();
        }
    }
}
