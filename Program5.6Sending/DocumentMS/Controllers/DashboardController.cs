using DocumentMS.Data;
using DocumentMS.Models.DashboardViewModel;
using DocumentMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace DocumentMS.Controllers
{
    [Authorize]
    //[Route("[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        DateTime TodayZeroHour = DateTime.Today;
        DateTime Today24Hour = DateTime.Today.AddDays(1).AddTicks(-1);
        public DashboardController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.Dashboard.RoleName)]
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                DashboardSummaryViewModel vm = new();
                var _Document = _context.Document.Where(x => x.Cancelled == false);
                var _DocumentCategories = _context.DocumentCategories.Where(x => x.Cancelled == false);
                var _DocumentStatus = _context.DocumentStatus.Where(x => x.Cancelled == false);
                var _Comment = _context.Comment.Where(x => x.Cancelled == false);

                vm.TotalDocument = _Document.Count();
                vm.TotalDocCategories = _DocumentCategories.Count();
                vm.TotalDocumentStatus = _DocumentStatus.Count();
                vm.TotalComment = _Comment.Count();

                var _UserProfile = _context.UserProfile.ToList();
                vm.TotalUser = _UserProfile.Count();
                vm.TotalActive = _UserProfile.Where(x => x.Cancelled == false).Count();
                vm.TotalInActive = _UserProfile.Where(x => x.Cancelled == true).Count();
                vm.listUserProfile = _UserProfile.Where(x => x.Cancelled == false).OrderByDescending(x => x.CreatedDate).Take(10).ToList();

                return View(vm);
            }
            catch (Exception) { throw; }
        }

        [HttpGet]
        public JsonResult GetDocumentByCategories()
        {
            var result = GetGroupByDocumentList().OrderByDescending(x => x.ItemTotal).Take(10).ToList();
            return new JsonResult(result.ToDictionary(x => x.ItemName, x => x.ItemTotal));
        }

        private List<GroupByViewModel> GetGroupByDocumentList()
        {
            var DocumentGroupBy = _context.Document.Where(x => x.Cancelled == false).GroupBy(p => p.CategoriesId).Select(g => new
            {
                CategoriesId = g.Key,
                CategoriesIdCount = g.Count()
            }).ToList();

            var result = (from _DocumentGroupBy in DocumentGroupBy
                          join _DocumentCategories in _context.DocumentCategories on _DocumentGroupBy.CategoriesId equals _DocumentCategories.Id
                          where _DocumentGroupBy.CategoriesId == _DocumentCategories.Id
                          select new GroupByViewModel
                          {
                              ItemName = _DocumentCategories.Name,
                              ItemTotal = _DocumentGroupBy.CategoriesId,
                          }).ToList();

            return result;
        }
    }
}