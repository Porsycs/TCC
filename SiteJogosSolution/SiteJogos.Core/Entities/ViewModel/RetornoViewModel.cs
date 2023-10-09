using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities.ViewModel
{
    public class RetornoViewModel
    {
        public bool Sucesso { get; set; }
        public string? Titulo { get; set; }
        public string? Mensagem { get; set; }
        public string? Codigo { get; set; }
    }
}
