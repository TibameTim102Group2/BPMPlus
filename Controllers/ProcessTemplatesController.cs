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
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;



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
        public bool DepartmentMatch(List<CategoryNode> nodes, out string err)
        {
            Dictionary<string, string> depGroup = new Dictionary<string, string>() { };
            foreach(var cNode in nodes)
            {
                if (new List<string>() { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" }.Contains(cNode.UserActivityId))
                {
                    if (depGroup.ContainsKey(cNode.UserActivityId)) 
                    {
                        err = $"審核環節 : {_context.UserActivity.FirstOrDefault(c => c.UserActivityId == cNode.UserActivityId).UserActivityIdDescription}僅能存在一次";
                        return false;
                    }
                    depGroup.Add(cNode.UserActivityId, cNode.DepartmentId);
                }
                    
            }
            if (
                (depGroup.ContainsKey("02") && depGroup["01"] != depGroup["02"]) ||
                (depGroup.ContainsKey("03") && depGroup["01"] != depGroup["03"]) ||
                (depGroup.ContainsKey("04") && depGroup["01"] != depGroup["04"]) ||
                (depGroup.ContainsKey("09") && depGroup["01"] != depGroup["09"]) ||
                (depGroup.ContainsKey("10") && depGroup["01"] != depGroup["10"])
               )
            {
                err = $"需求部門審核環節部門不一致";
                return false;
            }
            if (
                (depGroup.ContainsKey("01") && depGroup["01"] != "Requester") ||
                (depGroup.ContainsKey("02") && depGroup["02"] != "Requester") ||
                (depGroup.ContainsKey("03") && depGroup["03"] != "Requester") ||
                (depGroup.ContainsKey("04") && depGroup["04"] != "Requester") ||
                (depGroup.ContainsKey("09") && depGroup["09"] != "Requester") ||
                (depGroup.ContainsKey("10") && depGroup["10"] != "Requester")
               )
            {
                err = $"需求方申請人送出、需求方驗收、驗收完成環節，均需為送單部門";
                return false;
            }
            if (
                (depGroup.ContainsKey("05") && depGroup["07"] != depGroup["05"]) ||
                (depGroup.ContainsKey("06") && depGroup["07"] != depGroup["06"]) ||
                (depGroup.ContainsKey("08") && depGroup["07"] != depGroup["08"])
               )
            {
                err = $"處理部門審核環節部門不一致";
                return false;
            }
            err = "";
            return true;
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
                new List<string>() {"08"}
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

            foreach (var processUserActivityRegx in processUserActivityRegxList)
            { 
                if(processUserActivityRegx.Type == MatchType.ExactlyOne)
                {
                    if(nodeIndex >= nodes.Count)
                    {
                        var acts = _context.UserActivity
                            .Where(d => processUserActivityRegx.CandidateUserActivities.Contains(d.UserActivityId))
                            .Select(d => d.UserActivityIdDescription)
                            .ToList();
                        err = $"您所新增需求類別欠缺必要流程節點, 請往後加上以下任意一種功能 : \n{String.Join("\n", acts)}";
                        return false;
                    }
                    if (!processUserActivityRegx.CandidateUserActivities.Contains(nodes[nodeIndex].UserActivityId))
                    {
                        var act = _context.UserActivity.FirstOrDefault(x => x.UserActivityId == nodes[nodeIndex].UserActivityId);
                        err = $"您所新增的需求類別之功能 {act.UserActivityIdDescription} 不應處於目前位置";
                        return false;
                    }
                    nodeIndex++;
                }
                if (processUserActivityRegx.Type == MatchType.NoneOrMany)
                {
                    if (nodeIndex >= nodes.Count)
                    {
                        continue;
                    }
                    while (processUserActivityRegx.CandidateUserActivities.Contains(nodes[nodeIndex].UserActivityId))
                    {
                        nodeIndex++;
                    }
                }
            }
            if(nodeIndex < nodes.Count)
            {
                var act = _context.UserActivity.FirstOrDefault(x => x.UserActivityId == nodes[nodeIndex].UserActivityId);
                err = $"您所新增的需求類別之功能 {act.UserActivityIdDescription} 以及其後節點都是多餘的";
                return false;
            }
            err = "";
            return true;
        }
		// GET: ProcessTemplates
		[Authorize]
		public async Task<IActionResult> Index()
        {
            User user = await GetAuthorizedUser();
            //functionId:  01 -> 需求方申請人送出

            if (!user.PermittedTo("13"))
            {
                ViewBag.NotPermittedToCreateForm = "您的權限無法新建需求類別";
                return View("~/Views/Home/Index.cshtml");
            }
            GetTemplateOfNodeTemplates DefaultNodes = new GetTemplateOfNodeTemplates(
                new List<CategoryNode>()
                {
                    new CategoryNode("01", "Requester"),
                    new CategoryNode("07", "D002"),
                    new CategoryNode("08", "D002"),
                    new CategoryNode("09", "Requester"),
                    new CategoryNode("10", "Requester")
                }
            );
            Dictionary<string, string> UserActivityDict = new Dictionary<string, string>() { };
            foreach (var UserActivity in (await _context.UserActivity
                .Where(u => (new List<string>() { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10"}).Contains(u.UserActivityId))
                .ToListAsync()))
            { 
                UserActivityDict[UserActivity.UserActivityId] = UserActivity.UserActivityIdDescription;
            }
            Dictionary<string, string> DepartmentDict = new Dictionary<string, string>() { };
            foreach (var Department in (await _context.Department.ToListAsync()))
            {
                DepartmentDict[Department.DepartmentId] = Department.DepartmentName;
            }
            DepartmentDict["Requester"] = "送單部門";
            //var applicationDbContext = _context.ProcessTemplate.Include(p => p.Category).Include(p => p.UserActivity);
            return View(
                new GetDataForCategoryCreate(
                    DefaultNodes,
                    UserActivityDict,
                    DepartmentDict
                )
            );
        }

		// GET: ProcessTemplates/Details/5
		[Authorize]
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

            if (!DepartmentMatch(model.Nodes, out err))
            {
                return Json(new { errorCode = 400, message = err });
            }

            Category cat = new Category();
            cat.CategoryDescription = model.CategoryName;
            cat.CategoryId = (await GetCategoryIdListAsync(1))[0];
            cat.CreatedTime = DateTime.UtcNow;
            cat.UpdatedTime = DateTime.UtcNow;

            await _context.Category.AddAsync(cat);

            List<string> pTidList = await GetProcessTemplateIdListAsync(model.Nodes.Count);
            int pTidListIndex = 0;
            foreach(var node in model.Nodes)
            {
                ProcessTemplate pt = new ProcessTemplate();
                pt.ProcessTemplateId = pTidList[pTidListIndex];
                pt.DepartmentId = node.DepartmentId;
                pt.UserActivityId = node.UserActivityId;
                pt.CategoryId = cat.CategoryId;
                pt.CreatedTime = DateTime.UtcNow;
                pt.UpdatedTime = DateTime.UtcNow;
                pTidListIndex++;
                await _context.ProcessTemplate.AddAsync(pt);
            }
            await _context.SaveChangesAsync();
            return Json(new { errorCode = 200, message = $"新增需求類別成功!"});
        }

       

        // POST: ProcessTemplates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
		[Authorize]
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

		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
