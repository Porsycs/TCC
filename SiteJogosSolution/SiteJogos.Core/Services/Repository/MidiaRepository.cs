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
    public class MidiaRepository : CommonRepository<Midia>, IMidiaRepository
    {
        public MidiaRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration) { }
    }
}
