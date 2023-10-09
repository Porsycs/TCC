using Microsoft.AspNetCore.Mvc;
using SiteJogos.Core.Entities.ViewModel;

namespace SiteJogos.Console.Controllers
{
    public class UsuarioController : Controller
    {

        [Route("/Cadastrar")]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public RetornoViewModel CadastrarUsuario(FormCadastroViewModel formViewModel)
        {
            return new RetornoViewModel
            {
                Sucesso = true,
                Codigo = "info",
                Titulo = "Sucesso!",
                Mensagem = "Usuário cadastrado com sucesso",
            };
        }
    }
}
