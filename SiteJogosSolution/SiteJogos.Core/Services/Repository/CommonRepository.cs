using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SiteJogos.Core.Context;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Services.Interface;

namespace SiteJogos.Core.Services.Repository
{
    public class CommonRepository<T> : ICommonRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        private readonly IConfiguration _configuration;

        public CommonRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            _configuration = configuration;
        }

        public List<T> GetAll()
        {
            var dado = _dbSet.AsNoTracking().ToList();
            return dado;
        }

        public T? GetById(Guid Id)
        {
            var dado = _dbSet.FirstOrDefault(f => f.Id == Id);
            return dado;
        }

        public T Insert(T item)
        {
            try
            {
                _dbContext.Add(item);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return item;
        }

        public T? Update(T item)
        {

            var result = GetById(item.Id);

            if (result != null)
            {
                try
                {
                    _dbContext.Entry(result).CurrentValues.SetValues(item);
                    _dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
                return result;
            }
            else
                return null;
        }

        public void Delete(Guid Id)
        {

            var result = GetById(Id);
            if (result != null)
            {
                try
                {
                    _dbSet.Remove(result);
                    _dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public bool Existe(Guid Id)
        {
            var existe = _dbSet.AsNoTracking().Any(f => f.Id == Id);
            return existe;
        }

        public dynamic DadosEmail()
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
    }
}
