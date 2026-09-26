using DocumentMS.ConHelper;
using DocumentMS.Data;
using DocumentMS.Models;
using DocumentMS.Models.DocumentViewModel;
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
    public class DocumentDeepSearchController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommon _iCommon;
        private readonly IDBOperation _IDBOperation;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _iHostingEnvironment;
        private string projectRootPath;
        private string outputPath;
        private string storagePath;

        public DocumentDeepSearchController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ICommon iCommon, IDBOperation IDBOperation, IEmailSender emailSender, IWebHostEnvironment iHostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _iCommon = iCommon;
            _IDBOperation = IDBOperation;
            _emailSender = emailSender;

            _iHostingEnvironment = iHostingEnvironment;
            projectRootPath = _iHostingEnvironment.ContentRootPath;
            outputPath = Path.Combine(projectRootPath, "wwwroot/upload/FileView");
            storagePath = Path.Combine(projectRootPath, "wwwroot/upload");
        }

        [Authorize(Roles = Pages.MainMenu.Document.RoleName)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDataTabelData()
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

                IQueryable<DocumentCRUDViewModel> _GetGridItem = null;
                var _IsInRole = User.IsInRole("Admin");
                if (_IsInRole)
                {
                    _GetGridItem = _iCommon.GetDocumentGridItem();
                }
                else
                {
                    var _GetLoginEmployeeId = await GetLoginEmployeeId();
                    _GetGridItem = _iCommon.GetDocumentGridItem().Where(x => x.AssignEmployeeId == _GetLoginEmployeeId);
                }

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
        [HttpPost]
        public async Task<JsonResult> SearchInInnerDoc(string SearchItem)
        {
            try
            {
                SearchItem = SearchItem.ToLower();
                List<DocumentCRUDViewModel> listDocumentCRUDViewModel = new();
                var _DocumentFileContent = await _context.DocumentFileContent.Where(x => x.Cancelled == false).ToListAsync();
                var _GetDocumentGridItem = await _iCommon.GetDocumentGridItem().ToListAsync();

                foreach (var item in _DocumentFileContent)
                {
                    var _Content = item.Content.ToLower();
                    var _Contains = _Content.Contains(SearchItem);
                    if (_Contains)
                    {
                        var result = _GetDocumentGridItem.Where(x => x.Id == item.DocumentId).SingleOrDefault();
                        listDocumentCRUDViewModel.Add(result);
                    }
                }
                return new JsonResult(listDocumentCRUDViewModel);
            }
            catch (Exception) { throw; }
        }
        private async Task<Int64> GetLoginEmployeeId()
        {
            Int64 _UserProfileId = 0;
            var _UserEmail = HttpContext.User.Identity.Name;
            var _ApplicationUser = await _userManager.FindByEmailAsync(_UserEmail);
            if (_ApplicationUser != null)
            {
                _UserProfileId = _context.UserProfile.Where(x => x.ApplicationUserId == _ApplicationUser.Id).SingleOrDefault().UserProfileId;
            }
            return _UserProfileId;
        }
    }
}
