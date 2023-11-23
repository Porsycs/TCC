using Microsoft.Extensions.Configuration;
using SiteJogos.Core.Context;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Services.Repository
{
    public class JogoRankingRepository : CommonRepository<JogoRanking>, IJogoRankingRepository
    {
        public JogoRankingRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration) { }

        public IEnumerable<JogoRanking> GetByJogoId(Guid jogoId)
        {
            return _dbContext.JogoRankings.Where(w => w.JogoId == jogoId && !w.Excluido);
        }
    }
}
