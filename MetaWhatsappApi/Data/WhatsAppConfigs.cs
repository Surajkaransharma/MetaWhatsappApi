using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MetaWhatsappApi.Data
{
    public class WhatsAppConfigs
    {
        [Key]
        public int WCid { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string PhoneNumberId { get; set; }

        [Required]
        [StringLength(500)]
        public string AccessToken { get; set; }

        public bool IsActive { get; set; } = true;


        // 🔗 Navigation Property
        [ForeignKey("UserId")]
        public virtual UserTbl User { get; set; }
    }
}
