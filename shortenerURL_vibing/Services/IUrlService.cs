using shortenerURL_vibing.Models;

namespace shortenerURL_vibing.Services
{
    public interface IUrlService
    {
        Task<IEnumerable<UrlModel>> GetAllUrlsAsync();
        Task<UrlModel?> GetUrlByIdAsync(int id);
        Task<UrlModel?> GetUrlByShortCodeAsync(string shortCode);
        Task<UrlModel> CreateUrlAsync(UrlModel urlModel);
        Task<UrlModel> UpdateUrlAsync(UrlModel urlModel);
        Task<bool> DeleteUrlAsync(int id);
        Task<bool> IncrementClickCountAsync(int id);
        Task<string> GenerateShortCodeAsync();
        Task<bool> IsShortCodeUniqueAsync(string shortCode);
        Task<bool> IsCustomAliasUniqueAsync(string customAlias);
    }
}
