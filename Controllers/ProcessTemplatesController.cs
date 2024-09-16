using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BPMPlus.Data;
using BPMPlus.Models;
using BPMPlus.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;



namespace BPMPlus.Controllers
{
    public enum MatchType
    {
        ExactlyOne,
        NoneOrMany
    }
    public class ProcessUserActivityRegx 
    {
        public MatchType Type { get; set; }
        public List<string> CandidateUserActivities { get; set; }
        public ProcessUserActivityRegx(MatchType t, List<string> c)
        {
            this.Type = t;
            this.CandidateUserActivities = c;
        }
    }
    public class ProcessTemplatesController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ProcessTemplatesController(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
        public bool ProcessIsValid(List<CategoryNode> nodes, out string err)
        {
            if(nodes.Count == 0)
            {
                err = $"工單無流程";
                return false;
            }
            List<ProcessUserActivityRegx> processUserActivityRegxList = new List<ProcessUserActivityRegx>();
            processUserActivityRegxList.Add(new ProcessUserActivityRegx(
                MatchType.ExactlyOne,
                new List<string>() { "01"}
            ));
            processUserActivityRegxList.Add(new ProcessUserActivityRegx(
                MatchType.NoneOrMany,
                new List<string>() { "02", "03", "04", "05", "06" }
            ));
            processUserActivityRegxList.Add(new ProcessUserActivityRegx(
                MatchType.ExactlyOne,
                new List<string>() { "07"}
            ));
            processUserActivityRegxList.Add(new ProcessUserActivityRegx(
                MatchType.ExactlyOne,
                new List<string>() {"08" }
            ));
            processUserActivityRegxList.Add(new ProcessUserActivityRegx(
                MatchType.ExactlyOne,
                new List<string>() { "09" }
            ));
            processUserActivityRegxList.Add(new ProcessUserActivityRegx(
                MatchType.ExactlyOne,
                new List<string>() { "10" }
            ));

            int nodeIndex = 0;
            int regxIndex = 0;

            while(true)
            {
                if (processUserActivityRegxList[regxIndex].Type == MatchType.ExactlyOne)
                {
                    if (!processUserActivityRegxList[regxIndex].CandidateUserActivities.Contains(nodes[nodeIndex].UserActivityId))
                    {
                        var act = _context.UserActivity.FirstOrDefault(x => x.UserActivityId == nodes[nodeIndex].UserActivityId);
                        err = $"您所新增的功能 {act.UserActivityIdDescription} 不應處於目前位置";
                        return false;
                    }
                    nodeIndex++;
                    regxIndex++;
                }
                if (processUserActivityRegxList[regxIndex].Type == MatchType.NoneOrMany)
                {
                    
                }
            }
            err = "";
            return true;
        }
        // GET: ProcessTemplates
        public async Task<IActionResult> Index()
        {
            User user = await GetAuthorizedUser();
            //functionId:  01 -> 需求方申請人送出

            if (!user.PermittedTo("13"))
            {
                ViewBag.NotPermittedToCreateForm = "您的權限無法新建需求類別";
                return View("~/Views/Home/Index.cshtml");
            }
        //var applicationDbContext = _context.ProcessTemplate.Include(p => p.Category).Include(p => p.UserActivity);
            return View();
        }

        // GET: ProcessTemplates/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplate = await _context.ProcessTemplate
                .Include(p => p.Category)
                .Include(p => p.UserActivity)
                .FirstOrDefaultAsync(m => m.ProcessTemplateId == id);
            if (processTemplate == null)
            {
                return NotFound();
            }

            return View(processTemplate);
        }

        // GET: ProcessTemplates/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateCategory([FromBody] CreateCategory model)
        {
            User user = await GetAuthorizedUser();
            
            //functionId:  13 -> 新增需求類別
            List<Category> processValidatorList = new List<Category>();

            if (!user.PermittedTo("13"))
            {
                return Json(new { errorCode = 400, message = $"您無權新增工單類別" });
            }
            if(model.CategoryName == null || model.CategoryName == "")
            {
                return Json(new { errorCode = 400, message = $"工單名稱不可為空" });
            }
            List<string> ExistingCategoryNames = await _context.Category.Select(c => c.CategoryDescription).Where(c => c == model.CategoryName).ToListAsync();
            if(ExistingCategoryNames.Count()!=0)
            {
                return Json(new { errorCode = 400, message = $"工單類別已存在" });
            }
            string err;
            if(!ProcessIsValid(model.Nodes, out err))
            {
                return Json(new { errorCode = 400, message = err });
            }

            return Json(new { errorCode = 200, message = $"新增需求類別成功!"});
        }

        // POST: ProcessTemplates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProcessTemplateId,UserActivityId,DepartmentId,CategoryId,CreatedTime,UpdatedTime")] ProcessTemplate processTemplate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(processTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", processTemplate.CategoryId);
            ViewData["UserActivityId"] = new SelectList(_context.UserActivity, "UserActivityId", "UserActivityId", processTemplate.UserActivityId);
            return View(processTemplate);
        }

        // GET: ProcessTemplates/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplate = await _context.ProcessTemplate.FindAsync(id);
            if (processTemplate == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", processTemplate.CategoryId);
            ViewData["UserActivityId"] = new SelectList(_context.UserActivity, "UserActivityId", "UserActivityId", processTemplate.UserActivityId);
            return View(processTemplate);
        }

        // POST: ProcessTemplates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProcessTemplateId,UserActivityId,DepartmentId,CategoryId,CreatedTime,UpdatedTime")] ProcessTemplate processTemplate)
        {
            if (id != processTemplate.ProcessTemplateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessTemplateExists(processTemplate.ProcessTemplateId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", processTemplate.CategoryId);
            ViewData["UserActivityId"] = new SelectList(_context.UserActivity, "UserActivityId", "UserActivityId", processTemplate.UserActivityId);
            return View(processTemplate);
        }

        // GET: ProcessTemplates/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processTemplate = await _context.ProcessTemplate
                .Include(p => p.Category)
                .Include(p => p.UserActivity)
                .FirstOrDefaultAsync(m => m.ProcessTemplateId == id);
            if (processTemplate == null)
            {
                return NotFound();
            }

            return View(processTemplate);
        }

        // POST: ProcessTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var processTemplate = await _context.ProcessTemplate.FindAsync(id);
            if (processTemplate != null)
            {
                _context.ProcessTemplate.Remove(processTemplate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessTemplateExists(string id)
        {
            return _context.ProcessTemplate.Any(e => e.ProcessTemplateId == id);
        }
    }
}
