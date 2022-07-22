using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherAcquisition.DAL.Context;
using WeatherAcquisition.DAL.Entities.Base;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.DAL.Repositories
{
    public class DbNamedRepository<T> : DbRepository<T>, INamedRepository<T> where T : NamedEntity, 
        new()
    {
        //protected override IQueryable<T> Items => base.Items.OrderBy(i => i.Name);

        public DbNamedRepository(DataDb db) : base(db) {}
        public async Task<bool> ContainsName(string name, 
            CancellationToken cancellationToken = default)
        {
            return await Items.AnyAsync(item => item.Name == name, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<T> GetByName(string name, CancellationToken cancellationToken = default)
        {
            //return await Items.SingleOrDefaultAsync(item => item.Name == name, 
            //    cancellationToken).ConfigureAwait(false);

            return await Items.FirstOrDefaultAsync(item => item.Name == name,
                cancellationToken).ConfigureAwait(false);
        }

        public async Task<T> DeleteByName(string name, 
            CancellationToken cancellationToken = default)
        {
            T item = Set.Local.FirstOrDefault(i => i.Name == name) ?? await Set
                .Select(i => new T { Id = i.Id, Name = i.Name })
                .FirstOrDefaultAsync(i => i.Name == name, cancellationToken)
                .ConfigureAwait(false);

            if (item is null)
            {
                return null;
            }

            return await Delete(item, cancellationToken).ConfigureAwait(false);
        }
    }
}
