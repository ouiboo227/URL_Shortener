using Microsoft.AspNetCore.Mvc;
using shortenerURL_vibing.Models;
using shortenerURL_vibing.Services;

namespace shortenerURL_vibing.Controllers
{
    public class UrlController : Controller
    {
        private readonly IUrlService _urlService;

        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        // GET: Url
        public async Task<IActionResult> Index()
        {
            var urls = await _urlService.GetAllUrlsAsync();
            return View(urls);
        }

        // GET: Url/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Url/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OriginalUrl,CustomAlias")] UrlModel urlModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if custom alias is unique if provided
                    if (!string.IsNullOrEmpty(urlModel.CustomAlias))
                    {
                        if (!await _urlService.IsCustomAliasUniqueAsync(urlModel.CustomAlias))
                        {
                            ModelState.AddModelError("CustomAlias", "This custom alias is already in use.");
                            return View(urlModel);
                        }
                        urlModel.ShortenedUrl = urlModel.CustomAlias;
                    }

                    var createdUrl = await _urlService.CreateUrlAsync(urlModel);
                    TempData["SuccessMessage"] = "URL shortened successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the shortened URL.");
                }
            }
            return View(urlModel);
        }

        // GET: Url/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var urlModel = await _urlService.GetUrlByIdAsync(id.Value);
            if (urlModel == null)
                return NotFound();

            return View(urlModel);
        }

        // POST: Url/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OriginalUrl,CustomAlias,IsActive")] UrlModel urlModel)
        {
            if (id != urlModel.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if custom alias is unique if changed
                    var existingUrl = await _urlService.GetUrlByIdAsync(id);
                    if (existingUrl?.CustomAlias != urlModel.CustomAlias && 
                        !string.IsNullOrEmpty(urlModel.CustomAlias))
                    {
                        if (!await _urlService.IsCustomAliasUniqueAsync(urlModel.CustomAlias))
                        {
                            ModelState.AddModelError("CustomAlias", "This custom alias is already in use.");
                            return View(urlModel);
                        }
                    }

                    await _urlService.UpdateUrlAsync(urlModel);
                    TempData["SuccessMessage"] = "URL updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the URL.");
                }
            }
            return View(urlModel);
        }

        // GET: Url/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var urlModel = await _urlService.GetUrlByIdAsync(id.Value);
            if (urlModel == null)
                return NotFound();

            return View(urlModel);
        }

        // POST: Url/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _urlService.DeleteUrlAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "URL deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete URL.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Url/RedirectToUrl/{shortCode}
        public async Task<IActionResult> RedirectToUrl(string shortCode)
        {
            if (string.IsNullOrEmpty(shortCode))
                return NotFound();

            var urlModel = await _urlService.GetUrlByShortCodeAsync(shortCode);
            if (urlModel == null)
                return NotFound();

            // Increment click count
            await _urlService.IncrementClickCountAsync(urlModel.Id);

            // Redirect to original URL in new tab
            return Redirect(urlModel.OriginalUrl);
        }

        // GET: Url/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var urlModel = await _urlService.GetUrlByIdAsync(id.Value);
            if (urlModel == null)
                return NotFound();

            return View(urlModel);
        }
    }
}
