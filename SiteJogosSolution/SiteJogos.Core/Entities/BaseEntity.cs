using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = new Guid();
        public bool Excluido { get; set; } = false;
        public DateTime Inclusao { get; set; } = DateTime.Now;
        public DateTime? Alteracao { get; set; }
        [ForeignKey("Usuario")]
        public Guid UsuarioInclusao { get; set; }
        [ForeignKey("Usuario")]
        public Guid? UsuarioAlteracao { get; set; }
        public virtual Usuario? Usuario { get; set; }

    }
}
