using DocumentMS.Data;
using DocumentMS.Models;
using DocumentMS.Models.CommonViewModel;
using DocumentMS.Models.ManageUserRolesVM;
using DocumentMS.Pages;

namespace DocumentMS.Services
{
    public interface IRoles
    {
        Task GenerateRolesFromPageList();
        Task<string> CreateSingleRole(string _RoleName);
        Task AddToRoles(ApplicationUser _ApplicationUser);
        Task<MainMenuViewModel> RolebaseMenuLoad(ApplicationUser _ApplicationUser);
        Task<MainMenuViewModel> ManageUserRolesDetailsByUser(ApplicationUser _ApplicationUser, ApplicationDbContext _context);
        Task<List<ManageUserRolesViewModel>> GetRolesByUser(GetRolesByUserViewModel vm);
        Task<List<ManageUserRolesViewModel>> GetRoleList();
        Task<JsonResultViewModel> UpdateUserRoles(ManageUserRolesCRUDViewModel vm);
    }
}
