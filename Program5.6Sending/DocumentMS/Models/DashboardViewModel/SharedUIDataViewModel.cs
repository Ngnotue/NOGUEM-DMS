using DocumentMS.Models.AccountViewModels;
using DocumentMS.Models.CommonViewModel;
using DocumentMS.Pages;

namespace DocumentMS.Models.DashboardViewModel
{
    public class SharedUIDataViewModel
    {
        public UserProfile UserProfile { get; set; }
        public ApplicationInfo ApplicationInfo { get; set; }
        public MainMenuViewModel MainMenuViewModel { get; set; }
        public LoginViewModel LoginViewModel { get; set; }
    }
}
