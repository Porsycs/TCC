using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities.ViewModel
{
    public class ErrorViewModel
    {
        public int Codigo { get; set; } = 400;
        public string Descricao { get; set; };
    }
}
