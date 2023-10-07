using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities
{
    public class Template: BaseEntity
    {
        public string? TemplateNome { get; set; }
        [ForeignKey("TemplateJogo")]
        public Guid TemplateJogoId { get; set; }
        public virtual TemplateJogo? TemplateJogo { get; set; }
    }
}
