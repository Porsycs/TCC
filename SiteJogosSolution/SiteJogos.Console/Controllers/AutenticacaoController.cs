using Microsoft.AspNetCore.Mvc;

namespace SiteJogos.Console.Controllers
{
    public class AutenticacaoController : Controller
    {
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (email != null && password != null)
            {
                return RedirectToAction("Jogo", "Home");
            }

            else
            {
                return BadRequest();
            }
        }
    }
}
