using DocumentMS.Data;
using DocumentMS.Models;
using DocumentMS.Models.EmailTemplateViewModel;
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
    public class EmailTemplateController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public EmailTemplateController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.EmailTemplate.RoleName)]
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
                    || obj.From.ToLower().Contains(searchValue)
                    || obj.To.ToLower().Contains(searchValue)
                    || obj.CCTo.ToLower().Contains(searchValue)
                    || obj.Subject.ToLower().Contains(searchValue)
                    || obj.Body.ToLower().Contains(searchValue)
                    || obj.InvoiceImg.ToLower().Contains(searchValue)

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception) { throw; }
        }

        private IQueryable<EmailTemplateCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _EmailTemplate in _context.EmailTemplate
                        where _EmailTemplate.Cancelled == false
                        select new EmailTemplateCRUDViewModel
                        {
                            Id = _EmailTemplate.Id,
                            From = _EmailTemplate.From,
                            To = _EmailTemplate.To,
                            CCTo = _EmailTemplate.CCTo,
                            Subject = _EmailTemplate.Subject,
                            Body = _EmailTemplate.Body,
                            InvoiceImg = _EmailTemplate.InvoiceImg,
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception) { throw; }
        }
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            EmailTemplateCRUDViewModel vm = await _context.EmailTemplate.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            EmailTemplateCRUDViewModel vm = new EmailTemplateCRUDViewModel();
            if (id > 0) vm = await _context.EmailTemplate.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(EmailTemplateCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        EmailTemplate _EmailTemplate = new EmailTemplate();
                        if (vm.Id > 0)
                        {
                            _EmailTemplate = await _context.EmailTemplate.FindAsync(vm.Id);

                            vm.CreatedDate = _EmailTemplate.CreatedDate;
                            vm.CreatedBy = _EmailTemplate.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_EmailTemplate).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Email Template Updated Successfully. ID: " + _EmailTemplate.Id;
                            return new JsonResult(_AlertMessage);
                        }
                        else
                        {
                            _EmailTemplate = vm;
                            _EmailTemplate.CreatedDate = DateTime.Now;
                            _EmailTemplate.ModifiedDate = DateTime.Now;
                            _EmailTemplate.CreatedBy = HttpContext.User.Identity.Name;
                            _EmailTemplate.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_EmailTemplate);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Email Template Created Successfully. ID: " + _EmailTemplate.Id;
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
                var _EmailTemplate = await _context.EmailTemplate.FindAsync(id);
                _EmailTemplate.ModifiedDate = DateTime.Now;
                _EmailTemplate.ModifiedBy = HttpContext.User.Identity.Name;
                _EmailTemplate.Cancelled = true;

                _context.Update(_EmailTemplate);
                await _context.SaveChangesAsync();
                return new JsonResult(_EmailTemplate);
            }
            catch (Exception) { throw; }
        }        
    }
}
