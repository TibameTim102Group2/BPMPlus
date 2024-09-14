﻿using System.Net.Mail;
using System.Net;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace BPMPlus.Service
{
    public class EmailService(IConfiguration configuration) : AesAndTimestampService(configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public void SendEmail(string Email,string UserName)
        {
            var resetPasswordUrl = GetUrl(Email);

            // 使用 Google Mail Server 發信
            string GoogleID = "bpmplus102@gmail.com"; //Google 發信帳號
            string TempPwd = "ygzz mxhm zmao weyb"; //應用程式密碼
            string ReceiveMail = Email; //接收信箱

            string SmtpServer = "smtp.gmail.com";
            int SmtpPort = 587;
            MailMessage mms = new MailMessage();
            mms.From = new MailAddress(GoogleID);
            mms.Subject = "BPMPlus 密碼重設";
            mms.Body = "<!doctype html>\r\n<html lang=\"en\">\r\n  <head>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\r\n    <title>Simple Transactional Email</title>\r\n    <style media=\"all\" type=\"text/css\">\r\n@media all {\r\n  .btn-primary table td:hover {\r\n    background-color: #ec0867 !important;\r\n  }\r\n\r\n  .btn-primary a:hover {\r\n    background-color: #ec0867 !important;\r\n    border-color: #ec0867 !important;\r\n  }\r\n}\r\n@media only screen and (max-width: 640px) {\r\n  .main p,\r\n.main td,\r\n.main span {\r\n    font-size: 16px !important;\r\n  }\r\n\r\n  .wrapper {\r\n    padding: 8px !important;\r\n  }\r\n\r\n  .content {\r\n    padding: 0 !important;\r\n  }\r\n\r\n  .container {\r\n    padding: 0 !important;\r\n    padding-top: 8px !important;\r\n    width: 100% !important;\r\n  }\r\n\r\n  .main {\r\n    border-left-width: 0 !important;\r\n    border-radius: 0 !important;\r\n    border-right-width: 0 !important;\r\n  }\r\n\r\n  .btn table {\r\n    max-width: 100% !important;\r\n    width: 100% !important;\r\n  }\r\n\r\n  .btn a {\r\n    font-size: 16px !important;\r\n    max-width: 100% !important;\r\n    width: 100% !important;\r\n  }\r\n}\r\n@media all {\r\n  .ExternalClass {\r\n    width: 100%;\r\n  }\r\n\r\n  .ExternalClass,\r\n.ExternalClass p,\r\n.ExternalClass span,\r\n.ExternalClass font,\r\n.ExternalClass td,\r\n.ExternalClass div {\r\n    line-height: 100%;\r\n  }\r\n\r\n  .apple-link a {\r\n    color: inherit !important;\r\n    font-family: inherit !important;\r\n    font-size: inherit !important;\r\n    font-weight: inherit !important;\r\n    line-height: inherit !important;\r\n    text-decoration: none !important;\r\n  }\r\n\r\n  #MessageViewBody a {\r\n    color: inherit;\r\n    text-decoration: none;\r\n    font-size: inherit;\r\n    font-family: inherit;\r\n    font-weight: inherit;\r\n    line-height: inherit;\r\n  }\r\n}\r\n</style>\r\n  </head>\r\n  <body style=\"font-family: Helvetica, sans-serif; -webkit-font-smoothing: antialiased; font-size: 16px; line-height: 1.3; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; background-color: #f4f5f6; margin: 0; padding: 0;\">\r\n    <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"body\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #f4f5f6; width: 100%;\" width=\"100%\" bgcolor=\"#f4f5f6\">\r\n      <tr>\r\n        <td style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top;\" valign=\"top\">&nbsp;</td>\r\n        <td class=\"container\" style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; max-width: 600px; padding: 0; padding-top: 24px; width: 600px; margin: 0 auto;\" width=\"600\" valign=\"top\">\r\n          <div class=\"content\" style=\"box-sizing: border-box; display: block; margin: 0 auto; max-width: 600px; padding: 0;\">\r\n\r\n            <!-- START CENTERED WHITE CONTAINER -->\r\n            <span class=\"preheader\" style=\"color: transparent; display: none; height: 0; max-height: 0; max-width: 0; opacity: 0; overflow: hidden; mso-hide: all; visibility: hidden; width: 0;\">This is preheader text. Some clients will show this text as a preview.</span>\r\n            <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"main\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background: #ffffff; border: 1px solid #eaebed; border-radius: 16px; width: 100%;\" width=\"100%\">\r\n\r\n              <!-- START MAIN CONTENT AREA -->\r\n              <tr>\r\n                <td class=\"wrapper\" style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; box-sizing: border-box; padding: 24px;\" valign=\"top\">\r\n                  <p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">Hi  " + UserName + "</p>\r\n                  <p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">您執行了忘記密碼的功能，請點擊以下連結重新設定新密碼!!<br>此連結有效時間為15分鐘，逾期請重新申請!!</p>\r\n                  <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"btn btn-primary\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; box-sizing: border-box; width: 100%; min-width: 100%;\" width=\"100%\">\r\n                    <tbody>\r\n                      <tr>\r\n                        <td align=\"left\" style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; padding-bottom: 16px;\" valign=\"top\">\r\n                          <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: auto;\">\r\n                            <tbody>\r\n                              <tr>\r\n                                <td style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top; border-radius: 4px; text-align: center; background-color: #0867ec;\" valign=\"top\" align=\"center\" bgcolor=\"#0867ec\"> <a href=\""+resetPasswordUrl+"\" target=\"_blank\" style=\"border: solid 2px #0867ec; border-radius: 4px; box-sizing: border-box; cursor: pointer; display: inline-block; font-size: 16px; font-weight: bold; margin: 0; padding: 12px 24px; text-decoration: none; text-transform: capitalize; background-color: #0867ec; border-color: #0867ec; color: #ffffff;\">密碼重設</a> </td>\r\n                              </tr>\r\n                            </tbody>\r\n                          </table>\r\n                        </td>\r\n                      </tr>\r\n                    </tbody>\r\n                  </table>\r\n                  <p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">Have a nice day! :)</p><p style=\"font-family: Helvetica, sans-serif; font-size: 16px; font-weight: normal; margin: 0; margin-bottom: 16px;\">本信件為系統自動發出，請勿回信，謝謝!!</p>\r\n                </td>\r\n              </tr>\r\n\r\n              <!-- END MAIN CONTENT AREA -->\r\n              </table>\r\n\r\n            <!-- START FOOTER -->\r\n            <div class=\"footer\" style=\"clear: both; padding-top: 24px; text-align: center; width: 100%;\">\r\n              <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: separate; mso-table-lspace: 0pt; mso-table-rspace: 0pt; width: 100%;\" width=\"100%\">\r\n                <tr>\r\n                  <td class=\"content-block\" style=\"font-family: Helvetica, sans-serif; vertical-align: top; color: #9a9ea6; font-size: 16px; text-align: center;\" valign=\"top\" align=\"center\">\r\n                    <span class=\"apple-link\" style=\"color: #9a9ea6; font-size: 16px; text-align: center;\">BPMPlus</span>         </table>\r\n            </div>\r\n\r\n            <!-- END FOOTER -->\r\n            \r\n<!-- END CENTERED WHITE CONTAINER --></div>\r\n        </td>\r\n        <td style=\"font-family: Helvetica, sans-serif; font-size: 16px; vertical-align: top;\" valign=\"top\">&nbsp;</td></tr></table></body></html>";
            mms.IsBodyHtml = true;
            mms.To.Add(new MailAddress(ReceiveMail));

            using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(GoogleID, TempPwd);//寄信帳密 
                client.Send(mms); //寄出信件
            }

        }

        public string GetUrl(string Email)
        {
            //當下時間轉時間戳記
            var stampTime = ToUnixTimestamp(DateTime.Now);
            // 時間戳記+userEamail
            string str = stampTime + "|" + Email;

            var key = GenerateKey();
            var ivKey = "";
            string encryptStr = Encrypt(str, key, out ivKey);

            return "https://localhost:7129/Login/ForgetPwResetPw?data="+ ivKey + ";" + encryptStr;
        }

    }
}
