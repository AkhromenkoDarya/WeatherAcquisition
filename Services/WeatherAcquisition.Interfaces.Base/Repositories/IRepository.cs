using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.Interfaces.Base.Repositories
{
    //TODO: Добавить методы получения нескольких первых и последних элементов репозитория.
    public interface IRepository<T> where T : IEntity
    {
        //async Task<bool> ContainsId(int id, CancellationToken cancellationToken = default) =>
        //await GetById(id, cancellationToken) is not null;

        Task<bool> ContainsId(int id, CancellationToken cancellationToken = default);

        Task<bool> Contains(T item, CancellationToken cancellationToken = default);

        Task<int> GetCount(CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> Get(int skip, int count, 
            CancellationToken cancellationToken = default);

        Task<IPage<T>> GetPage(int pageIndex, int pageSize, 
            CancellationToken cancellationToken = default);

        //async Task<T> GetById(int id, CancellationToken cancellationToken = default) =>
        //(await GetAll(cancellationToken)).FirstOrDefault(item => item.Id == id);

        Task<T> GetById(int id, CancellationToken cancellationToken = default);

        Task<T> Add(T item, CancellationToken cancellationToken = default);

        Task<T> Update(T item, CancellationToken cancellationToken = default);

        Task<T> Delete(T item, CancellationToken cancellationToken = default);

        Task<T> DeleteById(int id, CancellationToken cancellationToken = default);
    }

    public interface IPage<out T>
    {
        IEnumerable<T> Items { get; }

        int TotalItemCount { get; }

        int PageIndex { get; }

        int PageSize { get; }

        int TotalPageCount { get; }
    }
}
