using Microsoft.AspNetCore.Mvc;
using SiteJogos.Core.Entities.ViewModel;
using SiteJogos.Core.ViewModels;
using System.Diagnostics;

namespace SiteJogos.Console.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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