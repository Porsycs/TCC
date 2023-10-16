using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Services.Interface;

namespace SiteJogos.Console.Controllers
{
    public class JogoController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IUsuarioRepository _usuarioRepository;

        public JogoController(UserManager<Usuario> userManager, IUsuarioRepository usuarioRepository)
        {
            _userManager = userManager;
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        [Authorize]
        [Route("/MeusJogos")]
        public IActionResult MeusJogos()
        {
            return View();
        }
    }
}
