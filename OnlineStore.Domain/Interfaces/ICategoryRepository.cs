using OnlineStore.Domain.Entities;

namespace OnlineStore.Domain.Interfaces;
public interface ICategoryRepository
{
    public Task<IEnumerable<Category>> GetAllAsync();

    public Task<Category?> GetByIdAsync(int id);

    public Task<int> AddAsync(Category category);

    public Task UpdateAsync(Category category);

    public Task DeleteAsync(int id);

    public Task<bool> IsExists(string Name);
}
