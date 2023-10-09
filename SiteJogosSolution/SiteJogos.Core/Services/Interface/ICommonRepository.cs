using SiteJogos.Core.Entities;

namespace SiteJogos.Core.Services.Interface
{
    public interface ICommonRepository<T> where T : BaseEntity
    {
        List<T> GetAll();
        T GetById(Guid Id);
        T Insert(T item);
        T Update(T item);
        void Delete(Guid Id);
        bool Existe(Guid Id);

    }
}
