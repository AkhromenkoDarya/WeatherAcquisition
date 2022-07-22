using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherAcquisition.DAL.Context;
using WeatherAcquisition.DAL.Entities.Base;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.DAL.Repositories
{
    public class DbRepository<T> : IRepository<T> where T : Entity, new()
    {
        private readonly DataDb _db;

        protected DbSet<T> Set { get; }

        protected virtual IQueryable<T> Items => Set;

        public bool AutoSaveChanges { get; set; } = true;

        public DbRepository(DataDb db)
        {
            _db = db;
            Set = _db.Set<T>();
        }

        public async Task<bool> ContainsId(int id, CancellationToken cancellationToken = default)
        {
            return await Items.AnyAsync(item => item.Id == id, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> Contains(T item, CancellationToken cancellationToken = default)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return await Items.AnyAsync(i => i.Id == item.Id, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> GetCount(CancellationToken cancellationToken = default)
        {
            return await Items.CountAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default)
        {
            return await Items.ToArrayAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> Get(int skip, int count, 
            CancellationToken cancellationToken = default)
        {
            //return await Items
            //    .Skip(skip)
            //    .Take(count)
            //    .ToArrayAsync(cancellationToken);

            if (count <= 0 || count > Items.Count() - skip)
            {
                return Enumerable.Empty<T>();
            }

            //IQueryable<T> query = Items.OrderBy(i => i.Id);

            IQueryable<T> query = Items switch
            {
                IOrderedQueryable<T> orderedQueryable => orderedQueryable,
                { } q => q.OrderBy(i => i.Id)
            };

            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            return await query.Take(count).ToArrayAsync(cancellationToken).ConfigureAwait(false);
        }

        protected record Page(IEnumerable<T> Items, int TotalItemCount, int PageIndex,
            int PageSize) : IPage<T>
        {
            public int TotalPageCount => (int)Math.Ceiling((double)TotalItemCount / PageSize);
        }

        public async Task<IPage<T>> GetPage(int pageIndex, int pageSize, 
            CancellationToken cancellationToken = default)
        {
            //if (pageSize <= 0)
            //{
            //    return new Page(Enumerable.Empty<T>(), pageSize, pageIndex, pageSize);
            //}

            if (pageSize <= 0)
            { 
                return new Page(Enumerable.Empty<T>(), await GetCount(cancellationToken)
                    .ConfigureAwait(false), pageIndex, pageSize);
            }

            IQueryable<T> query = Items;
            int totalItemCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

            if (totalItemCount == 0)
            {
                return new Page(Enumerable.Empty<T>(), 0, pageIndex, pageSize);
            }

            if (query is not IOrderedQueryable<T>)
            {
                query = query.OrderBy(item => item.Id);
            }

            if (pageIndex > 0)
            {
                query = query.Skip(pageIndex * pageSize);
            }

            query = query.Take(pageSize);
            T[] items = await query.ToArrayAsync(cancellationToken).ConfigureAwait(false);

            return new Page(items, totalItemCount, pageIndex, pageSize);
        }

        public async Task<T> GetById(int id, CancellationToken cancellationToken = default)
        {
            //return await Items.FirstOrDefaultAsync(item => item.Id == id, 
            //    cancellationToken).ConfigureAwait(false);

            //return await Items.SingleOrDefaultAsync(item => item.Id == id,
            //    cancellationToken).ConfigureAwait(false);

            return Items switch
            {
                DbSet<T> set => await set.FindAsync(new object[] { id }, cancellationToken)
                    .ConfigureAwait(false),
                { } items => await items.SingleOrDefaultAsync(item => 
                        item.Id == id, cancellationToken)
                    .ConfigureAwait(false),
                _ => throw new InvalidOperationException("Error in determining the data source")
            };
        }

        public async Task<T> Add(T item, CancellationToken cancellationToken = default)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            //Set.Add(item);

            //_db.Entry(item).State = EntityState.Added;

            await _db.AddAsync(item, cancellationToken).ConfigureAwait(false);
            
            //await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            if (AutoSaveChanges)
            {
                await SaveChanges(cancellationToken).ConfigureAwait(false);
            }

            return item;
        }

        public async Task<T> Update(T item, CancellationToken cancellationToken = default)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            //Set.Update(item);

            //_db.Entry(item).State = EntityState.Modified;

            _db.Update(item);

            //await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            if (AutoSaveChanges)
            {
                await SaveChanges(cancellationToken).ConfigureAwait(false);
            }

            return item;
        }

        public async Task<T> Delete(T item, CancellationToken cancellationToken = default)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (!(await ContainsId(item.Id, cancellationToken)))
            {
                return null;
            }

            //Set.Remove(item);

            //_db.Entry(item).State = EntityState.Deleted;

            _db.Remove(item);

            //await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            if (AutoSaveChanges)
            {
                await SaveChanges(cancellationToken).ConfigureAwait(false);
            }

            return item;
        }

        public async Task<T> DeleteById(int id, CancellationToken cancellationToken = default)
        {
            T item = Set.Local.FirstOrDefault(i => i.Id == id) ?? await Set
                .Select(i => new T { Id = i.Id })
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken)
                .ConfigureAwait(false);

            if (item is null)
            {
                return null;
            }

            return await Delete(item, cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> SaveChanges(CancellationToken cancellationToken = default)
        {
            return await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
