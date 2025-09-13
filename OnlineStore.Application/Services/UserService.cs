using FluentValidation;
using Mapster;
using OnlineStore.Application.Common;
using OnlineStore.Application.Providers;
using OnlineStore.Application.Requests;
using OnlineStore.Application.utils;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Enums;
using OnlineStore.Domain.Interfaces;
using System.Numerics;

public class UserService : IUserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SignUpRequest> _validator;

    public UserService(IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IValidator<SignUpRequest> validator
   )
    {
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<User>> CreateAsync(SignUpRequest request, RoleValue role = RoleValue.Customer)
    {

        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
            return Result<User>.Fail(validationResult.Errors.ToErrorItemList());

        var existing = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (existing != null)
            return Result<User>.Fail(new ErrorItem
            {
                Field = "Email",
                Message = "Email already in use"
            });

        var user = request.Adapt<User>();
        user.CreatedAt = DateTime.UtcNow;
        user.PasswordHash = _passwordHasher.Hash(request.Password);
        user.Roles = role;

        int userId = await _unitOfWork.Users.AddAsync(user);
        user.Id = userId;

        return Result<User>.Ok(user);
    }

    public bool VerifyPassword(User user, string password)
    {
        return user != null && _passwordHasher.Verify(password, user.PasswordHash);
    }

    public Task<User?> GetByEmailAsync(string email) => _unitOfWork.Users.GetByEmailAsync(email);

    public async Task<User?> GetByIdAsync(int Id)
    {
        return await _unitOfWork.Users.GetByIdAsync(Id);
    }

}
