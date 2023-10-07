using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities
{
    public class TemplateJogo: BaseEntity
    {
        public TipoJogo JogoTipo { get; set; }



        public enum TipoJogo
        {
            [Description("Jogo da Memória")]
            JogoMemoria = 0,
            [Description("Jogo da Forca")]
            JogoForca = 1,
        }

    }
}
