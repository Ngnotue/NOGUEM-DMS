using DocumentMS.Data;
using DocumentMS.Models;
using DocumentMS.Models.DocumentStatusViewModel;
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
    public class DocumentStatusController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public DocumentStatusController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.DocumentStatus.RoleName)]
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

                var _GetGridItem = GetGridItem();
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
                    || obj.Description.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
                    || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedBy.ToLower().Contains(searchValue)
                    || obj.ModifiedBy.ToLower().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception) { throw; }
        }

        private IQueryable<DocumentStatusCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _DocumentStatus in _context.DocumentStatus
                        where _DocumentStatus.Cancelled == false
                        select new DocumentStatusCRUDViewModel
                        {
                            Id = _DocumentStatus.Id,
                            Name = _DocumentStatus.Name,
                            Description = _DocumentStatus.Description,
                            CreatedDate = _DocumentStatus.CreatedDate,
                            ModifiedDate = _DocumentStatus.ModifiedDate,
                            CreatedBy = _DocumentStatus.CreatedBy,
                            ModifiedBy = _DocumentStatus.ModifiedBy,

                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception) { throw; }
        }
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            DocumentStatusCRUDViewModel vm = await _context.DocumentStatus.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            DocumentStatusCRUDViewModel vm = new DocumentStatusCRUDViewModel();
            if (id > 0) vm = await _context.DocumentStatus.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(DocumentStatusCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        DocumentStatus _DocumentStatus = new DocumentStatus();
                        if (vm.Id > 0)
                        {
                            _DocumentStatus = await _context.DocumentStatus.FindAsync(vm.Id);

                            vm.CreatedDate = _DocumentStatus.CreatedDate;
                            vm.CreatedBy = _DocumentStatus.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_DocumentStatus).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Document Status Updated Successfully. ID: " + _DocumentStatus.Id;
                            return new JsonResult(_AlertMessage);
                        }
                        else
                        {
                            _DocumentStatus = vm;
                            _DocumentStatus.CreatedDate = DateTime.Now;
                            _DocumentStatus.ModifiedDate = DateTime.Now;
                            _DocumentStatus.CreatedBy = HttpContext.User.Identity.Name;
                            _DocumentStatus.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_DocumentStatus);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Document Status Created Successfully. ID: " + _DocumentStatus.Id;
                            return new JsonResult(_AlertMessage);
                        }
                    }
                    return new JsonResult("Operation failed.");
                }
                catch (Exception) { throw; }
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _DocumentStatus = await _context.DocumentStatus.FindAsync(id);
                _DocumentStatus.ModifiedDate = DateTime.Now;
                _DocumentStatus.ModifiedBy = HttpContext.User.Identity.Name;
                _DocumentStatus.Cancelled = true;

                _context.Update(_DocumentStatus);
                await _context.SaveChangesAsync();
                return new JsonResult(_DocumentStatus);
            }
            catch (Exception) { throw; }
        }
    }
}
