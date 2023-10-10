using SiteJogos.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Services.Interface
{
    public interface IUsuarioRepository
    {
        IEnumerable<Usuario> GetAll();
        Usuario GetById(Guid id);
        void InsertOrReplace(Usuario usuario);
        void Delete(Guid id);
        bool VerificaUsuarios();
    }
}
