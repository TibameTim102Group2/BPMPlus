using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BPMPlus.Controllers
{
    public class BaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<List<string>> GetCreateFormIdListAsync(int count)
        {
            List<string> retList = new List<string>();
            var LastForm = await _context.Form.OrderBy(f => f.FormId).LastAsync();
            if (LastForm == null)
            {
                retList.Add("F00001");
                return retList;
            }
                
            string id = LastForm.FormId;
            id = id[1..];//拿掉第一個 F
            int idNum = Convert.ToInt32(id);
            idNum++;
            for (int i = 0; i < count; i++, idNum++ )
            {
                retList.Add("F" + idNum.ToString().PadLeft(5, '0'));
            }

            return retList;
        }

        [Authorize]
        public async Task<List<string>> GetCreateFormRecordIdListAsync(int count)
        {
            List<string> retList = new List<string>();
            var LastFormRecord = await _context.FormRecord.OrderBy(f => f.ProcessingRecordId).LastAsync();
            if (LastFormRecord == null)
            {
                retList.Add("PR00000001");
                return retList;
            }

            string id = LastFormRecord.ProcessingRecordId;
            id = id[2..];//拿掉第一個 F
            int idNum = Convert.ToInt32(id);
            idNum++;
            for (int i = 0; i < count; i++, idNum++)
            {
                retList.Add("PR" + idNum.ToString().PadLeft(8, '0'));
            }

            return retList;
        }

        [Authorize]
        public async Task<List<string>> GetProcessNodeIdListAsync(int count)
        {
            List<string> retList = new List<string>();
            var LastNode = await _context.ProcessNodes.OrderBy(f => f.ProcessNodeId).LastAsync();
            if (LastNode == null)
            {
                retList.Add("PN000001");
                return retList;
            }

            string id = LastNode.ProcessNodeId;
            id = id[2..];//拿掉第一個 F
            int idNum = Convert.ToInt32(id);
            idNum++;
            for (int i = 0; i < count; i++, idNum++)
            {
                retList.Add("PN" + idNum.ToString().PadLeft(6, '0'));
            }

            return retList;
            
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
            
            if (user == null)
            {
                throw new Exception("Server error, User is null");
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
                    var filePath = Path.Combine(folderPath, file.FileName);

                    // 保存檔案
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
        }
    }
}
