using OnlineStore.Application.Common;
using OnlineStore.Application.Requests;
using OnlineStore.Domain.Entities;

public interface IUserService
{
    Task<Result<User>> CreateAsync(SignUpRequest request);
    Task<User?> GetByIdAsync(int Id);
    Task<User?> GetByEmailAsync(string email);
    bool VerifyPassword(User user, string password);

}