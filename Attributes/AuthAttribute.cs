using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BPMPlus.Attributes
{
    public class AuthAttribute : ActionFilterAttribute
    {

        // 建立Attribute掛載到Controller的Action上
        // 
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var isAdmin = context.HttpContext.Session.GetString("IsAdmin");

            if (isAdmin == null) {
                context.Result =  new RedirectToActionResult("Index", "Login", null);
            }
            base.OnResultExecuting(context);
        }
    }
}
