using RecruitApi.Data;
using RecruitApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using RecruitApi.Repository;
using System.Collections.Generic;

namespace Recruit.Repository
{
    public class RecruitRepository : IRecruitRepository
    {
        private readonly ApplicationDbContext _db;

        public RecruitRepository(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1) where T : class
        {
            IQueryable<T> query = _db.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (pageSize > 0)
            {
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T?> GetAsync<T>(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null) where T : class
        {
            IQueryable<T> query = _db.Set<T>();

            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.FirstOrDefaultAsync();
        }


        public async Task CreateAsync<T>(T entity) where T : class
        {
            var dbset = _db.Set<T>();
            await dbset.AddAsync(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync<T>(T entity) where T : class
        {
            var dbset = _db.Set<T>();
            dbset.Remove(entity);
            await SaveAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            var dbset = _db.Set<T>();
            dbset.Update(entity);
            await SaveAsync();
        }



    }
}
