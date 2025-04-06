using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class FirebaseTokenModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public string FirebaseToken { get; set; }
        public string DeviceName { get; set; }
        public FirebaseTokenModel(Guid userId, string firebaseToken, string deviceName)
        {
            this.UserId = userId;
            this.FirebaseToken = firebaseToken;
            this.DeviceName = deviceName;
        }
    }
}