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

        public double Tempo { get; set; }

        [NotMapped]
        public int Ordem { get; set; }

        [NotMapped]
        public int Minutos 
        { 
            get
            {
                return (int)(Tempo / 60);
            }
        }

        [NotMapped]
        public int Segundos
        {
            get
            {
                return (int)(Tempo - (Minutos * 60));
            }
        }

        [NotMapped]
        public int Milissegundos
        {
            get
            {
                return (int)((Tempo % 1) * 1000);
            }
        }

        [NotMapped]
        public string TempoFormatado
        {
            get
            {
                return $"{Common.NumeroFormatado(Minutos)}:{Common.NumeroFormatado(Segundos)}:{Common.NumeroFormatado(Milissegundos, true)}";
            }
        }
    }
}
