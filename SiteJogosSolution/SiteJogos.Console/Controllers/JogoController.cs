using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRCoder;
using Rotativa.AspNetCore;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;
using SiteJogos.Core.Services.Interface;
using SiteJogos.Core.Services.Repository;
using static QRCoder.PayloadGenerator;

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
        [Route("/QRCode")]
        public IActionResult QRCode(Guid id)
        {
            var jogo = _jogoRepository.GetById(id);
            jogo.UsuarioInclusao = _usuarioRepository.GetById(jogo.UsuarioInclusaoId);

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode($"{Request.Scheme}://{Request.Host.Value}/Play?id={jogo.Id}", QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrCodeData);
            using var stream = new MemoryStream();
            var qrCodeImage = qrCode.GetGraphic(10);
            qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            jogo.Base64QrCode = Convert.ToBase64String(stream.ToArray());

            var pdf = new ViewAsPdf("_PDFJogoQRCode", jogo)
            {
                FileName = $"QR Code - {Common.RemoveCaratereEspecial(jogo.Nome)}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                ContentDisposition = Rotativa.AspNetCore.Options.ContentDisposition.Inline,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(1, 1, 1, 1),
            };
            return pdf;
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
