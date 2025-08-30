using FluentValidation;
using Mapster;
using OnlineStore.Application.Common;
using OnlineStore.Application.Providers;
using OnlineStore.Application.Requests;
using OnlineStore.Application.utils;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Interfaces;
using System.Numerics;

public class UserService : IUserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<SignUpRequest> _validator;

    public UserService(IPasswordHasher passwordHasher,
        IUserRepository userRepository,
        IValidator<SignUpRequest> validator
   )
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<Result<User>> CreateAsync(SignUpRequest request)
    {

        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
            return Result<User>.Fail(validationResult.Errors.ToErrorItemList());

        var existing = await _userRepository.GetByEmailAsync(request.Email);
        if (existing != null)
            return Result<User>.Fail(new ErrorItem
            {
                Field = "Email",
                Message = "Email already in use"
            });

        var user = request.Adapt<User>();
        user.CreatedAt = DateTime.UtcNow;
        user.PasswordHash = _passwordHasher.Hash(request.Password);

        int userId = await _userRepository.AddAsync(user);
        user.Id = userId;

        return Result<User>.Ok(user);
    }

    public bool VerifyPassword(User user, string password)
    {
        return user != null && _passwordHasher.Verify(password, user.PasswordHash);
    }

    public Task<User?> GetByEmailAsync(string email) => _userRepository.GetByEmailAsync(email);

    public async Task<User?> GetByIdAsync(int Id)
    {
        return await _userRepository.GetByIdAsync(Id);
    }
}
