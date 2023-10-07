using System.ComponentModel.DataAnnotations.Schema;

namespace SiteJogos.Core.Entities
{
    public class TemplateJogoMemoria : BaseEntity
    {
        public string? JogoMemoriaNome { get; set; }
        [ForeignKey("Template")]
        public Guid? TemplateId { get; set; }
        public virtual Template? Template { get; set; }

    }
}
