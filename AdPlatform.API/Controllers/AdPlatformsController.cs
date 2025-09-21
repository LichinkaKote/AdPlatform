using AdPlatform.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdPlatformsController : ControllerBase
    {
        private readonly IStorage _adStorage;

        public AdPlatformsController(IStorage adStorage)
        {
            _adStorage = adStorage;
        }

        [HttpPost("upload")]
        public IActionResult UploadPlatforms(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using var stream = file.OpenReadStream();
                _adStorage.LoadData(stream);
                return Ok("Data uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public IActionResult SearchPlatforms([FromQuery] string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                return BadRequest("Location parameter is required.");
            }

            try
            {
                var platforms = _adStorage.FindPlatformsForLocation(location);
                return Ok(platforms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
