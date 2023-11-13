using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;
using SiteJogos.Core.Services.Interface;

namespace SiteJogos.Console.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMidiaRepository _midiaRepository;

        public UsuarioController(UserManager<Usuario> userManager, IUsuarioRepository usuarioRepository, IMidiaRepository midiaRepository)
        {
            _userManager = userManager;
            _usuarioRepository = usuarioRepository;
            _midiaRepository = midiaRepository;
        }

        [HttpPut]
        [Authorize]
        public RetornoViewModel Put(string nome, string email, string? fileName = null, string? base64 = null)
        {
            try
            {
                var usuario = _userManager.GetUserAsync(User).Result;
                if (_usuarioRepository.VerificaUsuarios(usuario.Id, email)) throw new Exception("Já existe um usuário com esse e-mail");

                usuario.Nome = nome;
                usuario.Email = email;
                usuario.NormalizedEmail = email.ToUpper();
                usuario.UserName = email;
                usuario.NormalizedUserName = email.ToUpper();

                if(!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(base64))
                {
                    if (usuario.MidiaId.HasValue)
                    {
                        var midia = _midiaRepository.GetById(usuario.MidiaId.Value);
                        midia.Nome = fileName;
                        midia.Arquivo = Convert.FromBase64String(base64);
                        _midiaRepository.Update(midia);
                    }
                    else
                    {
                        var midia = new Midia()
                        {
                            UsuarioInclusaoId = usuario.Id,
                            UsuarioAlteracaoId = usuario.Id,
                            Arquivo = Convert.FromBase64String(base64),
                            Nome = fileName
                        };
                        _midiaRepository.Insert(midia);
                        usuario.MidiaId = midia.Id;
                    }
                }

                _usuarioRepository.InsertOrReplace(usuario);

                return new RetornoViewModel
                {
                    Sucesso = true,
                    Codigo = "success",
                    Titulo = "Sucesso!",
                    Mensagem = "Informações salvas com sucesso",
                };
            }
            catch(Exception e) 
            {
                return new RetornoViewModel
                {
                    Sucesso = false,
                    Codigo = "warning",
                    Titulo = "Erro ao salvar",
                    Mensagem = "Ocorreu um erro ao salvar",
                };
            }
        }
    }
}
