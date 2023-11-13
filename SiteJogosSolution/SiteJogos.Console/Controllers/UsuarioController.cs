﻿using Microsoft.AspNetCore.Authorization;
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

        public UsuarioController(UserManager<Usuario> userManager, IUsuarioRepository usuarioRepository)
        {
            _userManager = userManager;
            _usuarioRepository = usuarioRepository;
        }

        [HttpPut]
        [Authorize]
        public RetornoViewModel Put(string values, string? fileName = null, string? base64 = null)
        {
            try
            {
                var usuario = _userManager.GetUserAsync(User).Result;
                JsonConvert.PopulateObject(values, usuario);
                _usuarioRepository.InsertOrReplace(usuario);

                if(!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(base64))
                {

                }

                return new RetornoViewModel
                {
                    Sucesso = true,
                    Codigo = "success",
                    Titulo = "Sucesso!",
                    Mensagem = "Usuário cadastrado com sucesso",
                };
            }
            catch(Exception e) 
            {
                return new RetornoViewModel
                {
                    Sucesso = false,
                    Codigo = "success",
                    Titulo = "Erro ao salvar",
                    Mensagem = "Usuário cadastrado com sucesso",
                };
            }
        }
    }
}
