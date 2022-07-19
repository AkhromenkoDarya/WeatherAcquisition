using System.Threading;
using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.Interfaces.Base.Repositories
{
    public interface INamedRepository<T> : IRepository<T> where T : INamedEntity
    {
        Task<bool> ContainsName(string name, CancellationToken cancellationToken = default);

        Task<T> GetByName(string name, CancellationToken cancellationToken = default);

        Task<T> RemoveByName(string name, CancellationToken cancellationToken = default);
    }
}
