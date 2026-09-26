using DocumentMS.Data;
using DocumentMS.Models;
using DocumentMS.Models.UserProfileViewModel;
using DocumentMS.Pages;
using DocumentMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace DocumentMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class UserProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileController(UserManager<ApplicationUser> userManager, ICommon iCommon,
            ApplicationDbContext context)
        {
            _context = context;
            _iCommon = iCommon;
            _userManager = userManager;
        }

        [Authorize(Roles = Pages.MainMenu.UserProfile.RoleName)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var _UserName = User.Identity.Name;
            var _UserProfile = _context.UserProfile.FirstOrDefault(x => x.Email == _UserName);
            var result = await _iCommon.GetUserProfileDetails().Where(x => x.UserProfileId == _UserProfile.UserProfileId).SingleOrDefaultAsync();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string ApplicationUserId)
        {
            var _ApplicationUser = await _userManager.FindByIdAsync(ApplicationUserId);
            ResetPasswordViewModel _ResetPasswordViewModel = new();
            _ResetPasswordViewModel.ApplicationUserId = _ApplicationUser.Id;
            return PartialView("_ResetPassword", _ResetPasswordViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveResetPassword(ResetPasswordViewModel vm)
        {
            try
            {
                string AlertMessage = string.Empty;
                var _ApplicationUser = await _userManager.FindByIdAsync(vm.ApplicationUserId);
                if (_ApplicationUser.Email.Trim() == "admin@gmail.com")
                {
                    return new JsonResult("You not allowed to chnage demo user info. User Name: " + _ApplicationUser.Email);
                }

                if (vm.NewPassword.Equals(vm.ConfirmPassword))
                {
                    var result = await _userManager.ChangePasswordAsync(_ApplicationUser, vm.OldPassword, vm.NewPassword);
                    if (result.Succeeded)
                        AlertMessage = "Change Password Succeeded. User name: " + _ApplicationUser.Email;
                    else
                    {
                        string errorMessage = string.Empty;
                        foreach (var item in result.Errors)
                        {
                            errorMessage = errorMessage + " " + item.Description;
                        }
                        AlertMessage = "error" + errorMessage;
                    }
                }
                return new JsonResult(AlertMessage);
            }
            catch (Exception ex)
            {
                return new JsonResult("error" + ex.Message);
                throw;
            }
        }
    }
}
