using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Services.Interface
{
    public interface IAutenticacaoRepository
    {
        bool Login(string login, string senha);
    }
}
