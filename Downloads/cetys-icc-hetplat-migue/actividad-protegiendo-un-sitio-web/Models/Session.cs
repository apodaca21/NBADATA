using System.ComponentModel.DataAnnotations;

namespace AuthDemoApp.Models
{
    public class Session
    {
        public int Id { get; set; }

        [Required, MaxLength(64)]
        public string SessionId { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime LastActivityUtc { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
