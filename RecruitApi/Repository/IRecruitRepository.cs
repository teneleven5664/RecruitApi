using Microsoft.EntityFrameworkCore;
using RecruitApi.Models;
using System.Linq.Expressions;

namespace RecruitApi.Repository
{
    public interface IRecruitRepository
    {
        Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1) where T : class;
        Task<T?> GetAsync<T>(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null) where T : class;
        Task CreateAsync<T>(T entity) where T : class;
        Task SaveAsync();
        Task RemoveAsync<T>(T entity) where T : class;
        Task UpdateAsync<T>(T entity) where T : class;
    }
}
