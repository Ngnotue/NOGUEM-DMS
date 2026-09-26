using DocumentMS.Data;
using DocumentMS.Helpers;
using DocumentMS.Models;
using DocumentMS.Models.DocumentViewModel;
using DocumentMS.Models.EmailConfigViewModel;
using DocumentMS.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Security.Claims;


namespace DocumentMS.Controllers
{
    //[Authorize]
    [Route("[controller]/[action]")]
    public class DocumentPublicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public DocumentPublicController(ApplicationDbContext context, ICommon iCommon)
        {
            _iCommon = iCommon;
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetDataTabelData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int resultTotal = 0;

                IQueryable<DocumentCRUDViewModel> _GetGridItem = _iCommon.GetDocumentGridItem();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.Name.ToLower().Contains(searchValue)
                    || obj.CategoriesDisplay.ToLower().Contains(searchValue)
                    || obj.AssignEmployeeDisplay.ToLower().Contains(searchValue)
                    || obj.DocumentStatusDisplay.ToLower().Contains(searchValue)
                    || obj.FilesDirName.ToLower().Contains(searchValue)

                    || obj.Tag01.ToLower().Contains(searchValue)
                    || obj.Tag02.ToLower().Contains(searchValue)
                    || obj.Tag03.ToLower().Contains(searchValue)
                    || obj.Tag04.ToLower().Contains(searchValue)
                    || obj.Tag05.ToLower().Contains(searchValue)

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception) { throw; }
        }
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            ViewBag.GetddlEmailConfig = new SelectList(_iCommon.GetddlEmailConfig(), "Id", "Name");
            ViewBag.GetddlUserEmail = new SelectList(_iCommon.GetddlUserEmail(), "Id", "Name");

            DocumentCRUDViewModel vm = await _iCommon.GetDocumentList().Where(x => x.Id == id).SingleOrDefaultAsync();
            SendEmailViewModel _SendEmailViewModel = new();
            _SendEmailViewModel.Subject = EmailContent.Subject + vm.Name;
            _SendEmailViewModel.Body = EmailContent.Body;
            vm.SendEmailViewModel = _SendEmailViewModel;
            //vm.DocByteBase64 = Convert.ToBase64String(vm.DocByte);

            if (vm == null) return NotFound();

            //vm.FileViewName = Path.GetFileName(vm.FilePath);
            vm.listComment = _context.Comment.Where(x => x.DocumentId == id && x.Cancelled == false).ToList();
            vm.listDocumentFileCRUDViewModel = _iCommon.GetDocumentFileList().Where(x => x.DocumentId == id).ToList();
            vm.listDocumentHistoryCRUDViewModel = _iCommon.GetDocumentHistoryList().Where(x => x.DocumentId == id).ToList();
            if (vm.AssignEmployeeId != 0)
            {
                vm.UserProfileCRUDViewModel = _iCommon.GetByUserProfileInfo(vm.AssignEmployeeId);
            }

            // Exentsion
            // Check if user is admin
            var isUserAdmin = User.Claims.FirstOrDefault(x => x.Value == "Admin");
            // get the id of current user     
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (isUserAdmin is not null)
            {
                // user is admin then set allowed to true
                vm.IsUserAllowed = true;
            }
            else
            {
                // get the list of allowed users for this document and check if current user exists in it
                var isUserAllowed = vm.listUsersAllowedCRUDViewModel.FirstOrDefault(u => u.ApplicationUserId == userId);
                // if user id not found(null) set IsUserAllowed to false, else if exists set it to true
                vm.IsUserAllowed = isUserAllowed is not null;
            }
            //Check if user is assigned
            var assignedUserProfile = _iCommon.GetByUserProfile(vm.AssignEmployeeId);
            if (assignedUserProfile != null)
            {
                vm.IsUserAssigned = assignedUserProfile.ApplicationUserId.Equals(userId);
            }
            return PartialView("_DocInfo", vm);
        }
        [HttpGet]
        public async Task<JsonResult> GetViewingFileInfo(Int64 id)
        {
            try
            {
                var _DocumentFile = await _iCommon.GetDocumentFileList().Where(x => x.Id == id).SingleOrDefaultAsync();
                return new JsonResult(_DocumentFile);
            }
            catch (Exception) { throw; }
        }
    }
}
