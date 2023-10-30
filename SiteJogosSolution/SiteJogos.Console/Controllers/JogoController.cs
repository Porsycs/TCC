using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;
using SiteJogos.Core.Services.Interface;

namespace SiteJogos.Console.Controllers
{
    [Authorize]
    public class JogoController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJogoRepository _jogoRepository;

        public JogoController(UserManager<Usuario> userManager, IUsuarioRepository usuarioRepository, IJogoRepository jogoRepository)
        {
            _userManager = userManager;
            _usuarioRepository = usuarioRepository;
            _jogoRepository = jogoRepository;
        }

        [HttpGet]
        [Route("/MeusJogos")]
        public IActionResult MeusJogos()
        {
            return View();
        }

        [HttpGet]
        public object Get()
        {
            try
            {
                var jogos = _jogoRepository.GetByUsuarioId(_userManager.GetUserAsync(User).Result.Id).ToList();
                return jogos;
            }
            catch (Exception)
            {
                return new List<Jogo>();
            }
        }

        [HttpGet]
        [Route("/NovoJogo")]
        public IActionResult NovoJogo(Jogo.EnumTemplate template)
        {
            return View();
        }

        [HttpPost]
        public RetornoViewModel InsereJogoDaMemoria(string jogo, string midias)
        {
            var Jogo = new Jogo();
            var Midias = new List<JogoDaMemoriaMidia>();

            JsonConvert.PopulateObject(jogo, Jogo);
            JsonConvert.PopulateObject(midias, Midias);

            Jogo.UsuarioInclusaoId = _userManager.GetUserAsync(User).Result.Id;
            Midias.ForEach(f => 
            {
                f.UsuarioInclusaoId = _userManager.GetUserAsync(User).Result.Id;
                f.MidiaPrimaria.UsuarioInclusaoId = _userManager.GetUserAsync(User).Result.Id;
                f.MidiaSecundaria.UsuarioInclusaoId = _userManager.GetUserAsync(User).Result.Id;
            });
            return _jogoRepository.InsereJogoDaMemoria(Jogo, Midias);
        }
    }
}
