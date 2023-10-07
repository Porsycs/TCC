using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteJogos.Core.Entities
{
    public class Usuario : IdentityUser<Guid>
    {
        public string? Nome { get; set; }
        public bool Excluido { get; set; } = false;
        public DateTime Inclusao { get; set; } = DateTime.Now;
        public DateTime? Alteracao { get; set; }
        [ForeignKey("User")]
        public Guid UsuarioInclusao { get; set; }
        [ForeignKey("User")]
        public Guid? UsuarioAlteracao { get; set; }
        public virtual Usuario? User { get; set; }

    }
}
