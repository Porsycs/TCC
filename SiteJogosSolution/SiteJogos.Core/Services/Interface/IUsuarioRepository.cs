using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;
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
        bool VerificaUsuarios(Guid Id, string email);
        Task<bool> EmailRecuperaSenha(string email, Usuario usuario, string token, string link);
        Usuario LoginGoogle(IList<GoogleClaims> claims);
    }
}
