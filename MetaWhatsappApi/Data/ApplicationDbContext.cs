using Microsoft.EntityFrameworkCore;



namespace MetaWhatsappApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApiCredentialsTbl> ApiCredentialsTbl { get; set; }
        public DbSet<UserTbl> UserTbl { get; set; }
        public DbSet<WhatsAppConfigs> WhatsAppConfigs { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiCredentialsTbl>()
                .HasIndex(x => x.ApiKey)
                .IsUnique();

            modelBuilder.Entity<ApiCredentialsTbl>()
                .HasOne(x => x.User)
                .WithMany(u => u.ApiCredentials)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<WhatsAppConfigs>()
     .HasOne(x => x.User)
     .WithMany()
     .HasForeignKey(x => x.UserId)
     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
