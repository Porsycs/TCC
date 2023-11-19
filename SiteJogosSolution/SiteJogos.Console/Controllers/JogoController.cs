using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;
using SiteJogos.Core.Services.Interface;
using SiteJogos.Core.Services.Repository;

namespace SiteJogos.Console.Controllers
{
    [Authorize]
    public class JogoController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJogoRepository _jogoRepository;
        private readonly IJogoDaMemoriaMidiaRepository _jogoDaMemoriaMidiaRepository;

        public JogoController(UserManager<Usuario> userManager, IUsuarioRepository usuarioRepository, IJogoRepository jogoRepository, IJogoDaMemoriaMidiaRepository jogoDaMemoriaMidiaRepository)
        {
            _userManager = userManager;
            _usuarioRepository = usuarioRepository;
            _jogoRepository = jogoRepository;
            _jogoDaMemoriaMidiaRepository = jogoDaMemoriaMidiaRepository;
        }

        [HttpGet]
        [Route("/MeusJogos")]
        public IActionResult MeusJogos()
        {
            ViewData["UrlRequest"] = Request.Scheme + "://" + Request.Host.Value;
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
        [AllowAnonymous]
        [Route("/Play")]
        public IActionResult Play(Guid id)
        {
            try
            {
                var jogo = _jogoRepository.GetById(id) ?? throw new Exception("Jogo não encontrado");

                if(jogo.Template == Jogo.EnumTemplate.JogoDaMemoria)
                {
                    ViewData["Midias"] = _jogoDaMemoriaMidiaRepository.GetByJogoId(jogo.Id).ToList(); 
                    return View("~/Views/Jogo/JogoDaMemoria.cshtml", jogo);
                }

                throw new Exception();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public RetornoViewModel Delete(Guid key)
        {
            try
            {
                _jogoRepository.Delete(key);
                return new RetornoViewModel()
                {
                    Sucesso = true,
                    Mensagem = "Jogo deletado com sucesso"
                };
            }
            catch (Exception e)
            {
                return new RetornoViewModel()
                {
                    Sucesso = false,
                    Mensagem = e.Message
                };
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
