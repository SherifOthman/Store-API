using OnlineStore.Application.Common;
using OnlineStore.Application.Requests;
using OnlineStore.Application.Responses;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Enums;

public interface IUserService
{
    Task<Result<User>> CreateAsync(SignUpRequest request, RoleValue role = RoleValue.Customer);
    bool VerifyPassword(User user, string password);
    Task<Result<UserResponse>> GetLoggedInUserAsync();
    Task<Result> UpdateLoggedInUserAsync(UpdateUserRequest request);

    Task<Result> ChangePasswordAsync(ChangePasswordRequest request);

}