using System.ComponentModel.DataAnnotations;

namespace shortenerURL_vibing.Models
{
    public class UrlModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Original URL is required")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        [Display(Name = "Original URL")]
        public string OriginalUrl { get; set; } = string.Empty;

        [Display(Name = "Shortened URL")]
        public string ShortenedUrl { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Click Count")]
        public int ClickCount { get; set; } = 0;

        [Display(Name = "Custom Alias")]
        public string? CustomAlias { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
    }
}
