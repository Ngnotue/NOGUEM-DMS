using System.Linq;
using System.Threading.Tasks;
using DocumentMS.Services;

namespace DocumentMS.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, IFunctional functional)
        {
            context.Database.EnsureCreated();
            await functional.GetDefaultIdentitySettings();

            if (context.ApplicationUser.Any())
            {
                return;
            }
            else
            {
                await functional.CreateDefaultSuperAdmin();
                await functional.CreateDefaultEmailSettings();
                await functional.CreateDefaultIdentitySettings();
                await functional.CreateDocument();
                await functional.InitAppData();
                await functional.GenerateUserUserRole();
                await functional.CreateDefaultOtherUser();
            }
        }
    }
}
