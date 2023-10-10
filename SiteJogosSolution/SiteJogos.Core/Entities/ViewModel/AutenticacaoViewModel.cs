using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities.ViewModel
{
    public class AutenticacaoViewModel
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool Lembrar { get; set; }
    }
}
