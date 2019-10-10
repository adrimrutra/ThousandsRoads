using System.Web.Mvc;

namespace Travel.Web.Controllers
{
	public class HomeController : MvcControllerBase
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
	}
}