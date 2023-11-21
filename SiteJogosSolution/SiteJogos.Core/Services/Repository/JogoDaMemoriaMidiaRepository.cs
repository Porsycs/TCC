using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<JogoDaMemoriaMidia> GetByJogoId(Guid jogoId, bool includes = false)
        {
            if(includes)
                return _dbContext.JogoDaMemoriaMidias.Include(i => i.MidiaPrimaria).Include(i => i.MidiaSecundaria).Where(w => !w.Excluido && w.JogoId == jogoId).ToList();
            else
                return _dbContext.JogoDaMemoriaMidias.Where(w => !w.Excluido && w.JogoId == jogoId);
        }
    }
}
