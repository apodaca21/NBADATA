using System.ComponentModel.DataAnnotations;

namespace AuthDemoApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(32)]
        public string Username { get; set; } = null!;

        [Required, MaxLength(256)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(128)]
        public string FullName { get; set; } = null!;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; } = null!;

        [Required]
        public byte[] PasswordSalt { get; set; } = null!;
    }
}
