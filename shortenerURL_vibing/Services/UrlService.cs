using Microsoft.EntityFrameworkCore;
using shortenerURL_vibing.Data;
using shortenerURL_vibing.Models;
using System.Security.Cryptography;
using System.Text;

namespace shortenerURL_vibing.Services
{
    public class UrlService : IUrlService
    {
        private readonly UrlDbContext _context;

        public UrlService(UrlDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UrlModel>> GetAllUrlsAsync()
        {
            return await _context.Urls
                .Where(u => u.IsActive)
                .OrderByDescending(u => u.CreatedDate)
                .ToListAsync();
        }

        public async Task<UrlModel?> GetUrlByIdAsync(int id)
        {
            return await _context.Urls.FindAsync(id);
        }

        public async Task<UrlModel?> GetUrlByShortCodeAsync(string shortCode)
        {
            return await _context.Urls
                .FirstOrDefaultAsync(u => u.ShortenedUrl == shortCode && u.IsActive);
        }

        public async Task<UrlModel> CreateUrlAsync(UrlModel urlModel)
        {
            // Generate short code if not provided
            if (string.IsNullOrEmpty(urlModel.ShortenedUrl))
            {
                urlModel.ShortenedUrl = await GenerateShortCodeAsync();
            }

            // Set creation date
            urlModel.CreatedDate = DateTime.Now;
            urlModel.ClickCount = 0;
            urlModel.IsActive = true;

            _context.Urls.Add(urlModel);
            await _context.SaveChangesAsync();

            return urlModel;
        }

        public async Task<UrlModel> UpdateUrlAsync(UrlModel urlModel)
        {
            var existingUrl = await _context.Urls.FindAsync(urlModel.Id);
            if (existingUrl == null)
                throw new InvalidOperationException("URL not found");

            existingUrl.OriginalUrl = urlModel.OriginalUrl;
            existingUrl.CustomAlias = urlModel.CustomAlias;
            existingUrl.IsActive = urlModel.IsActive;

            await _context.SaveChangesAsync();
            return existingUrl;
        }

        public async Task<bool> DeleteUrlAsync(int id)
        {
            var url = await _context.Urls.FindAsync(id);
            if (url == null)
                return false;

            _context.Urls.Remove(url);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IncrementClickCountAsync(int id)
        {
            var url = await _context.Urls.FindAsync(id);
            if (url == null)
                return false;

            url.ClickCount++;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GenerateShortCodeAsync()
        {
            string shortCode;
            do
            {
                shortCode = GenerateRandomString(6);
            } while (!await IsShortCodeUniqueAsync(shortCode));

            return shortCode;
        }

        public async Task<bool> IsShortCodeUniqueAsync(string shortCode)
        {
            return !await _context.Urls.AnyAsync(u => u.ShortenedUrl == shortCode);
        }

        public async Task<bool> IsCustomAliasUniqueAsync(string customAlias)
        {
            if (string.IsNullOrEmpty(customAlias))
                return true;

            return !await _context.Urls.AnyAsync(u => u.CustomAlias == customAlias);
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
