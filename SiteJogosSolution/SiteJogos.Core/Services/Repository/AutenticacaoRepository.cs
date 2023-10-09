using SiteJogos.Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Services.Repository
{
    public class AutenticacaoRepository : IAutenticacaoRepository
    {

        public bool Login(string username, string password)
        {
            if (username.ToLower() == "apresentacao" && password == "Apresentacao@2023")
                return true;

            else
                return false;
        }

    }
}
