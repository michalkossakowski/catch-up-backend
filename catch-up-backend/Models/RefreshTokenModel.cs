using System.ComponentModel.DataAnnotations;

namespace catch_up_backend.Models
{
    public class RefreshTokenModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }

        public RefreshTokenModel(Guid userId, string token, DateTime expiresAt)
        {
            this.Id = Guid.NewGuid();
            this.UserId = userId;
            this.Token = token;
            this.ExpiresAt = expiresAt;
        }
    }
}