using SiteJogos.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Services.Interface
{
    public interface IJogoDaMemoriaMidiaRepository : ICommonRepository<JogoDaMemoriaMidia>
    {
        IEnumerable<JogoDaMemoriaMidia> GetByJogoId(Guid jogoId, bool includes = false);
    }
}
