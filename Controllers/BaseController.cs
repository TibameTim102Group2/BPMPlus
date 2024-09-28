using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BPMPlus.Controllers
{
    public class ThreadSafeList
    {
        private readonly List<string> _list = new List<string>();
        private readonly object _lockObject = new object(); // 鎖定對象

        // 添加項目到列表
        public void Add(string item)
        {
            lock (_lockObject)
            {
                _list.Add(item);
            }
        }

        // 移除列表中的項目
        public bool Remove(string item)
        {
            lock (_lockObject)
            {
                return _list.Remove(item);
            }
        }

        // 獲取列表的所有項目
        public List<string> GetAllItems()
        {
            lock (_lockObject)
            {
                return new List<string>(_list); // 返回列表的副本
            }
        }
    }

    public class BaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ThreadSafeList _safeCreateFormIdList = new ThreadSafeList();
        public ThreadSafeList _safeCreateFormRecordIdList = new ThreadSafeList();
        public ThreadSafeList _safeProcessNodeList = new ThreadSafeList();
        public ThreadSafeList _safeCreateProjectIdList = new ThreadSafeList();
        public ThreadSafeList _safeCreateCategoryIdList = new ThreadSafeList();
        public ThreadSafeList _safeCreateUserIdList = new ThreadSafeList();
        public ThreadSafeList _safeCreateProcessTemplateIdList = new ThreadSafeList();
        public ThreadSafeList _safeCreateMeetingIdList = new ThreadSafeList();


        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<List<string>> GetCreateFormIdListAsync(int count)
        {
            
            Form? LastForm = await _context.Form.OrderBy(f => f.FormId).LastOrDefaultAsync();
                
            string id = LastForm == null?"F0":LastForm.FormId;
            id = id[1..];//拿掉第一個 F
            int idNum = Convert.ToInt32(id);
            idNum++;
            for (int i = 0; i < count; i++, idNum++ )
            {
                _safeCreateFormIdList.Add("F" + idNum.ToString().PadLeft(5, '0'));
            }

            return _safeCreateFormIdList.GetAllItems();
        }

        [Authorize]
        public async Task<List<string>> GetCreateFormRecordIdListAsync(int count)
        {
            
            var LastFormRecord = await _context.FormRecord.OrderBy(f => f.ProcessingRecordId).LastOrDefaultAsync();
            string id = LastFormRecord == null ? "PR0" : LastFormRecord.ProcessingRecordId;
            id = id[2..];//拿掉前二個 PR
            int idNum = Convert.ToInt32(id);
            idNum++;
            for (int i = 0; i < count; i++, idNum++)
            {
                _safeProcessNodeList.Add("PR" + idNum.ToString().PadLeft(8, '0'));
            }

            return _safeProcessNodeList.GetAllItems();
        }
        [Authorize]
        public async Task<List<string>> GetProcessTemplateIdListAsync(int count)
        {
            var NodeList = await _context.ProcessTemplate.ToListAsync();
            var maxId = NodeList.Max(n => Convert.ToInt32(n.ProcessTemplateId[2..]));
            var LastNode = NodeList.FirstOrDefault(n => Convert.ToInt32(n.ProcessTemplateId[2..]) == maxId);
            string id = NodeList == null ? "PT0" : LastNode.ProcessTemplateId;
            id = id[2..];//
            int idNum = Convert.ToInt32(id);
            idNum++;
            for (int i = 0; i < count; i++, idNum++)
            {
                _safeCreateProcessTemplateIdList.Add("PT" + idNum.ToString().PadLeft(2, '0'));
            }

            return _safeCreateProcessTemplateIdList.GetAllItems();

        }
        [Authorize]
        public async Task<List<string>> GetCategoryIdListAsync(int count)
        {
            var CategoryList = await _context.Category.ToListAsync();
            var maxId = CategoryList.Max(n => Convert.ToInt32(n.CategoryId[1..]));
            var LastCategory = CategoryList.FirstOrDefault(n => Convert.ToInt32(n.CategoryId[1..]) == maxId);
            string id = CategoryList == null ? "C0" : LastCategory.CategoryId;
            id = id[1..];// 拿掉第一個 C
            int idNum = Convert.ToInt32(id);
            idNum++;
            for (int i = 0; i < count; i++, idNum++)
            {
                _safeCreateCategoryIdList.Add("C" + idNum.ToString());
            }

            return _safeCreateCategoryIdList.GetAllItems();

        }
        [Authorize]
        public async Task<List<string>> GetProcessNodeIdListAsync(int count)
        {
            
            var LastNode = await _context.ProcessNodes.OrderBy(f => f.ProcessNodeId).LastOrDefaultAsync();
            string id = LastNode == null ? "PN0" : LastNode.ProcessNodeId;
            id = id[2..];//拿掉前二個 PN
            int idNum = Convert.ToInt32(id);
            idNum++;
            for (int i = 0; i < count; i++, idNum++)
            {
                _safeCreateFormRecordIdList.Add("PN" + idNum.ToString().PadLeft(6, '0'));
            }

            return _safeCreateFormRecordIdList.GetAllItems();
            
        }
        [Authorize]
        public async Task<User> GetAuthorizedUser()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;

            var user = await _context.User
                .Include(u => u.PermissionGroups)
                .ThenInclude(pg => pg.UserActivities)
                .Include(u => u.Projects)         // 加載 Projects
                .Include(u => u.Meetings)
                .FirstOrDefaultAsync(m => m.UserId == userId && m.UserIsActive == true);
            var browerSessionToken = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "SessionToken")?.Value;

			if (user == null || user.SessionToken != browerSessionToken)
            {
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			}
            return user;
        }
		[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task UploadFiles(List<IFormFile> Files, string DirectoryName)
        {
            // 指定專案資料夾名稱
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", DirectoryName);

            // 檢查資料夾是否存在，如果不存在則創建一個新資料夾
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 檢查是否有上傳的檔案
            if (Files != null && Files.Count > 0)
            {
                foreach (var file in Files)
                {
                    // 檔案存放的完整路徑
                    var filePath = Path.Combine(folderPath, DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd-HHmmss-") + file.FileName);

                    // 保存檔案
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
        }

        [Authorize]
        public async Task<List<string>> CreateProjectIdListAsync(int count)
        {
            var LastNode = await _context.Project.OrderBy(f => f.ProjectId).LastOrDefaultAsync();
            string id = LastNode == null ? "P0" : LastNode.ProjectId;
            id = id[1..];//拿掉第兩個 P0
            int idNum = Convert.ToInt32(id);
            idNum++;
            for (int i = 0; i < count; i++, idNum++)
            {
                _safeCreateProjectIdList.Add("P" + idNum.ToString().PadLeft(3, '0'));
            }

            return _safeCreateProjectIdList.GetAllItems(); 
        }
		[Authorize]
		public async Task<List<string>> CreateUserIdListAsync(int count)
		{
			var LastNode = await _context.User.OrderBy(f => f.UserId).LastOrDefaultAsync();
			string id = LastNode == null ? "A0" : LastNode.UserId;
			id = id[1..];//拿掉第一個 A
			int idNum = Convert.ToInt32(id);
			idNum++;
			for (int i = 0; i < count; i++, idNum++)
			{
                _safeCreateUserIdList.Add("A" + idNum.ToString().PadLeft(3, '0'));
			}

			return _safeCreateUserIdList.GetAllItems();
		}

		public async Task<List<string>> CreateMeetingIdListAsync(int count)
		{
			var LastNode = await _context.Meeting.OrderBy(f => f.MeetingId).LastOrDefaultAsync();
			string id = LastNode == null ? "M0" : LastNode.MeetingId;
			id = id[1..];//拿掉第一個M
			int idNum = Convert.ToInt32(id);
			idNum++;
			for (int i = 0; i < count; i++, idNum++)
			{
                _safeCreateMeetingIdList.Add("M" + idNum.ToString().PadLeft(3, '0'));
			}

			return _safeCreateMeetingIdList.GetAllItems();
		}

	}
}
