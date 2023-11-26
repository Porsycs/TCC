using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities
{
    [Table("JogoQuiz")]
    public class JogoQuiz : BaseEntity
    {
        [ForeignKey("Jogo")]
        public Guid JogoId { get; set; }
        public virtual Jogo Jogo { get; set; }

        public int Ordem { get; set; }

        public string Pergunta { get; set; }
        public string? Resposta1 { get; set; }
        public string? Resposta2 { get; set; }
        public string? Resposta3 { get; set; }
        public string? Resposta4 { get; set; }
        public int RespostaCerta { get; set; }
    }
}
