using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities
{
    [Table("Jogo")]
    public class Jogo : BaseEntity
    {
        public string Nome { get; set; }

        public EnumTemplate Template { get; set; }

        public enum EnumTemplate
        {
            JogoDaMemoria = 0,
            Forca = 1,
        }
    }
}
