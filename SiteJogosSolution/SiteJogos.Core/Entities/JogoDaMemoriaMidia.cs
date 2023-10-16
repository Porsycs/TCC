using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities
{
    [Table("JogoDaMemoriaMidia")]
    public class JogoDaMemoriaMidia : BaseEntity
    {
        [ForeignKey("Jogo")]
        public Guid JogoId { get; set; }
        public virtual Jogo Jogo { get; set; }

        [ForeignKey("MidiaPrimaria")]
        public Guid MidiaPrimariaId { get; set; }
        public virtual Midia MidiaPrimaria { get; set; }

        [ForeignKey("MidiaSecundaria")]
        public Guid MidiaSecundariaId { get; set; }
        public virtual Midia MidiaSecundaria { get; set; }
    }
}
