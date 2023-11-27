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
    public class JogoQuizRepository : CommonRepository<JogoQuiz>, IJogoQuizRepository
    {
        public JogoQuizRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration) { }

        public IEnumerable<JogoQuiz> GetByJogoId(Guid jogoId)
        {
            return _dbContext.JogoQuizzes.Where(w => !w.Excluido && w.JogoId == jogoId).OrderBy(o => o.Ordem).ToList();
        }
    }
}
