using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities
{
    [Table("JogoRanking")]
    public class JogoRanking : BaseEntity
    {
        [ForeignKey("Jogo")]
        public Guid JogoId { get; set; }
        public virtual Jogo Jogo { get; set; }

        public string Jogador { get; set; }

        public int Pontuacao { get; set; }

        [NotMapped]
        public int Ordem { get; set; }
    }
}
