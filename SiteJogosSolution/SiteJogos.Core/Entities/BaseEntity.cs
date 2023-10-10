using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteJogos.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = new Guid();
        public bool Excluido { get; set; } = false;
        public DateTime Inclusao { get; set; } = DateTime.Now;
        public DateTime? Alteracao { get; set; }

        [ForeignKey("UsuarioInclusao")]
        public Guid UsuarioInclusaoId { get; set; }
        public virtual Usuario? UsuarioInclusao { get; set; }

        [ForeignKey("UsuarioAlteracao")]
        public Guid? UsuarioAlteracaoId { get; set; }
        public virtual Usuario? UsuarioAlteracao { get; set; }
    }
}
