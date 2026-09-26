using DocumentMS.Data;
using DocumentMS.Models;
using DocumentMS.Models.CommentViewModel;
using DocumentMS.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using Services.SignalRService;
using DocumentMS.Models.DocumentHistoryViewModel;

namespace DocumentMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IHubContext<SignalServer> _signalServer;

        public CommentController(ApplicationDbContext context, ICommon iCommon, IHubContext<SignalServer> signalServer)
        {
            _context = context;
            _iCommon = iCommon;
            _signalServer = signalServer;
        }

        [Authorize(Roles = Pages.MainMenu.Comment.RoleName)]
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
                    || obj.DocumentId.ToString().ToLower().Contains(searchValue)
                    || obj.Message.ToLower().Contains(searchValue)
                    || obj.IsDeleted.ToString().ToLower().Contains(searchValue)
                    || obj.IsAdmin.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
                    || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception) { throw; }
        }

        private IQueryable<CommentCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _Comment in _context.Comment
                        where _Comment.Cancelled == false
                        select new CommentCRUDViewModel
                        {
                            Id = _Comment.Id,
                            DocumentId = _Comment.DocumentId,
                            Message = _Comment.Message,
                            IsDeleted = _Comment.IsDeleted,
                            IsAdmin = _Comment.IsAdmin,
                            CreatedDate = _Comment.CreatedDate
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception) { throw; }
        }
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            CommentCRUDViewModel vm = await _context.Comment.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            CommentCRUDViewModel vm = new CommentCRUDViewModel();
            if (id > 0) vm = await _context.Comment.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(CommentCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Comment _Comment = new Comment();
                        if (vm.Id > 0)
                        {
                            _Comment = await _context.Comment.FindAsync(vm.Id);

                            vm.CreatedDate = _Comment.CreatedDate;
                            vm.CreatedBy = _Comment.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_Comment).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Comment Updated Successfully. ID: " + _Comment.Id;
                            return new JsonResult(_AlertMessage);
                        }
                        else
                        {
                            _Comment = vm;
                            _Comment.CreatedDate = DateTime.Now;
                            _Comment.ModifiedDate = DateTime.Now;
                            _Comment.CreatedBy = HttpContext.User.Identity.Name;
                            _Comment.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_Comment);
                            await _context.SaveChangesAsync();

                            await AddDocumentCommentHis(_Comment.DocumentId, "Document new commnet added. Comment Id: " + _Comment.Id);
                            await _signalServer.Clients.All.SendAsync("refreshComment");
                            return new JsonResult(_Comment);
                        }
                    }
                    return new JsonResult("Operation failed.");
                }
                catch (Exception) { throw; }
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<JsonResult> AddCommentList(List<Comment> listComment)
        {
            try
            {
                Comment _Comment = new();
                if (listComment != null)
                {
                    foreach (var item in listComment)
                    {
                        _Comment = item;
                        _Comment.CreatedDate = DateTime.Now;
                        _Comment.ModifiedDate = DateTime.Now;
                        _Comment.CreatedBy = HttpContext.User.Identity.Name;
                        _Comment.ModifiedBy = HttpContext.User.Identity.Name;
                        _context.Add(_Comment);
                        await _context.SaveChangesAsync();

                        await AddDocumentCommentHis(_Comment.DocumentId, "Document new commnet added. Comment Id: " + _Comment.Id);
                    }
                }
                return new JsonResult(_Comment);
            }
            catch (Exception) { throw; }
        }

        [HttpPost]
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _Comment = await _context.Comment.FindAsync(id);
                _Comment.ModifiedDate = DateTime.Now;
                _Comment.ModifiedBy = HttpContext.User.Identity.Name;
                _Comment.Cancelled = true;

                _context.Update(_Comment);
                await _context.SaveChangesAsync();
                await AddDocumentCommentHis(_Comment.DocumentId, "Document commnet deleted. Comment Id: " + id);

                await _signalServer.Clients.All.SendAsync("refreshComment");
                return new JsonResult(_Comment);
            }
            catch (Exception) { throw; }
        }

        private async Task AddDocumentCommentHis(Int64 _DocumentId, string _Action)
        {
            var _Document = await _context.Document.FindAsync(_DocumentId);
            AddDocumentHistoryViewModel _AddDocumentHistoryViewModel = new()
            {
                DocumentId = _DocumentId,
                AssignEmployeeId = _Document.AssignEmployeeId,
                Action = _Action,
                UserName = HttpContext.User.Identity.Name,
            };
            var result = await _iCommon.AddDocumentHistory(_AddDocumentHistoryViewModel);
        }
    }
}
