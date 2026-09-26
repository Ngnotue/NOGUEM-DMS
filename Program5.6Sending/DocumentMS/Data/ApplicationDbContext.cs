using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DocumentMS.Models;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace DocumentMS.Data
{
    // This factory class is for design time only (To use ef tools like Update-Database) on remote access
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            // Add the conn string here first
            string connString = "";
            optionsBuilder.UseSqlServer(connString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }

        public DbSet<SMTPEmailSetting> SMTPEmailSetting { get; set; }
        public DbSet<SendGridSetting> SendGridSetting { get; set; }
        public DbSet<DefaultIdentityOptions> DefaultIdentityOptions { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }

        //DocumentMS
        public DbSet<Document> Document { get; set; }
        public DbSet<DocumentFile> DocumentFile { get; set; }
        public DbSet<DocumentFileContent> DocumentFileContent { get; set; }
        public DbSet<DocumentCategories> DocumentCategories { get; set; }
        public DbSet<DocumentStatus> DocumentStatus { get; set; }
        public DbSet<DocumentHistory> DocumentHistory { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<EmailConfig> EmailConfig { get; set; }
        public DbSet<EmailTemplate> EmailTemplate { get; set; }
        public DbSet<ManageUserRoles> ManageUserRoles { get; set; }
        public DbSet<ManageUserRolesDetails> ManageUserRolesDetails { get; set; }
        public DbSet<UserInfoFromBrowser> UserInfoFromBrowser { get; set; }
    }
}
