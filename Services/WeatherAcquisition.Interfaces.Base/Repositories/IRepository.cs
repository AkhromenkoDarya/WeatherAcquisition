using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.Interfaces.Base.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task<bool> ContainsId(int id);

        Task<bool> Contains(T item);

        Task<int> GetCount();

        Task<IEnumerable<T>> GetAll();

        Task<IEnumerable<T>> Get(int skip, int count);

        Task<IPage<T>> GetPage(int pageIndex, int pageSize);

        //async Task<T> GetById(int id) => (await GetAll()).FirstOrDefault(item => item.Id == id);

        Task<T> GetById(int id);

        Task<T> Add(T item);

        Task<T> Update(T item);

        Task<T> Remove(T item);

        Task<T> RemoveById(int id);
    }

    public interface IPage<T>
    {
        IEnumerable<T> Items { get; }

        int TotalItemCount { get; set; }

        int PageIndex { get; }

        int PageSize { get; }

        int TotalPageCount => (int)Math.Ceiling((double)TotalItemCount / TotalPageCount);
    }
}
