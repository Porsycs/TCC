using Microsoft.AspNetCore.Mvc;
using SiteJogos.Core.Entities.ViewModel;

namespace SiteJogos.Console.Controllers
{
    public class AutenticacaoController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Cadastrar")]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(FormCadastroViewModel formViewModel)
        {
            if (formViewModel.email != null && formViewModel.senha != null)
            {
                return RedirectToAction("Jogo", "Home");
            }

            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public RetornoViewModel CadastrarUsuario(FormCadastroViewModel formViewModel)
        {
            return  new RetornoViewModel 
            {
                Sucesso = "info",
                Titulo = "Sucesso!",
                Mensagem = "Usuário cadastrado com sucesso",
            };
        }
    }
}
