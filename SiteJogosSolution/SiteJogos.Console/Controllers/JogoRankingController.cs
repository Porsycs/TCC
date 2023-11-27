using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;
using SiteJogos.Core.Services.Interface;

namespace SiteJogos.Console.Controllers
{
    public class JogoRankingController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJogoRepository _jogoRepository;
        private readonly IJogoRankingRepository _jogoRankingRepository;

        public JogoRankingController(UserManager<Usuario> userManager, IUsuarioRepository usuarioRepository, IJogoRepository jogoRepository, IJogoRankingRepository jogoRankingRepository)
        {
            _userManager = userManager;
            _usuarioRepository = usuarioRepository;
            _jogoRepository = jogoRepository;
            _jogoRankingRepository = jogoRankingRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public object GetByJogoId(Guid jogoId)
        {
            try
            {
                var jogo = _jogoRepository.GetById(jogoId) ?? throw new Exception();
                var ranking =  _jogoRankingRepository.GetByJogoId(jogoId).ToList();

                if(jogo.Template == Jogo.EnumTemplate.JogoDaMemoria)
                {
                    var ordem = 1;
                    ranking = ranking.OrderBy(o => o.Pontuacao).ThenBy(tb => tb.Tempo).ToList();
                    ranking.ForEach(fe =>
                    {
                        fe.Ordem = ordem;
                        ordem++;
                    });
                }
                else if (jogo.Template == Jogo.EnumTemplate.Quiz)
                {
                    var ordem = 1;
                    ranking = ranking.OrderByDescending(o => o.Pontuacao).ThenBy(tb => tb.Tempo).ToList();
                    ranking.ForEach(fe =>
                    {
                        fe.Ordem = ordem;
                        ordem++;
                    });
                }

                return ranking;
            }
            catch (Exception)
            {
                return new List<JogoRanking>();
            }
        }

        [HttpPost]
        public RetornoViewModel Post(string values)
        {
            try
            {

                var Ranking = new JogoRanking();
                JsonConvert.PopulateObject(values, Ranking);

                var jogo = _jogoRepository.GetById(Ranking.JogoId);
                Ranking.UsuarioInclusaoId = jogo.UsuarioInclusaoId;

                _jogoRankingRepository.Insert(Ranking);

                return new RetornoViewModel()
                {
                    Sucesso = true
                };
            }
            catch (Exception e)
            {
                return new RetornoViewModel()
                {
                    Sucesso = false,
                    Mensagem = "Erro: " + e.Message
                };
            }
        }

        [HttpDelete]
        public RetornoViewModel Delete(Guid key)
        {
            return _jogoRankingRepository.DeleteByJogoId(key);
        }
    }
}
