using DocumentMS.Models;
using DocumentMS.Models.UserProfileViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using DocumentMS.Models.AccountViewModels;

namespace DocumentMS.ConHelper
{
    public interface IAccount
    {
        Task<Tuple<ApplicationUser, IdentityResult>> CreateUserAccount(CreateUserAccountViewModel _CreateUserAccountViewModel);
        Task<Tuple<ApplicationUser, string>> CreateUserProfile(UserProfileCRUDViewModel vm, string LoginUser);
    }
}
