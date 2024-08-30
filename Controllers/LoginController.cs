using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BPMPlus.Controllers
{
	public class LoginController : Controller
	{
		// GET: LoginController
		public ActionResult Index()
		{
			return View();
		}

        public ActionResult EmailValid()
        {
            return View();
        }

        public ActionResult EmailForgetPassWord()
        {
            return View();
        }
        public ActionResult ResetPassWord()
        {
            return View();
        }
    }
}
