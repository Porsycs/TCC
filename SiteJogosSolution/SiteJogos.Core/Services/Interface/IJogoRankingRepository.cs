using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Services.Interface
{
    public interface IJogoRankingRepository : ICommonRepository<JogoRanking>
    {
        public IEnumerable<JogoRanking> GetByJogoId(Guid jogoId);
        RetornoViewModel DeleteByJogoId(Guid jogoId);
    }
}
