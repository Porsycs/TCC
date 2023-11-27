using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;
using SiteJogos.Core.Services.Interface;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace SiteJogos.Console.Controllers
{
    public class AutenticacaoController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IUsuarioRepository _usuarioRepository;

        public AutenticacaoController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IUsuarioRepository usuarioRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _usuarioRepository = usuarioRepository;
        }

        [Route("/Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if(_userManager.GetUserAsync(User).Result != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [Route("GoogleLogin")]
        public async Task GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        [Route("GoogleResponse")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            var claims = result?.Principal?.Identities?.FirstOrDefault()?.Claims.Select(claim => new GoogleClaims
            {
                Issuer = claim.Issuer,
                OriginalIssuer = claim.OriginalIssuer,
                Type = claim.Type,
                Value = claim.Value
            }).ToList();

            var usuario = _usuarioRepository.LoginGoogle(claims);
            await _signInManager.SignInAsync(usuario, true);

            return RedirectToAction("Index", "Home");

        }

        [Route("/Cadastrar")]
        [AllowAnonymous]
        public IActionResult Cadastrar()
        {
            if (_userManager.GetUserAsync(User).Result != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public RetornoViewModel RealizaLogin(AutenticacaoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userManager.FindByNameAsync(model.Login).Result;

                    // Verifica se o usuário existe
                    if (user == null || user.Excluido) throw new Exception("Usuário não encontrado");

                    var result = _signInManager.PasswordSignInAsync(model.Login, model.Senha, model.Lembrar, lockoutOnFailure: false).Result;

                    if (result.Succeeded)
                    {
                        return new RetornoViewModel
                        {
                            Sucesso = true,
                            Titulo = "Sucesso",
                            Mensagem = "Sucesso ao logar no site!",
                            Codigo = "success"
                        };
                    }
                    else
                    {
                        throw new Exception("Usuário ou Senha Inválidos");
                    }
                }

                throw new Exception("Falha no login");
            }
            catch (Exception e)
            {
                return new RetornoViewModel
                {
                    Sucesso = false,
                    Titulo = "Atenção",
                    Mensagem = e.Message ?? "Ocorreu um erro ao entrar no site!",
                    Codigo = "warning"
                };
            }
        }

        [HttpPost]
        public RetornoViewModel TrocaSenha(SenhaViewModel model)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;

                // Verifica se o usuário existe
                if (user == null || user.Excluido) throw new Exception("Usuário não encontrado");
                if (model.SenhaNova != model.RepetirSenhaNova) throw new Exception("Senhas novas estão diferentes");

                var retorno = _userManager.ChangePasswordAsync(user, model.SenhaAtual, model.SenhaNova).Result;
                if (retorno.Succeeded)
                {
                    return new RetornoViewModel
                    {
                        Sucesso = true,
                        Mensagem = "Senha alterada com sucesso"
                    };
                }
                else
                {
                    throw new Exception($"Ocorreram alguns erros ao alterar a senha: {string.Join(", ", retorno.Errors.Select(s => s.Description))}");
                }

            }
            catch (Exception e)
            {
                return new RetornoViewModel
                {
                    Sucesso = false,
                    Mensagem = e.Message
                };
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public RetornoViewModel CadastraUsuario(FormCadastroViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userManager.FindByNameAsync(model.Email).Result;

                    // Verifica se o usuário existe
                    if (user != null) throw new Exception("Já existe um usuário cadastrado com esse e-mail");

                    var novoUsuario = new Usuario()
                    {
                        Email = model.Email,
                        Nome = model.Nome,
                        UserName = model.Email,
                    };

                    var result = _userManager.CreateAsync(novoUsuario, model.Senha).Result;

                    if (result.Succeeded)
                    {
                        return new RetornoViewModel
                        {
                            Sucesso = true,
                            Codigo = "success",
                            Titulo = "Sucesso!",
                            Mensagem = "Usuário cadastrado com sucesso!"
                        };
                    }
                    
                    
                    else
                    {
                        var listaErros = new List<string>();
                        foreach (var erro in result.Errors.ToList())
                        {   
                            listaErros.Add(erro.Description);
                        }
                        var erroMensagem = string.Join(";", listaErros);
                        return new RetornoViewModel
                        {
                            Sucesso = false,
                            Titulo = "Atenção!",
                            Codigo = "warning",
                            Mensagem = $"{erroMensagem}",
                            Duracao = 10000
                        };
                    }
                }
                throw new Exception();
            }
            catch (Exception)
            {
                return new RetornoViewModel
                {
                    Sucesso = false,
                    Titulo = "Atenção!",
                    Codigo = "warning",
                    Mensagem = "Ocorreu um problema ao cadastrar usuário!"
                };
            }
        }

        [HttpGet]
        [Route("/Sair")]
        public IActionResult Sair()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Index", "Autenticacao");
        }

        [HttpPost]
        public RetornoViewModel RecuperarSenha(AutenticacaoViewModel model)
        {
            try
            {
                if (model.Login != null)
                {

                    var emailEnviado = _usuarioRepository.EmailRecuperaSenha(model.Login);

                    if (emailEnviado) {
                        return new RetornoViewModel
                        {
                            Sucesso = true,
                            Codigo = "success",
                            Titulo = "Sucesso!",
                            Mensagem = "Se o email estiver cadastrado você recebera as instruções nele em breve!"
                        };
                    }
                    else
                        throw new Exception("Ocorreu um erro ao enviar o email!");
                }

                else
                    throw new Exception("Preencha o campo email!");
            }

            catch(Exception e)
            {
                return new RetornoViewModel
                {
                    Sucesso = false,
                    Codigo = "warning",
                    Titulo = "Atenção!",
                    Mensagem = e.Message ?? "Algo deu errado"
                };
            }
        }
    }
}
