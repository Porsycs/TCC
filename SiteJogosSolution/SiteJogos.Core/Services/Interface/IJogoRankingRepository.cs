﻿using SiteJogos.Core.Entities;
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
    }
}
