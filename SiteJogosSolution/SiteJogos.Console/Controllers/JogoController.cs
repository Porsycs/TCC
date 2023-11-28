using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRCoder;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;
using SiteJogos.Core.Services.Interface;
using System.Net;

namespace SiteJogos.Console.Controllers
{
    [Authorize]
    public class JogoController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJogoRepository _jogoRepository;
        private readonly IJogoDaMemoriaMidiaRepository _jogoDaMemoriaMidiaRepository;
        private readonly IJogoQuizRepository _jogoQuizRepository;

        public JogoController(UserManager<Usuario> userManager, IUsuarioRepository usuarioRepository, IJogoRepository jogoRepository, IJogoDaMemoriaMidiaRepository jogoDaMemoriaMidiaRepository, IJogoQuizRepository jogoQuizRepository)
        {
            _userManager = userManager;
            _usuarioRepository = usuarioRepository;
            _jogoRepository = jogoRepository;
            _jogoDaMemoriaMidiaRepository = jogoDaMemoriaMidiaRepository;
            _jogoQuizRepository = jogoQuizRepository;
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
        [Route("/QRCode")]
        public IActionResult QRCode(Guid id)
        {
            try
            {
                using var client = new WebClient();
                var html = client.DownloadString($"{Request.Scheme}://{Request.Host}/GetQRCode?id={id}");
                return File(PdfSharpConvert(html), "application/pdf");
            }
            catch(Exception ex)
            {
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel { Descricao = ex.Message + "/" + ex?.InnerException?.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/GetQRCode")]
        public IActionResult GetQRCode(Guid id)
        {
            var jogo = _jogoRepository.GetById(id);
            jogo.UsuarioInclusao = _usuarioRepository.GetById(jogo.UsuarioInclusaoId);

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode($"http://oficialhistoriando.azurewebsites.net/Play?id={jogo.Id}", QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrCodeData);
            using var stream = new MemoryStream();
            var qrCodeImage = qrCode.GetGraphic(6);
            qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            jogo.Base64QrCode = Convert.ToBase64String(stream.ToArray());

            return View("~/Views/Jogo/_PDFJogoQRCode.cshtml", jogo);
        }

        private static byte[] PdfSharpConvert(string html)
        {
            byte[] res;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4, 4);
                pdf.Save(ms);
                res = ms.ToArray();
            }
            return res;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/Play")]
        public IActionResult Play(Guid id)
        {
            try
            {
                var jogo = _jogoRepository.GetById(id) ?? throw new Exception("Jogo não encontrado");
                var jogador = HttpContext.Request.Cookies["NomeJogador"]?.Trim();
                if (string.IsNullOrEmpty(jogador))
                {
                    return View("~/Views/Jogo/Jogador.cshtml");
                }

                ViewData["Jogador"] = jogador;
                HttpContext.Response.Cookies.Append("NomeJogador", "");
                HttpContext.Response.Cookies.Append("NomeJogadorUtilizado", jogador);

                if (jogo.Template == Jogo.EnumTemplate.JogoDaMemoria)
                {
                    ViewData["Midias"] = _jogoDaMemoriaMidiaRepository.GetByJogoId(jogo.Id).ToList(); 
                    return View("~/Views/Jogo/JogoDaMemoria.cshtml", jogo);
                }
                else if (jogo.Template == Jogo.EnumTemplate.Quiz)
                {
                    var quiz = _jogoQuizRepository.GetByJogoId(jogo.Id).ToList();
                    if (jogo.Aleatorio)
                    {
                        quiz = Common.EmbaralharLista(quiz);
                    }
                    ViewData["Quiz"] = quiz;
                    return View("~/Views/Jogo/JogoQuiz.cshtml", jogo);
                }

                throw new Exception();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public bool InsereJogador(string nome)
        {
            HttpContext.Response.Cookies.Append("NomeJogador", nome);
            return true;
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
            ViewData["Existe"] = false;
            var novoJogo = new Jogo()
            {
                Template = template,
            };
            return View(novoJogo);
        }

        [HttpGet]
        [Route("/Editar")]
        public IActionResult Editar(Guid id)
        {
            try
            {
                ViewData["Existe"] = true;
                var jogo = _jogoRepository.GetById(id) ?? throw new Exception("Jogo não encontrado");
                if (jogo.Template == Jogo.EnumTemplate.JogoDaMemoria)
                {
                    ViewData["Midias"] = _jogoDaMemoriaMidiaRepository.GetByJogoId(jogo.Id, true).ToList();
                }
                else if(jogo.Template == Jogo.EnumTemplate.Quiz)
                {
                    ViewData["Quiz"] = _jogoQuizRepository.GetByJogoId(jogo.Id);
                }
                return View("~/Views/Jogo/NovoJogo.cshtml", jogo);
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
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

        [HttpPost]
        public RetornoViewModel InsereJogoQuiz(string jogo, string quiz)
        {
            var Jogo = new Jogo();
            var jogoQuiz = new List<JogoQuiz>();

            JsonConvert.PopulateObject(jogo, Jogo);
            JsonConvert.PopulateObject(quiz, jogoQuiz);

            Jogo.UsuarioInclusaoId = _userManager.GetUserAsync(User).Result.Id;
            jogoQuiz.ForEach(f =>
            {
                f.UsuarioInclusaoId = _userManager.GetUserAsync(User).Result.Id;
            });
            return _jogoRepository.InsereJogoQuiz(Jogo, jogoQuiz);
        }
    }
}
