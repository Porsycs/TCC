using Microsoft.AspNetCore.Mvc;
using SiteJogos.Core.Entities.ViewModel;

namespace SiteJogos.Console.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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