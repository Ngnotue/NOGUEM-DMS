using DocumentMS.Data;
using DocumentMS.Models;
using DocumentMS.Models.DocumentCategoriesViewModel;
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
    public class DocumentCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public DocumentCategoriesController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.DocumentCategories.RoleName)]
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
                    || obj.ModifiedBy.ToLower().Contains(searchValue)

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception) { throw; }
        }

        private IQueryable<DocumentCategoriesCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _DocumentCategories in _context.DocumentCategories
                        where _DocumentCategories.Cancelled == false
                        select new DocumentCategoriesCRUDViewModel
                        {
                            Id = _DocumentCategories.Id,
                            Name = _DocumentCategories.Name,
                            Description = _DocumentCategories.Description,
                            CreatedDate = _DocumentCategories.CreatedDate,
                            ModifiedDate = _DocumentCategories.ModifiedDate,
                            CreatedBy = _DocumentCategories.CreatedBy,
                            ModifiedBy = _DocumentCategories.ModifiedBy,

                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception) { throw; }
        }
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            DocumentCategoriesCRUDViewModel vm = await _context.DocumentCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            DocumentCategoriesCRUDViewModel vm = new DocumentCategoriesCRUDViewModel();
            if (id > 0) vm = await _context.DocumentCategories.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(DocumentCategoriesCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        DocumentCategories _DocumentCategories = new DocumentCategories();
                        if (vm.Id > 0)
                        {
                            _DocumentCategories = await _context.DocumentCategories.FindAsync(vm.Id);

                            vm.CreatedDate = _DocumentCategories.CreatedDate;
                            vm.CreatedBy = _DocumentCategories.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_DocumentCategories).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Document Categories Updated Successfully. ID: " + _DocumentCategories.Id;
                            return new JsonResult(_AlertMessage);
                        }
                        else
                        {
                            _DocumentCategories = vm;
                            _DocumentCategories.CreatedDate = DateTime.Now;
                            _DocumentCategories.ModifiedDate = DateTime.Now;
                            _DocumentCategories.CreatedBy = HttpContext.User.Identity.Name;
                            _DocumentCategories.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_DocumentCategories);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Document Categories Created Successfully. ID: " + _DocumentCategories.Id;
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
                var _DocumentCategories = await _context.DocumentCategories.FindAsync(id);
                _DocumentCategories.ModifiedDate = DateTime.Now;
                _DocumentCategories.ModifiedBy = HttpContext.User.Identity.Name;
                _DocumentCategories.Cancelled = true;

                _context.Update(_DocumentCategories);
                await _context.SaveChangesAsync();
                return new JsonResult(_DocumentCategories);
            }
            catch (Exception) { throw; }
        }       
    }
}
