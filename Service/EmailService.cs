using System.Net.Mail;
using System.Net;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Web;
using BPMPlus.ViewModels.Login;
using Microsoft.EntityFrameworkCore;
using BPMPlus.Data;
using BPMPlus.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol.Plugins;
using System.Data;
using Azure.Core;

namespace BPMPlus.Service
{
    public class EmailService(IConfiguration configuration, ApplicationDbContext context) : AesAndTimestampService(configuration)
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ApplicationDbContext _context = context;

        public void SendEmail(string Email,string UserName)
        {
            var resetPasswordUrl = GetUrl(Email);

            var text = "您執行了忘記密碼的功能，請點擊以下連結重新設定新密碼!!<br>此連結有效時間為15分鐘，逾期請重新申請!!";
            var btnText = "忘記密碼";

            // 使用 Google Mail Server 發信
            string GoogleID = "bpmplus102@gmail.com"; //Google 發信帳號
            string TempPwd = "ygzz mxhm zmao weyb"; //應用程式密碼
            string ReceiveMail = Email; //接收信箱           

            string SmtpServer = "smtp.gmail.com";
            int SmtpPort = 587;
            MailMessage mms = new MailMessage();
            mms.From = new MailAddress(GoogleID);
            mms.Subject = "BPMPlus 密碼重設";
            mms.Body = "<!doctype html>\r\n<html lang=\"en\">\r\n  <head>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\r\n    <title>Simple Transactional Email</title>\r\n    <style media=\"all\" type=\"text/css\">\r\n@media all {\r\n  .btn-primary table td:hover {\r\n    background-color: #ec0867 !important;\r\n  }\r\n\r\n  .btn-primary a:hover {\r\n    background-color: #ec0867 !important;\r\n    border-color: #ec0867 !important;\r\n  }\r\n}\r\n@media only screen and (max-width: 640px) {\r\n  .main p,\r\n.main td,\r\n.main span {\r\n    font-size: 16px !important;\r\n  }\r\n\r\n  .wrapper {\r\n    padding: 8px !important;\r\n  }\r\n\r\n  .content {\r\n    padding: 0 !important;\r\n  }\r\n\r\n  .container {\r\n    padding: 0 !important;\r\n    padding-top: 8px !important;\r\n    width: 100% !important;\r\n  }\r\n\r\n  .main {\r\n    border-left-width: 0 !important;\r\n    border-radius: 0 !important;\r\n    border-right-width: 0 !important;\r\n  }\r\n\r\n  .btn table {\r\n    max-width: 100% !important;\r\n    width: 100% !important;\r\n  }\r\n\r\n  .btn a {\r\n    font-size: 16px !important;\r\n    max-width: 100% !important;\r\n    width: 100% !important;\r\n  }\r\n}\r\n@media all {\r\n  .ExternalClass {\r\n    width: 100%;\r\n  }\r\n\r\n  .ExternalClass,\r\n.ExternalClass p,\r\n.ExternalClass span,\r\n.ExternalClass font,\r\n.ExternalClass td,\r\n.ExternalClass div {\r\n    line-height: 100%;\r\n  }\r\n\r\n  .apple-link a {\r\n    color: inherit !important;\r\n    font-family: inherit !important;\r\n    font-size: inherit !important;\r\n    font-weight: inherit !important;\r\n    line-height: inherit !important;\r\n    text-decoration: none !important;\r\n  }\r\n\r\n  #MessageViewBody a {\r\n    color: inherit;\r\n    text-decoration: none;\r\n    font-size: inherit;\r\n    font-family: inherit;\r\n    font-weight: inherit;\r\n    line-height: inherit;\r\n  }\r\n}\r\n</style>\r\n  </head>\r\n  <body style=\"font-family: Helvetica, sans-serif; -webkit-font-smoothing: antialiased; font-size: 16px; line-height: 1.3; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; background-color: #f4f5f6; margin: 0; padding: 0;\">\r\n    <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"body\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #f4f5f6; width: 100%;\" width=\"100%\" bgcolor=\"#f4f5f6\">\r\n      <tr>\r\n        <td style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top;\" valign=\"top\">&nbsp;</td>\r\n        <td class=\"container\" style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; max-width: 600px; padding: 0; padding-top: 24px; width: 600px; margin: 0 auto;\" width=\"600\" valign=\"top\">\r\n          <div class=\"content\" style=\"box-sizing: border-box; display: block; margin: 0 auto; max-width: 600px; padding: 0;\">\r\n\r\n            <!-- START CENTERED WHITE CONTAINER -->\r\n            <span class=\"preheader\" style=\"color: transparent; display: none; height: 0; max-height: 0; max-width: 0; opacity: 0; overflow: hidden; mso-hide: all; visibility: hidden; width: 0;\">This is preheader text. Some clients will show this text as a preview.</span>\r\n            <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"main\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background: #ffffff; border: 1px solid #eaebed; border-radius: 16px; width: 100%;\" width=\"100%\">\r\n\r\n              <!-- START MAIN CONTENT AREA -->\r\n              <tr>\r\n                <td class=\"wrapper\" style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; box-sizing: border-box; padding: 24px;\" valign=\"top\">\r\n                  <p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">Hi  " + UserName + "</p>\r\n                  <p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">"+text+"</p>\r\n                  <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"btn btn-primary\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; box-sizing: border-box; width: 100%; min-width: 100%;\" width=\"100%\">\r\n                    <tbody>\r\n                      <tr>\r\n                        <td align=\"left\" style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; padding-bottom: 16px;\" valign=\"top\">\r\n                          <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: auto;\">\r\n                            <tbody>\r\n                              <tr>\r\n                                <td style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; border-radius: 4px; text-align: center; background-color: #0867ec;\" valign=\"top\" align=\"center\" bgcolor=\"#0867ec\"> <a href=\""+ resetPasswordUrl + "\" target=\"_blank\" style=\"border: solid 2px #0867ec; border-radius: 4px; box-sizing: border-box; cursor: pointer; display: inline-block; font-size: 16px; font-weight: bold; margin: 0; padding: 12px 24px; text-decoration: none; text-transform: capitalize; background-color: #0867ec; border-color: #0867ec; color: #ffffff;\">"+btnText+"</a> </td>\r\n                              </tr>\r\n                            </tbody>\r\n                          </table>\r\n                        </td>\r\n                      </tr>\r\n                    </tbody>\r\n                  </table>\r\n                  <p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">Have a nice day! :)</p><p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">本信件為系統自動發出，請勿回信，謝謝!!</p>\r\n                </td>\r\n              </tr>\r\n\r\n              <!-- END MAIN CONTENT AREA -->\r\n              </table>\r\n\r\n            <!-- START FOOTER -->\r\n            <div class=\"footer\" style=\"clear: both; padding-top: 24px; text-align: center; width: 100%;\">\r\n              <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%;\" width=\"100%\">\r\n                <tr>\r\n                  <td class=\"content-block\" style=\"font-family: Helvetica, sans-serif; vertical-align: top; color: #9a9ea6; font-size: 16px; text-align: center;\" valign=\"top\" align=\"center\">\r\n                    <span class=\"apple-link\" style=\"color: #9a9ea6; font-size: 16px; text-align: center;\">BPMPlus</span>         </table>\r\n            </div>\r\n\r\n            <br><!-- END FOOTER -->\r\n            \r\n<!-- END CENTERED WHITE CONTAINER --></div>\r\n        </td>\r\n        <td style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top;\" valign=\"top\">&nbsp;</td></tr></table></body></html>";
            mms.IsBodyHtml = true;
            mms.To.Add(new MailAddress(ReceiveMail));

            using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(GoogleID, TempPwd);//寄信帳密 
                client.Send(mms); //寄出信件
            }

        }

        public void SendFormReviewEmail(User recieveEmp , string formId)
        {

            // 使用 Google Mail Server 發信
            string GoogleID = "bpmplus102@gmail.com"; //Google 發信帳號
            string TempPwd = "ygzz mxhm zmao weyb"; //應用程式密碼
            string ReceiveMail = recieveEmp.Email; //接收信箱

            // 抓目前要寄信的人
            var recieveFormRecordEmail = _context.FormRecord
            .Where(fr => fr.FormId == formId).OrderByDescending(d => d.ProcessingRecordId).Select(fr => new
            {
                fr.UserActivity.UserActivityIdDescription,
                fr.FormId,
                fr.ResultId,
            }).FirstOrDefault();

            // 工單流程節點是不是第一筆
            bool isFirstProcessNodeId = _context.ProcessNodes
                .Where(pn => pn.FormId == formId)
                .OrderBy(pn => pn.ProcessNodeId)  // 取得第一筆 ProcessNode
                .Select(pn => pn.ProcessNodeId)   // 取得第一筆的 ProcessNodeId
                .FirstOrDefault() == _context.Form
                .Where(f => f.FormId == formId)
                .Select(f => f.ProcessNodeId)
                .FirstOrDefault();  // 比對是否與 Form 的 ProcessNodeId 一樣

            var resultDescription = _context.Result.Where(r=>r.ResultId==recieveFormRecordEmail.ResultId).Select(c=>c.ResultDescription).FirstOrDefault();

			if (recieveFormRecordEmail.ResultId == "RS4")
			{
				// 寄給被退回的人
				if (isFirstProcessNodeId)
				{
					var returnUrl = HttpUtility.UrlEncode($"/FormDetails/Index?id={formId}");
					var Url = $"https://localhost:7129/Login/Index?returnUrl={returnUrl}";
					var btnText = "工單細節";
					var reviewText = "您好, 提醒您有退件的工單需處理 ! 請至工單細節點選修改按鈕<br>目前狀態為**" + resultDescription + "**<br>工單流程進度為" + recieveFormRecordEmail.UserActivityIdDescription.ToString() + "<br>以此溫韾提醒~ 感謝";
					string SmtpServer = "smtp.gmail.com";
					int SmtpPort = 587;
					MailMessage mms = new MailMessage();
					mms.From = new MailAddress(GoogleID);
					mms.Subject = "BPMPlus 待辦工單" + formId + "退件提醒"; //信件標題
					mms.Body = "<!doctype html>\r\n<html lang=\"en\">\r\n  <head>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\r\n    <title>Simple Transactional Email</title>\r\n    <style media=\"all\" type=\"text/css\">\r\n@media all {\r\n  .btn-primary table td:hover {\r\n    background-color: #ec0867 !important;\r\n  }\r\n\r\n  .btn-primary a:hover {\r\n    background-color: #ec0867 !important;\r\n    border-color: #ec0867 !important;\r\n  }\r\n}\r\n@media only screen and (max-width: 640px) {\r\n  .main p,\r\n.main td,\r\n.main span {\r\n    font-size: 16px !important;\r\n  }\r\n\r\n  .wrapper {\r\n    padding: 8px !important;\r\n  }\r\n\r\n  .content {\r\n    padding: 0 !important;\r\n  }\r\n\r\n  .container {\r\n    padding: 0 !important;\r\n    padding-top: 8px !important;\r\n    width: 100% !important;\r\n  }\r\n\r\n  .main {\r\n    border-left-width: 0 !important;\r\n    border-radius: 0 !important;\r\n    border-right-width: 0 !important;\r\n  }\r\n\r\n  .btn table {\r\n    max-width: 100% !important;\r\n    width: 100% !important;\r\n  }\r\n\r\n  .btn a {\r\n    font-size: 16px !important;\r\n    max-width: 100% !important;\r\n    width: 100% !important;\r\n  }\r\n}\r\n@media all {\r\n  .ExternalClass {\r\n    width: 100%;\r\n  }\r\n\r\n  .ExternalClass,\r\n.ExternalClass p,\r\n.ExternalClass span,\r\n.ExternalClass font,\r\n.ExternalClass td,\r\n.ExternalClass div {\r\n    line-height: 100%;\r\n  }\r\n\r\n  .apple-link a {\r\n    color: inherit !important;\r\n    font-family: inherit !important;\r\n    font-size: inherit !important;\r\n    font-weight: inherit !important;\r\n    line-height: inherit !important;\r\n    text-decoration: none !important;\r\n  }\r\n\r\n  #MessageViewBody a {\r\n    color: inherit;\r\n    text-decoration: none;\r\n    font-size: inherit;\r\n    font-family: inherit;\r\n    font-weight: inherit;\r\n    line-height: inherit;\r\n  }\r\n}\r\n</style>\r\n  </head>\r\n  <body style=\"font-family: Helvetica, sans-serif; -webkit-font-smoothing: antialiased; font-size: 16px; line-height: 1.3; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; background-color: #f4f5f6; margin: 0; padding: 0;\">\r\n    <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"body\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #f4f5f6; width: 100%;\" width=\"100%\" bgcolor=\"#f4f5f6\">\r\n      <tr>\r\n        <td style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top;\" valign=\"top\">&nbsp;</td>\r\n        <td class=\"container\" style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; max-width: 600px; padding: 0; padding-top: 24px; width: 600px; margin: 0 auto;\" width=\"600\" valign=\"top\">\r\n          <div class=\"content\" style=\"box-sizing: border-box; display: block; margin: 0 auto; max-width: 600px; padding: 0;\">\r\n\r\n            <!-- START CENTERED WHITE CONTAINER -->\r\n            <span class=\"preheader\" style=\"color: transparent; display: none; height: 0; max-height: 0; max-width: 0; opacity: 0; overflow: hidden; mso-hide: all; visibility: hidden; width: 0;\">This is preheader text. Some clients will show this text as a preview.</span>\r\n            <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"main\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background: #ffffff; border: 1px solid #eaebed; border-radius: 16px; width: 100%;\" width=\"100%\">\r\n\r\n              <!-- START MAIN CONTENT AREA -->\r\n              <tr>\r\n                <td class=\"wrapper\" style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; box-sizing: border-box; padding: 24px;\" valign=\"top\">\r\n                  <p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">Hi  " + recieveEmp.UserName + "</p>\r\n                  <p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">" + reviewText + "</p>\r\n                  <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"btn btn-primary\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; box-sizing: border-box; width: 100%; min-width: 100%;\" width=\"100%\">\r\n                    <tbody>\r\n                      <tr>\r\n                        <td align=\"left\" style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; padding-bottom: 16px;\" valign=\"top\">\r\n                          <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: auto;\">\r\n                            <tbody>\r\n                              <tr>\r\n                                <td style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; border-radius: 4px; text-align: center; background-color: #0867ec;\" valign=\"top\" align=\"center\" bgcolor=\"#0867ec\"> <a href=\"" + Url + "\" target=\"_blank\" style=\"border: solid 2px #0867ec; border-radius: 4px; box-sizing: border-box; cursor: pointer; display: inline-block; font-size: 16px; font-weight: bold; margin: 0; padding: 12px 24px; text-decoration: none; text-transform: capitalize; background-color: #0867ec; border-color: #0867ec; color: #ffffff;\">" + btnText + "</a> </td>\r\n                              </tr>\r\n                            </tbody>\r\n                          </table>\r\n                        </td>\r\n                      </tr>\r\n                    </tbody>\r\n                  </table>\r\n                  <p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">Have a nice day! :)</p><p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">本信件為系統自動發出，請勿回信，謝謝!!</p>\r\n                </td>\r\n              </tr>\r\n\r\n              <!-- END MAIN CONTENT AREA -->\r\n              </table>\r\n\r\n            <!-- START FOOTER -->\r\n            <div class=\"footer\" style=\"clear: both; padding-top: 24px; text-align: center; width: 100%;\">\r\n              <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%;\" width=\"100%\">\r\n                <tr>\r\n                  <td class=\"content-block\" style=\"font-family: Helvetica, sans-serif; vertical-align: top; color: #9a9ea6; font-size: 16px; text-align: center;\" valign=\"top\" align=\"center\">\r\n                    <span class=\"apple-link\" style=\"color: #9a9ea6; font-size: 16px; text-align: center;\">BPMPlus</span>         </table>\r\n            </div>\r\n\r\n            <!-- END FOOTER -->\r\n            \r\n<!-- END CENTERED WHITE CONTAINER --></div>\r\n        </td>\r\n        <td style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top;\" valign=\"top\">&nbsp;</td></tr></table></body></html>";
					mms.IsBodyHtml = true;
					mms.To.Add(new MailAddress(ReceiveMail));

					using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
					{
						client.EnableSsl = true;
						client.Credentials = new NetworkCredential(GoogleID, TempPwd);//寄信帳密 
						client.Send(mms); //寄出信件
					}
				}
				// 寄給剩下需要審核的人
				else
				{
					//var Url = "https://localhost:7129/Login/Index?returnUrl=/FormReview/Index?id=" + formId.ToString();
					var returnUrl = HttpUtility.UrlEncode($"/FormReview/Index?id={formId}");
					var Url = $"https://localhost:7129/Login/Index?returnUrl={returnUrl}";
					//var Url = "https://localhost:7129/Login/Index";
					var btnText = "審核工單";
					var reviewText = "您好, 提醒您有新的待辦審核工單需處理 ! <br>目前狀態為**" + resultDescription + "**<br>工單流程進度為" + recieveFormRecordEmail.UserActivityIdDescription.ToString() + "<br>以此溫韾提醒~ 感謝";
					string SmtpServer = "smtp.gmail.com";
					int SmtpPort = 587;
					MailMessage mms = new MailMessage();
					mms.From = new MailAddress(GoogleID);
					mms.Subject = "BPMPlus 待辦工單" + formId + "審核提醒"; //信件標題
					mms.Body = "<!doctype html>\r\n<html lang=\"en\">\r\n  <head>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\r\n    <title>Simple Transactional Email</title>\r\n    <style media=\"all\" type=\"text/css\">\r\n@media all {\r\n  .btn-primary table td:hover {\r\n    background-color: #ec0867 !important;\r\n  }\r\n\r\n  .btn-primary a:hover {\r\n    background-color: #ec0867 !important;\r\n    border-color: #ec0867 !important;\r\n  }\r\n}\r\n@media only screen and (max-width: 640px) {\r\n  .main p,\r\n.main td,\r\n.main span {\r\n    font-size: 16px !important;\r\n  }\r\n\r\n  .wrapper {\r\n    padding: 8px !important;\r\n  }\r\n\r\n  .content {\r\n    padding: 0 !important;\r\n  }\r\n\r\n  .container {\r\n    padding: 0 !important;\r\n    padding-top: 8px !important;\r\n    width: 100% !important;\r\n  }\r\n\r\n  .main {\r\n    border-left-width: 0 !important;\r\n    border-radius: 0 !important;\r\n    border-right-width: 0 !important;\r\n  }\r\n\r\n  .btn table {\r\n    max-width: 100% !important;\r\n    width: 100% !important;\r\n  }\r\n\r\n  .btn a {\r\n    font-size: 16px !important;\r\n    max-width: 100% !important;\r\n    width: 100% !important;\r\n  }\r\n}\r\n@media all {\r\n  .ExternalClass {\r\n    width: 100%;\r\n  }\r\n\r\n  .ExternalClass,\r\n.ExternalClass p,\r\n.ExternalClass span,\r\n.ExternalClass font,\r\n.ExternalClass td,\r\n.ExternalClass div {\r\n    line-height: 100%;\r\n  }\r\n\r\n  .apple-link a {\r\n    color: inherit !important;\r\n    font-family: inherit !important;\r\n    font-size: inherit !important;\r\n    font-weight: inherit !important;\r\n    line-height: inherit !important;\r\n    text-decoration: none !important;\r\n  }\r\n\r\n  #MessageViewBody a {\r\n    color: inherit;\r\n    text-decoration: none;\r\n    font-size: inherit;\r\n    font-family: inherit;\r\n    font-weight: inherit;\r\n    line-height: inherit;\r\n  }\r\n}\r\n</style>\r\n  </head>\r\n  <body style=\"font-family: Helvetica, sans-serif; -webkit-font-smoothing: antialiased; font-size: 16px; line-height: 1.3; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; background-color: #f4f5f6; margin: 0; padding: 0;\">\r\n    <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"body\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #f4f5f6; width: 100%;\" width=\"100%\" bgcolor=\"#f4f5f6\">\r\n      <tr>\r\n        <td style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top;\" valign=\"top\">&nbsp;</td>\r\n        <td class=\"container\" style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; max-width: 600px; padding: 0; padding-top: 24px; width: 600px; margin: 0 auto;\" width=\"600\" valign=\"top\">\r\n          <div class=\"content\" style=\"box-sizing: border-box; display: block; margin: 0 auto; max-width: 600px; padding: 0;\">\r\n\r\n            <!-- START CENTERED WHITE CONTAINER -->\r\n            <span class=\"preheader\" style=\"color: transparent; display: none; height: 0; max-height: 0; max-width: 0; opacity: 0; overflow: hidden; mso-hide: all; visibility: hidden; width: 0;\">This is preheader text. Some clients will show this text as a preview.</span>\r\n            <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"main\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background: #ffffff; border: 1px solid #eaebed; border-radius: 16px; width: 100%;\" width=\"100%\">\r\n\r\n              <!-- START MAIN CONTENT AREA -->\r\n              <tr>\r\n                <td class=\"wrapper\" style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; box-sizing: border-box; padding: 24px;\" valign=\"top\">\r\n                  <p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">Hi  " + recieveEmp.UserName + "</p>\r\n                  <p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">" + reviewText + "</p>\r\n                  <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"btn btn-primary\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; box-sizing: border-box; width: 100%; min-width: 100%;\" width=\"100%\">\r\n                    <tbody>\r\n                      <tr>\r\n                        <td align=\"left\" style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; padding-bottom: 16px;\" valign=\"top\">\r\n                          <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: auto;\">\r\n                            <tbody>\r\n                              <tr>\r\n                                <td style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; border-radius: 4px; text-align: center; background-color: #0867ec;\" valign=\"top\" align=\"center\" bgcolor=\"#0867ec\"> <a href=\"" + Url + "\" target=\"_blank\" style=\"border: solid 2px #0867ec; border-radius: 4px; box-sizing: border-box; cursor: pointer; display: inline-block; font-size: 16px; font-weight: bold; margin: 0; padding: 12px 24px; text-decoration: none; text-transform: capitalize; background-color: #0867ec; border-color: #0867ec; color: #ffffff;\">" + btnText + "</a> </td>\r\n                              </tr>\r\n                            </tbody>\r\n                          </table>\r\n                        </td>\r\n                      </tr>\r\n                    </tbody>\r\n                  </table>\r\n                  <p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">Have a nice day! :)</p><p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">本信件為系統自動發出，請勿回信，謝謝!!</p>\r\n                </td>\r\n              </tr>\r\n\r\n              <!-- END MAIN CONTENT AREA -->\r\n              </table>\r\n\r\n            <!-- START FOOTER -->\r\n            <div class=\"footer\" style=\"clear: both; padding-top: 24px; text-align: center; width: 100%;\">\r\n              <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%;\" width=\"100%\">\r\n                <tr>\r\n                  <td class=\"content-block\" style=\"font-family: Helvetica, sans-serif; vertical-align: top; color: #9a9ea6; font-size: 16px; text-align: center;\" valign=\"top\" align=\"center\">\r\n                    <span class=\"apple-link\" style=\"color: #9a9ea6; font-size: 16px; text-align: center;\">BPMPlus</span>         </table>\r\n            </div>\r\n\r\n            <!-- END FOOTER -->\r\n            \r\n<!-- END CENTERED WHITE CONTAINER --></div>\r\n        </td>\r\n        <td style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top;\" valign=\"top\">&nbsp;</td></tr></table></body></html>";
					mms.IsBodyHtml = true;
					mms.To.Add(new MailAddress(ReceiveMail));

					using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
					{
						client.EnableSsl = true;
						client.Credentials = new NetworkCredential(GoogleID, TempPwd);//寄信帳密 
						client.Send(mms); //寄出信件
					}
				}
			}
		}

        public string GetUrl(string Email)
        {
            //當下時間轉時間戳記
            var timeStamp = ToUnixTimestamp(DateTime.Now);
            // 時間戳記+userEamail
            string str = timeStamp + "|" + Email;

            var key = GetKey();
            var ivKey = "";
            string encryptStr = Encrypt(str, key, out ivKey);

            //改變符號-_, 避免URL編碼出錯
            var changeSymbolStr = (ivKey + "|" + encryptStr).Replace('+', '-').Replace('/', '_');

            //URL編碼
            var strToUrl = HttpUtility.UrlEncode(changeSymbolStr);

            string dataStr = "https://localhost:7129/Login/ForgetPwResetPw?data=" + strToUrl;

            return dataStr;
        }

    }
}
