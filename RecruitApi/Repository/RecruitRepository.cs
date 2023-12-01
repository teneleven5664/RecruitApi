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


    }
}
