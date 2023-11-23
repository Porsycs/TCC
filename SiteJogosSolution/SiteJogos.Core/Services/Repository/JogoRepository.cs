using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SiteJogos.Core.Context;
using SiteJogos.Core.Entities;
using SiteJogos.Core.Entities.ViewModel;
using SiteJogos.Core.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteJogos.Core.Services.Repository
{
    public class JogoRepository : CommonRepository<Jogo>, IJogoRepository
    {
        public JogoRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration) { }

        public IEnumerable<Jogo> GetByUsuarioId(Guid usuarioId)
        {
            return _dbContext.Jogos.Where(w => !w.Excluido && w.UsuarioInclusaoId == usuarioId);
        }

        public RetornoViewModel InsereJogoDaMemoria(Jogo jogo, List<JogoDaMemoriaMidia> midias)
        {
            try
            {
                var jogoExistente = _dbContext.Jogos.AsNoTracking().FirstOrDefault(f => f.Id == jogo.Id);
                if (jogoExistente == null)
                {
                    _dbContext.Jogos.Add(jogo);
                }
                else
                {
                    jogo.Alteracao = DateTime.Now;
                    _dbContext.Jogos.Update(jogo);
                }

                var midiasExistentes = _dbContext.JogoDaMemoriaMidias.AsNoTracking().Where(w => w.JogoId == jogo.Id).ToList();
                foreach (var m in midias)
                {
                    m.Jogo = null;
                    if (!midiasExistentes.Any(a => a.Id == m.Id))
                    {
                        _dbContext.JogoDaMemoriaMidias.Add(m);
                    }
                    else
                    {
                        _dbContext.JogoDaMemoriaMidias.Update(m);
                    }
                }

                foreach (var m in midiasExistentes.Where(w => !midias.Any(a => a.Id == w.Id)).ToList())
                {
                    m.Excluido = true;
                    m.Alteracao = DateTime.Now;
                    _dbContext.JogoDaMemoriaMidias.Update(m);
                }

                _dbContext.SaveChanges();

                return new RetornoViewModel()
                {
                    Sucesso = true,
                    Mensagem = "Jogo inserido com sucesso"
                };
            }
            catch (Exception e)
            {
                return new RetornoViewModel()
                {
                    Sucesso = false,
                    Mensagem = "Erro: " + e.Message
                };
            }
        }
    }
}
