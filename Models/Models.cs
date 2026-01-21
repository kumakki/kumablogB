using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kumablogB.Models
{
    public class Users
    {
        public string Id { get; set; } = "";
        public string UserId { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Profile { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }

    public class Blogs
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public List<string> Tags { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }

    public class Sessions
    {
        [Key]
        public string AuthToken { get; set; } = "";

        public string UserId { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
