using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities
{
    [Table("Jogo")]
    public class Jogo : BaseEntity
    {
        public string Nome { get; set; }

        public EnumTemplate Template { get; set; }

        public enum EnumTemplate
        {
            [Description("Jogo da Memória")]
            JogoDaMemoria = 0,

            [Description("Forca")]
            Forca = 1,
        }

        [NotMapped]
        public string TemplateDescricao
        {
            get
            {
                return Common.GetEnumDescription(Template);
            }
        } 

        [NotMapped]
        public string IconeJogo
        {
            get
            {
                switch(Template)
                {
                    case EnumTemplate.JogoDaMemoria: return "view_module";
                    case EnumTemplate.Forca: return "table_lamp";
                    default: return "";
                }
            }
        }
    }
}
