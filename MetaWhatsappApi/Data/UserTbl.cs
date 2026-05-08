using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MetaWhatsappApi.Data
{
    public class UserTbl
    {
      
     
            [Key]
            public int UserId { get; set; }

            [Required]
            [StringLength(100)]
            public string Name { get; set; }

            [Required]
            [StringLength(150)]
            public string Email { get; set; }

            public bool IsActive { get; set; } = true;

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            // 🔁 One-to-Many Relation
            public virtual ICollection<ApiCredentialsTbl> ApiCredentials { get; set; }
        }
    
}
