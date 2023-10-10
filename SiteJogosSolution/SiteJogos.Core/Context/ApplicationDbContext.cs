using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SiteJogos.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Context
{
    public class ApplicationDbContext : IdentityDbContext<Usuario, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        //public DbSet<Template> Templates { get; set; }
        //public DbSet<TemplateJogoForca> TemplateJogoForca { get; set; }
        //public DbSet<TemplateJogoMemoria> TemplateJogoMemoria { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Identity
            base.OnModelCreating(builder);
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                if (entity.GetTableName() == "AspNetUsers")
                {
                    entity.SetTableName("Usuario");
                }
                if (entity.GetTableName() == "AspNetUserClaims")
                {
                    entity.SetTableName("UsuarioAcesso");
                }
                if (entity.GetTableName() == "AspNetRoleClaims")
                {
                    entity.SetTableName("FuncaoAcesso");
                }
                if (entity.GetTableName() == "AspNetUserLogins")
                {
                    entity.SetTableName("UsuarioLogin");
                }
                if (entity.GetTableName() == "AspNetRoles")
                {
                    entity.SetTableName("Funcao");
                }
                if (entity.GetTableName() == "AspNetUserRoles")
                {
                    entity.SetTableName("UsuarioFuncao");
                }
                if (entity.GetTableName() == "AspNetUserTokens")
                {
                    entity.SetTableName("UsuarioToken");
                }
            }

            #endregion
        }
    }
}
