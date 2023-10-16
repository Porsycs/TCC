using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Entities
{
    [Table("Midia")]
    public class Midia : BaseEntity
    {
        public byte[] Arquivo { get; set; }

        public string Nome { get; set; }

        [NotMapped]
        public string Extensao
        {
            get
            {
                return Path.GetExtension(Nome);
            }
        }

        [NotMapped]
        public string NomeSemExtensao
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Nome);
            }
        }
    }
}
