using DocumentMS.Data;
using DocumentMS.Models;
using DocumentMS.Models.DocumentHistoryViewModel;
using DocumentMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace DocumentMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class DocumentHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public DocumentHistoryController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.DocumentHistory.RoleName)]
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

                var _GetGridItem = _iCommon.GetDocumentHistoryList();
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
                    || obj.DocumentId.ToString().ToLower().Contains(searchValue)
                    || obj.Action.ToLower().Contains(searchValue)
                    || obj.AssignEmployeeId.ToString().Contains(searchValue)
                    || obj.Note.ToLower().Contains(searchValue)
                    //|| obj.CreatedDateDisplay.ToLower().Contains(searchValue)
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
            DocumentHistoryCRUDViewModel vm = await _context.DocumentHistory.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            DocumentHistoryCRUDViewModel vm = new();
            if (id > 0) vm = await _context.DocumentHistory.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(DocumentHistoryCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        DocumentHistory _AssetHistory = new DocumentHistory();
                        if (vm.Id > 0)
                        {
                            _AssetHistory = await _context.DocumentHistory.FindAsync(vm.Id);

                            vm.CreatedDate = _AssetHistory.CreatedDate;
                            vm.CreatedBy = _AssetHistory.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_AssetHistory).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Asset History Updated Successfully. ID: " + _AssetHistory.Id;
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _AssetHistory = vm;
                            _AssetHistory.CreatedDate = DateTime.Now;
                            _AssetHistory.ModifiedDate = DateTime.Now;
                            _AssetHistory.CreatedBy = HttpContext.User.Identity.Name;
                            _AssetHistory.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_AssetHistory);
                            await _context.SaveChangesAsync();
                            TempData["successAlert"] = "Asset History Created Successfully. ID: " + _AssetHistory.Id;
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    TempData["errorAlert"] = "Operation failed.";
                    return View("Index");
                }
                catch (Exception) { throw; }
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _AssetHistory = await _context.DocumentHistory.FindAsync(id);
                _AssetHistory.ModifiedDate = DateTime.Now;
                _AssetHistory.ModifiedBy = HttpContext.User.Identity.Name;
                _AssetHistory.Cancelled = true;

                _context.Update(_AssetHistory);
                await _context.SaveChangesAsync();
                return new JsonResult(_AssetHistory);
            }
            catch (Exception) { throw; }
        }
    }
}
