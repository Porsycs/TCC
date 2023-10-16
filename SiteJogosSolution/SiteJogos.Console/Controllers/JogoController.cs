using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiteJogos.Console.Controllers
{
    public class JogoController : Controller
    {
        [Authorize]
        public IActionResult MeusJogos()
        {
            return View();
        }
    }
}
