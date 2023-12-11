using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;

namespace SiteJogos.Console.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Usuario> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<Usuario> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [Route("/")]
        public IActionResult Index()
        {
            //if (!_usuarioRepository.VerificaUsuarios())
            //{
            //    var usuarioMaster = new Usuario()
            //    {
            //        UserName = "master@gmail.com",
            //        Nome = "Master",
            //        Email = "master@gmail.com",
            //    };
            //    _userManager.CreateAsync(usuarioMaster, "Master@2023");
            //}
            return View();
        }

        [Route("/Jogo")]
        public IActionResult Jogo()
        {
            return View();
        }

        [Route("/Perfil")]
        [Authorize]
        public IActionResult Perfil()
        {
            return View(_userManager.GetUserAsync(User).Result);
        }

        [Route("/Tutorial")]
        [AllowAnonymous]
        public IActionResult Tutorial()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/JogoDaMemoria")]
        public IActionResult JogoDaMemoria()
        {
            return View("~/Views/Jogo/JogoDaMemoria.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel());
        }
    }
}