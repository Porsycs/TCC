using System.ComponentModel.DataAnnotations.Schema;

namespace SiteJogos.Core.Entities
{
    public class TemplateJogoForca : BaseEntity
    {
        public string? JogoForcaNome { get; set; }
        [ForeignKey("Template")]
        public Guid? TemplateId { get; set; }
        public virtual Template? Template { get; set; }
    }
}
