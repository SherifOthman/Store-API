using OnlineStore.Domain.Entities;

namespace OnlineStore.Domain.Interfaces;
public interface IUserRepository
{
    public Task<User?> GetByIdAsync(int Id);

    public Task<User?> GetByEmailAsync(string email);

    public Task<int> AddAsync(User user);

    public Task UpdateAsync(User user);

}
