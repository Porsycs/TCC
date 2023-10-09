using Microsoft.AspNetCore.Mvc;
using SiteJogos.Core.Entities.ViewModel;
using SiteJogos.Core.Services.Interface;

namespace SiteJogos.Console.Controllers
{
    public class AutenticacaoController : Controller
    {
        private readonly IAutenticacaoRepository _autenticacaoRepository;
        public AutenticacaoController(IAutenticacaoRepository autenticacaoRepository)
        {
            _autenticacaoRepository = autenticacaoRepository;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(FormCadastroViewModel formViewModel)
        {
            var loginSucesso = _autenticacaoRepository.Login(formViewModel.email, formViewModel.senha);

            if (loginSucesso)
                return RedirectToAction("Jogo", "Home");
            else
                return BadRequest();
        }
    }
}
