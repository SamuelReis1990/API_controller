using System.Web.Mvc;

namespace API.ControleRapido.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "API Controle Rápido";

            return View();
        }
    }
}
