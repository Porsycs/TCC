using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Services.Interface
{
    public interface IJogoRepository : ICommonRepository<Jogo>
    {
        IEnumerable<Jogo> GetByUsuarioId(Guid usuarioId);

        RetornoViewModel InsereJogoDaMemoria(Jogo jogo, List<JogoDaMemoriaMidia> midias);
        RetornoViewModel InsereJogoQuiz(Jogo jogo, List<JogoQuiz> quiz);
    }
}
