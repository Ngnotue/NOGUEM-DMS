using DocumentFormat.OpenXml.InkML;
using DocumentMS.ConHelper;
using DocumentMS.Data;
using DocumentMS.Helpers;
using DocumentMS.Models;
using DocumentMS.Models.CommonViewModel;
using DocumentMS.Models.DocumentHistoryViewModel;
using DocumentMS.Models.DocumentViewModel;
using DocumentMS.Models.EmailConfigViewModel;
using DocumentMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Text.RegularExpressions;


namespace DocumentMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class DocumentController : Controller
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
        private string _StartDate = null;
        private string _EndDate = null;

        public DocumentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ICommon iCommon, IDBOperation IDBOperation, IEmailSender emailSender, IWebHostEnvironment iHostingEnvironment)
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
        public IActionResult Index(string StartDate, string EndDate)
        {
            if (StartDate != null && EndDate != null)
            {
                HttpContext.Session.SetString("_StartDate", StartDate);
                HttpContext.Session.SetString("_EndDate", EndDate);
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;
            }
            else
            {
                HttpContext.Session.SetString("_StartDate", string.Empty);
                HttpContext.Session.SetString("_EndDate", string.Empty);
                ViewBag.StartDate = "Min";
                ViewBag.EndDate = "Max";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDataTabelData()
        {
            try
            {
                _StartDate = this.HttpContext.Session.GetString("_StartDate");
                _EndDate = this.HttpContext.Session.GetString("_EndDate");

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

                if (_StartDate != null && _EndDate != null && _StartDate != "" && _EndDate != "")
                {
                    _GetGridItem = _GetGridItem.Where(x => x.CreatedDate >= Convert.ToDateTime(_StartDate) && x.CreatedDate <= Convert.ToDateTime(_EndDate).AddDays(1));
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

            // Extension
            var usersNotShared = _iCommon.GetddlEmployee();
            var allowedList = vm.listUsersAllowedCRUDViewModel.Select(u => u.UserProfileId).ToList();
            usersNotShared = usersNotShared.Where(u => !allowedList.Contains(u.Id));
            ViewBag.GetddlEmployee = new SelectList(usersNotShared, "Id", "Name");
            // Get the id of current user
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            // Check if user is admin
            var isUserAdmin = User.Claims.FirstOrDefault(x => x.Value == "Admin");
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
            vm.IsUserAssigned = vm.AssignEmployeeId.Equals(userId);
            return PartialView("_DocInfo", vm);
        }
        [HttpPost]
        public async Task<JsonResult> AddAllowedUserToDoc(int userProfileId, int documentId)
        {
            var userProfile = _context.UserProfile.FirstOrDefault(u => u.UserProfileId == userProfileId);
            var document = _context.Document
                .Include(u => u.SharedUsers)
                .FirstOrDefault(d => d.Id == documentId);
            try
            {
                document.SharedUsers.Add(userProfile);
                _context.Entry(document).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<JsonResult> RemoveAllowedUserToDoc(int userProfileId, int documentId)
        {
            var userProfile = _context.UserProfile.FirstOrDefault(u => u.UserProfileId == userProfileId);
            var document = _context.Document
                .Include(u => u.SharedUsers)
                .FirstOrDefault(d => d.Id == documentId);
            try
            {
                document.SharedUsers.Remove(userProfile);
                _context.Entry(document).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            DocumentCRUDViewModel vm = new();
            var _DocumentCategories = _iCommon.DropdownData<DocumentCategories>().Where(x => x.Cancelled == false).OrderByDescending(x => x.Id);
            ViewBag.ddlDocumentCategories = new SelectList(_DocumentCategories, "Id", "Name");
            var _DocumentStatus = _iCommon.DropdownData<DocumentStatus>().Where(x => x.Cancelled == false).OrderBy(x => x.Id);
            ViewBag.ddlDocumentStatus = new SelectList(_DocumentStatus, "Id", "Name");
            ViewBag.GetddlEmployee = new SelectList(_iCommon.GetddlEmployee(), "Id", "Name");

            List<Comment> _listComment = new();

            if (id > 0)
            {
                vm = await _context.Document.Where(x => x.Id == id).Include(x=>x.SharedUsers).SingleOrDefaultAsync();
                vm.SelectedUserProfiles = new List<UserProfile>();

                _listComment = _context.Comment.Where(x => x.DocumentId == id && x.Cancelled == false).ToList();
                vm.listComment = _listComment;
                vm.listDocumentFileCRUDViewModel = _iCommon.GetDocumentFileList().Where(x => x.DocumentId == id).ToList();
                return PartialView("_Edit", vm);
            }
            else
            {
                vm.FilesDirName = StaticData.GetUniqueIDOnlyDate("Doc_");
                vm.CurrentUserId = HttpContext.User.Identity.Name;
                vm.listComment = _listComment;
                vm.Name = "test";
                return PartialView("_Add", vm);
            }
        }

        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [HttpPost]
        public async Task<JsonResult> AddEdit(DocumentCRUDViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            string _UserName = HttpContext.User.Identity.Name;
            vm.lisFiles = Request.Form.Files;

            try
            {
                if (ModelState.IsValid)
                {
                    Document _Document = new();
                    if (vm.Id > 0)
                    {
                        _Document = await _context.Document.FindAsync(vm.Id);
                        var _AssetAllocationUpdate = await DocumentAllocationUpdate(vm, _Document);

                        vm.CreatedDate = _Document.CreatedDate;
                        vm.CreatedBy = _Document.CreatedBy;
                        vm.ModifiedDate = DateTime.Now;
                        vm.ModifiedBy = _UserName;
                        _context.Entry(_Document).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();

                        _JsonResultViewModel.AlertMessage = "Document Updated Successfully. ID: " + _Document.Id;
                        _JsonResultViewModel.Id = _Document.Id;
                        _JsonResultViewModel.OperationTyep = CRUD.Edit;
                        return new JsonResult(_JsonResultViewModel);
                    }
                    else
                    {
                        vm.FilesDirName = vm.Name + "_" + vm.FilesDirName;
                        _Document = vm;
                        _Document.FilesPath = "/upload/" + vm.FilesDirName;
                        _Document.CreatedDate = DateTime.Now;
                        _Document.ModifiedDate = DateTime.Now;
                        _Document.CreatedBy = _UserName;
                        _Document.ModifiedBy = _UserName;
                        _context.Add(_Document);
                        await _context.SaveChangesAsync();

                        //Add Files
                        vm.Id = _Document.Id;
                        vm.UserName = _UserName;
                        if (vm.lisFiles.Count == 0)
                        {
                            var result = await _IDBOperation.AddDefaultFile(vm);
                        }
                        else
                        {
                            var result = await _IDBOperation.AddMultipleFile(vm);
                            foreach (var item in result)
                            {
                                if (_iCommon.IsAllowedFileExtension(item.FilePath))
                                {
                                    var result2 = await _IDBOperation.AddDocumentFileContent(result, _UserName);
                                }
                            }
                        }

                        await AddDocumentHistory(_Document.Id, _Document.AssignEmployeeId, "Document Created.");
                        if (vm.Id != 0)
                        {
                            await AddDocumentHistory(_Document.Id, vm.AssignEmployeeId, "Unassigned Document Assigned to Employee.");
                        }

                        _JsonResultViewModel.AlertMessage = "Document Created Successfully. ID: " + _Document.Id;
                        _JsonResultViewModel.Id = _Document.Id;
                        _JsonResultViewModel.OperationTyep = CRUD.Add;
                        return new JsonResult(_JsonResultViewModel);
                    }
                }

                _JsonResultViewModel.AlertMessage = "Operation failed.";
                return new JsonResult(_JsonResultViewModel);
            }
            catch (Exception) { throw; }
        }

        [HttpPost]
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _Document = await _context.Document.FindAsync(id);
                _Document.ModifiedDate = DateTime.Now;
                _Document.ModifiedBy = HttpContext.User.Identity.Name;
                _Document.Cancelled = true;

                _context.Update(_Document);
                await _context.SaveChangesAsync();
                return new JsonResult(_Document);
            }
            catch (Exception) { throw; }
        }

        [HttpPost]
        public async Task<JsonResult> ShareDocUsingEmail(SendEmailViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                _JsonResultViewModel = await FileMissingChecking(vm.DocumentId);
                if (!_JsonResultViewModel.IsSuccess)
                {
                    return new JsonResult(_JsonResultViewModel);
                }

                var _Document = await _context.Document.FindAsync(vm.DocumentId);
                //EmailConfigCRUDViewModel _EmailConfigCRUDViewModel = await _context.EmailConfig.FindAsync(vm.SenderEmailId);
                //SendEmailViewModel _SendEmailViewModel = _EmailConfigCRUDViewModel;
                SMTPEmailSetting _smtpOptions = await _iCommon.GetSMTPEmailSetting();
                SendEmailViewModel _SendEmailViewModel = _smtpOptions;

                _SendEmailViewModel.Subject = vm.Subject;
                _SendEmailViewModel.Body = vm.Body;
                _SendEmailViewModel.ReceiverEmail = vm.ReceiverEmail;
                _SendEmailViewModel.IsSSL = true;
                _SendEmailViewModel.Document = _Document;
                _SendEmailViewModel.listDocumentFile = await _context.DocumentFile.Where(x => x.DocumentId == vm.DocumentId).ToListAsync();
                var result = await _emailSender.SendEmailByGmailAsync(_SendEmailViewModel);

                if (result.Status == TaskStatus.RanToCompletion)
                {
                    await AddDocumentHistory(_Document.Id, _Document.AssignEmployeeId, "Document Shared Using Email.");
                    _JsonResultViewModel.AlertMessage = "Email Send Successfully. Document Name: " + _Document.Name;
                    _JsonResultViewModel.IsSuccess = true;
                    _JsonResultViewModel.Id = _Document.Id;
                }
                else
                {
                    _JsonResultViewModel.AlertMessage = "Email Send Failed. Status: " + result.Status;
                    _JsonResultViewModel.IsSuccess = false;
                }
                return new JsonResult(_JsonResultViewModel);
            }
            catch (Exception) { throw; }
        }

        private async Task<int> DocumentAllocationUpdate(DocumentCRUDViewModel vm, Document _Document)
        {
            int _DocumentStatusValue = vm.DocumentStatus;
            if (_Document.AssignEmployeeId != vm.AssignEmployeeId)
            {
                if (_Document.AssignEmployeeId == 0)
                {
                    await AddDocumentHistory(_Document.Id, vm.AssignEmployeeId, "Unassigned Document Assigned to Employee.");
                    _DocumentStatusValue = DocumentStatusValue.Reviewing;
                }
                else
                {
                    if (vm.AssignEmployeeId == 0)
                    {
                        await AddDocumentHistory(_Document.Id, _Document.AssignEmployeeId, "Document Unassigned from Employee.");
                        _DocumentStatusValue = DocumentStatusValue.New;
                    }
                    else
                    {
                        await AddDocumentHistory(_Document.Id, _Document.AssignEmployeeId, "Document Unassigned from Employee.");
                        await AddDocumentHistory(_Document.Id, vm.AssignEmployeeId, "Document Assigned to Employee.");
                        _DocumentStatusValue = DocumentStatusValue.Reviewing;
                    }
                }
            }
            else
            {
                await AddDocumentHistory(_Document.Id, vm.AssignEmployeeId, "Document Updated.");
                _DocumentStatusValue = vm.DocumentStatus;
            }

            return _DocumentStatusValue;
        }
        private async System.Threading.Tasks.Task AddDocumentHistory(Int64 _DocumentId, Int64 _AssignEmployeeId, string _Action)
        {
            DocumentHistoryCRUDViewModel _AssetHistoryCRUDViewModel = new()
            {
                DocumentId = _DocumentId,
                AssignEmployeeId = _AssignEmployeeId,
                Action = _Action,
                UserName = HttpContext.User.Identity.Name
            };
            var result = await _iCommon.AddDocumentHistory(_AssetHistoryCRUDViewModel);
        }

        [HttpGet]
        public JsonResult DownloadFile(Int64 id)
        {
            try
            {
                var _Document = _context.DocumentFile.Find(id);
                var result = AddDocumentHistoryNonSync(_Document.Id, 103, "Document Downloaded.");

                if (_Document.IsFileSaveInDB)
                {
                    var FileExtension = Regex.Split(_Document.ContentType, "/");
                    _Document.DocByte = StaticUtility.Decrypt(_Document.DocByte, _Document.Name);
                    return new JsonResult(_Document);
                }
                else
                {
                    var _GetDownloadDetails = _iCommon.GetDownloadDetails(id);
                    _Document.ContentType = "application/octet-stream";
                    _Document.DocByte = _GetDownloadDetails.Item1;
                    return new JsonResult(_Document);
                }
            }
            catch (Exception) { throw; }
        }
        private DocumentHistory AddDocumentHistoryNonSync(Int64 _DocumentId, Int64 _AssignEmployeeId, string _Action)
        {
            DocumentHistory _DocumentHistory = new();
            DocumentHistoryCRUDViewModel _AssetHistoryCRUDViewModel = new()
            {
                DocumentId = _DocumentId,
                AssignEmployeeId = _AssignEmployeeId,
                Action = _Action,
                UserName = HttpContext.User.Identity.Name
            };

            _DocumentHistory = _iCommon.AddDocumentHistoryNoAsync(_AssetHistoryCRUDViewModel);
            return _DocumentHistory;
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

        [HttpPost]
        public async Task<JsonResult> SaveAddFilesInEditMode(DocumentCRUDViewModel vm)
        {
            try
            {
                vm.lisFiles = Request.Form.Files;
                vm.UserName = HttpContext.User.Identity.Name;
                var result = await _IDBOperation.AddMultipleFile(vm);
                var _AddDocumentHistoryNonSync = AddDocumentHistoryNonSync(vm.Id, vm.AssignEmployeeId, "New File Added.");

                var listDocumentFile = await _context.DocumentFile.Where(x => x.DocumentId == vm.Id && x.Cancelled == false).ToListAsync();
                return new JsonResult(listDocumentFile);
            }
            catch (Exception) { throw; }
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteDocumentFile(Int64 id)
        {
            try
            {
                var _DocumentFile = _context.DocumentFile.Find(id);
                var _GetContentPath = _iCommon.GetContentPath(_DocumentFile.FilePath);
                System.IO.File.Delete(_GetContentPath);


                _DocumentFile.ModifiedDate = DateTime.Now;
                _DocumentFile.ModifiedBy = HttpContext.User.Identity.Name;
                _DocumentFile.Cancelled = true;
                _context.Update(_DocumentFile);
                await _context.SaveChangesAsync();

                var result = AddDocumentHistoryNonSync(_DocumentFile.DocumentId, 101, "Document Deleted.");
                return new JsonResult(_DocumentFile);
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
        private async Task<JsonResultViewModel> FileMissingChecking(Int64 _DocumentId)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                var _DocumentFile = await _context.DocumentFile.Where(x => x.Id == _DocumentId).ToListAsync();
                int CountFileMissing = 0;
                foreach (var item in _DocumentFile)
                {
                    var _FilePath = _iCommon.GetContentPath(item.FilePath);
                    var linuxPath = _FilePath.Replace('\\', '/');
                    if (!System.IO.File.Exists(linuxPath))
                    {
                        CountFileMissing++;
                    }
                }
                if (CountFileMissing > 0)
                {
                    _JsonResultViewModel.AlertMessage = "Document File Deleted/Missing. Total File Deleted/Missing: " + CountFileMissing;
                    _JsonResultViewModel.IsSuccess = false;
                }
                else
                {
                    _JsonResultViewModel.IsSuccess = true;
                }
                return _JsonResultViewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
