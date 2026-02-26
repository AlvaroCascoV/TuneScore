using System.ComponentModel.DataAnnotations;

namespace TuneScore.Models
{
    public class EditProfileViewModel
    {
        public int IdUser { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        public string? NewPassword { get; set; }

        [Compare(nameof(NewPassword))]
        public string? ConfirmPassword { get; set; }
    }
}
