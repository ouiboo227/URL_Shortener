using Microsoft.AspNetCore.Mvc;
using shortenerURL_vibing.Models;
using shortenerURL_vibing.Services;

namespace shortenerURL_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlApiController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlApiController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        // GET: api/UrlApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UrlModel>>> GetUrls()
        {
            try
            {
                var urls = await _urlService.GetAllUrlsAsync();
                return Ok(urls);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        // GET: api/UrlApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UrlModel>> GetUrl(int id)
        {
            try
            {
                var url = await _urlService.GetUrlByIdAsync(id);
                if (url == null)
                {
                    return NotFound(new { error = "URL not found" });
                }
                return Ok(url);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        // GET: api/UrlApi/redirect/{shortCode}
        [HttpGet("redirect/{shortCode}")]
        public async Task<ActionResult<object>> GetRedirectInfo(string shortCode)
        {
            try
            {
                var url = await _urlService.GetUrlByShortCodeAsync(shortCode);
                if (url == null)
                {
                    return NotFound(new { error = "Shortened URL not found" });
                }

                if (!url.IsActive)
                {
                    return BadRequest(new { error = "URL is inactive" });
                }

                // Increment click count
                await _urlService.IncrementClickCountAsync(url.Id);

                return Ok(new
                {
                    originalUrl = url.OriginalUrl,
                    shortCode = url.ShortenedUrl,
                    clickCount = url.ClickCount + 1
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        // POST: api/UrlApi
        [HttpPost]
        public async Task<ActionResult<UrlModel>> CreateUrl([FromBody] UrlModel urlModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if custom alias is unique if provided
                if (!string.IsNullOrEmpty(urlModel.CustomAlias))
                {
                    if (!await _urlService.IsCustomAliasUniqueAsync(urlModel.CustomAlias))
                    {
                        return BadRequest(new { error = "Custom alias already exists" });
                    }
                    urlModel.ShortenedUrl = urlModel.CustomAlias;
                }

                var createdUrl = await _urlService.CreateUrlAsync(urlModel);
                return CreatedAtAction(nameof(GetUrl), new { id = createdUrl.Id }, createdUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        // PUT: api/UrlApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUrl(int id, [FromBody] UrlModel urlModel)
        {
            try
            {
                if (id != urlModel.Id)
                {
                    return BadRequest(new { error = "ID mismatch" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if custom alias is unique if changed
                var existingUrl = await _urlService.GetUrlByIdAsync(id);
                if (existingUrl?.CustomAlias != urlModel.CustomAlias && 
                    !string.IsNullOrEmpty(urlModel.CustomAlias))
                {
                    if (!await _urlService.IsCustomAliasUniqueAsync(urlModel.CustomAlias))
                    {
                        return BadRequest(new { error = "Custom alias already exists" });
                    }
                }

                var updatedUrl = await _urlService.UpdateUrlAsync(urlModel);
                return Ok(updatedUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        // DELETE: api/UrlApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrl(int id)
        {
            try
            {
                var result = await _urlService.DeleteUrlAsync(id);
                if (!result)
                {
                    return NotFound(new { error = "URL not found" });
                }

                return Ok(new { message = "URL deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        // POST: api/UrlApi/5/increment-clicks
        [HttpPost("{id}/increment-clicks")]
        public async Task<ActionResult<object>> IncrementClicks(int id)
        {
            try
            {
                var result = await _urlService.IncrementClickCountAsync(id);
                if (!result)
                {
                    return NotFound(new { error = "URL not found" });
                }

                var url = await _urlService.GetUrlByIdAsync(id);
                return Ok(new { clickCount = url.ClickCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        // GET: api/UrlApi/check-alias/{alias}
        [HttpGet("check-alias/{alias}")]
        public async Task<ActionResult<object>> CheckAliasAvailability(string alias)
        {
            try
            {
                if (string.IsNullOrEmpty(alias))
                {
                    return BadRequest(new { error = "Alias cannot be empty" });
                }

                var isAvailable = await _urlService.IsCustomAliasUniqueAsync(alias);
                return Ok(new { alias = alias, isAvailable = isAvailable });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
    }
}
