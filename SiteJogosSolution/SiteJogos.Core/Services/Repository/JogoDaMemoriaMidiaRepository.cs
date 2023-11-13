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
    public class JogoDaMemoriaMidiaRepository : CommonRepository<JogoDaMemoriaMidia>, IJogoDaMemoriaMidiaRepository
    {
        public JogoDaMemoriaMidiaRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration) { }

        public IEnumerable<JogoDaMemoriaMidia> GetByJogoId(Guid jogoId)
        {
            return _dbContext.JogoDaMemoriaMidias.Where(w => !w.Excluido && w.JogoId == jogoId);
        }
    }
}
