using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities.ViewModel
{
    public class FormCadastroViewModel
    {
        //Input Dados
        public string? type { get; set; }
        public string? classe { get; set; }
        public string? id { get; set; }
        public string? name { get; set; } 
        public string? placeholder { get; set; }


        //Campos Form
        public string? nome { get; set; }
        public string? email { get; set; }
        public string? senha { get; set; }
        public string? confirmaSenha { get; set; }
    }
}
