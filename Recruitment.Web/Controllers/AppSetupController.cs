using Microsoft.AspNetCore.Mvc;

namespace Recruitment.Web.Controllers
{
    public class AppSetupController : Controller
    {
        public IActionResult Index()
        {
            ViewData["BreadcrumbTitle"] = "Setup";

            return View();
        }
    }
}
 