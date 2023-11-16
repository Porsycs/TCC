using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;
using SiteJogos.Core.Services.Interface;
using System;

namespace Test
{
    public class Login
    {
        private FormCadastroViewModel usuario;
        private readonly UserManager<Usuario> _userManager;

        public Login(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        [SetUp]
        public void Setup()
        {
            var usuarioJson = "{\"type\":null,\"classe\":null,\"id\":null,\"name\":null,\"placeholder\":null,\"Nome\":\"Master\",\"Email\":\"master@historiando.com.br\",\"Senha\":\"Master@2023\",\"ConfirmaSenha\":\"Master@2023\"}";
            usuario = JsonConvert.DeserializeObject<FormCadastroViewModel>(usuarioJson);
        }

        [Test]

        public void LoginUsuario()
        {
            var user = _userManager.FindByNameAsync(usuario.Email).Result;
            Assert.Pass();
        }
    }
}