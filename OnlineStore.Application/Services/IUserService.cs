using OnlineStore.Application.Common;
using OnlineStore.Application.Requests;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Enums;

public interface IUserService
{
    Task<Result<User>> CreateAsync(SignUpRequest request, RoleValue role= RoleValue.Customer);
    Task<User?> GetByIdAsync(int Id);
    Task<User?> GetByEmailAsync(string email);
    bool VerifyPassword(User user, string password);

}