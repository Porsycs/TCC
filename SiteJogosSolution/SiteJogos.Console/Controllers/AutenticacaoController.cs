﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;
using SiteJogos.Core.Services.Interface;

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

        [Route("/")]
        public IActionResult Index()
        {
            //if (!_usuarioRepository.VerificaUsuarios())
            //{
            //    var usuarioMaster = new Usuario()
            //    {
            //        UserName = "master@gmail.com",
            //        Nome = "Master",
            //        Email = "master@gmail.com",
            //    };
            //    _userManager.CreateAsync(usuarioMaster, "Master@2023");
            //}
            return View();
        }

        [Route("/Cadastrar")]
        [AllowAnonymous]
        public IActionResult Cadastrar()
        {
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
                    if (user == null || user.Excluido) throw new Exception("Usuário ou Senha Inválidos");

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
                }

                throw new Exception("Falha ao cadastrar");
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
