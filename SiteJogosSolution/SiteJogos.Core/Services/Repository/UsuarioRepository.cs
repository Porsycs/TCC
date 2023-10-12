using Microsoft.EntityFrameworkCore;
using SiteJogos.Core.Context;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace SiteJogos.Core.Services.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        
        public UsuarioRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public IEnumerable<Usuario> GetAll()
        {
            return _dbContext.Usuarios.Where(u => !u.Excluido).AsEnumerable();
        }

        public Usuario GetById(Guid id)
        {
            return _dbContext.Usuarios.SingleOrDefault(t => t.Id == id);
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

        public bool EmailRecuperaSenha(string email)
        {

            if (!IsValidEmail(email))
                throw new Exception("Email em formato inválido!");

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
                mailMessage.Body = $"Email de recuperação de senha teste para: {email}";

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

            catch(Exception)
            {
                return new object();
            }
        }

        public bool VerificaUsuarios()
        {
            return true;
            //return _dbContext.Usuarios.Any();
        }

        private bool IsValidEmail(string email)
        {
            // Use a regular expression to validate the email format
            string emailPattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}
