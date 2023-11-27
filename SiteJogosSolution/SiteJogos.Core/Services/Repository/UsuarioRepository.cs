using Microsoft.EntityFrameworkCore;
using SiteJogos.Core.Context;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Services.Interface;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using SiteJogos.Core.Entities.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Buffers.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiteJogos.Core.Migrations;
using Midia = SiteJogos.Core.Entities.Midia;

namespace SiteJogos.Core.Services.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly UserManager<Usuario> _userManager;
        private readonly IMidiaRepository _midiaRepository;

        public UsuarioRepository(ApplicationDbContext dbContext, IConfiguration configuration, UserManager<Usuario> userManager, IMidiaRepository midiaRepository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _userManager = userManager;
            _midiaRepository = midiaRepository;
        }

        public IEnumerable<Usuario> GetAll()
        {
            return _dbContext.Usuarios.AsNoTracking().Where(u => !u.Excluido).AsEnumerable();
        }

        public Usuario GetById(Guid id)
        {
            return _dbContext.Usuarios.AsNoTracking().SingleOrDefault(t => t.Id == id) ?? new Usuario();
        }

        public void Delete(Guid Id)
        {
            var Entity = _dbContext.Usuarios.SingleOrDefault(sd => sd.Id == Id) ?? throw new Exception("Registro não encontrado");

            Entity.Excluido = false;
            Entity.Alteracao = DateTime.Now;

            _dbContext.Entry(Entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void InsertOrReplace(Usuario usuario)
        {
            try
            {
                var Entity = _dbContext.Usuarios.AsNoTracking().SingleOrDefault(e => e.Id == usuario.Id);

                usuario.Excluido = false;

                if (Entity == null)
                {
                    usuario.NormalizedUserName = usuario.NormalizedUserName.ToUpper();
                    usuario.UserName = usuario.UserName;
                    usuario.PasswordHash = usuario.PasswordHash;
                    usuario.NormalizedUserName = usuario.UserName;
                    usuario.Alteracao = DateTime.Now;
                    _dbContext.Usuarios.Add(usuario);
                }
                //se encoutrou
                else
                {
                    usuario.NormalizedUserName = usuario.NormalizedUserName.ToUpper();
                    usuario.UserName = usuario.UserName;
                    usuario.NormalizedUserName = usuario.UserName;
                    usuario.PasswordHash = usuario.PasswordHash;
                    usuario.Alteracao = DateTime.Now;
                    _dbContext.Entry(usuario).State = EntityState.Modified;
                }
                _dbContext.SaveChanges();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool VerificaUsuarios(Guid Id, string email)
        {
            return _dbContext.Usuarios.Any(a => a.Id != Id && a.Email == email);
        }

        private static bool IsValidEmail(string email)
        {
            string emailPattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
            return Regex.IsMatch(email, emailPattern);
        }

        public async Task<bool> EmailRecuperaSenha(string email, Usuario usuario, string token, string link)
        {

            if (!IsValidEmail(email))
                return false;

            if (usuario == null)
                return true;

            dynamic dados = DadosEmail();

            using (SmtpClient smtpClient = new SmtpClient(dados.smtpServer, dados.smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(dados.smtpUsername, dados.smtpPassword);
                smtpClient.EnableSsl = true;

                // Create a new MailMessage
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(dados.smtpUsername);
                mailMessage.To.Add(email);
                mailMessage.Subject = "Recuperação de senha";
                mailMessage.Body = $"Clique no link abaixo para trocar sua senha: " +
                    $"{link}";

                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        private dynamic DadosEmail()
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");
                var smtpServer = emailSettings["SmtpServer"];
                var smtpPort = int.Parse(emailSettings["SmtpPort"]);
                var smtpUsername = emailSettings["SmtpUsername"];
                var smtpPassword = emailSettings["SmtpPassword"];
                var emailSettingsObject = new
                {
                    smtpServer,
                    smtpPort,
                    smtpUsername,
                    smtpPassword
                };

                return emailSettingsObject;
            }

            catch (Exception)
            {
                return new object();
            }
        }

        public Usuario LoginGoogle(IList<GoogleClaims> claims)
        {
            try 
            {
                Usuario usuario = null;

                var result = false;
                var email = claims[4]?.Value;
                var foto = claims[5]?.Value;
                var nomeInteiro = claims[1]?.Value;

                var usuarioCadastrado = _dbContext.Usuarios.FirstOrDefault(f => f.UserName == email);
                if (usuarioCadastrado != null)
                {
                    var midiaAtualizada = _dbContext.Midias.FirstOrDefault(f => f.Id == usuarioCadastrado.MidiaId);
                    string base64Data = UrlToBase64Async(foto);

                    if (midiaAtualizada == null)
                    {
                        var midia = new Midia()
                        {
                            UsuarioInclusaoId = _dbContext.Usuarios.First().Id,
                            Arquivo = Convert.FromBase64String(base64Data),
                            Nome = "Foto.jpg",
                            Url = foto,
                        };
                        _midiaRepository.Insert(midia);
                        usuarioCadastrado.MidiaId = midia.Id;

                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        midiaAtualizada.Arquivo = Convert.FromBase64String(base64Data);
                        midiaAtualizada.Url = foto;
                    }
                    _dbContext.SaveChanges();


                    return usuarioCadastrado;
                }

                else
                {
                    string base64Data = UrlToBase64Async(foto);
                    Midia midia = null;
                    if (!string.IsNullOrEmpty(base64Data))
                    {
                        midia = new Midia()
                        {
                            UsuarioInclusaoId = _dbContext.Usuarios.First().Id,
                            Arquivo = Convert.FromBase64String(base64Data),
                            Nome = "Foto.jpg",
                            Url = foto,
                        };
                        _midiaRepository.Insert(midia);
                    }

                    if (!string.IsNullOrEmpty(email))
                    {
                        usuario = new Usuario
                        {
                            Nome = nomeInteiro,
                            UserName = email,
                            Email = email,
                            MidiaId = midia?.Id ?? null,
                        };
                        result = _userManager.CreateAsync(usuario, GenerateRandomPassword(12)).Result.Succeeded;
                    }
                    else
                        return usuario;
                }

                return usuario;
            }
            catch
            {
                return new Usuario();
            }
        }

        private static string UrlToBase64Async(string url)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    byte[] imageBytes = httpClient.GetByteArrayAsync(url).Result;

                    string base64Data = Convert.ToBase64String(imageBytes);

                    return base64Data;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-+=<>?";

            Random random = new Random();

            string password = new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return password;
        }
    }
}
