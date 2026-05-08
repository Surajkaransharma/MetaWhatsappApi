using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MetaWhatsappApi.Data
{
    public class ApiCredentialsTbl
    {
        [Key]
        public int ApiCid { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(64)]
        public string ApiKey { get; set; }

        [Required]
        [StringLength(256)]
        public string SecretKeyHash { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiresAt { get; set; }

        // 🔗 Navigation Property
        [ForeignKey("UserId")]
        public virtual UserTbl User { get; set; }
    }
}
