using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common;
using OnlineStore.Application.Providers;
using OnlineStore.Application.Requests;
using OnlineStore.Application.Responses;
using OnlineStore.Application.Services;
using OnlineStore.Application.utils;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Enums;
using OnlineStore.Domain.Interfaces;
using System.Numerics;

public class UserService : IUserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SignUpRequest> _signinValidator;
    private readonly IValidator<UpdateUserRequest> _updateValidator;
    private readonly ILoggedInUser _loggedInUser;
    private readonly IFileService _fileService;
    private readonly ILogger<UserService> _logger;
    private readonly IFileUrlBuilder _fileUrlBuilder;

    public UserService(IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IValidator<SignUpRequest> signinValidator,
        IValidator<UpdateUserRequest> updateValidator,
        ILoggedInUser loggedInUser,
        IFileService fileService,
        ILogger<UserService> logger,
        IFileUrlBuilder fileUrlBuilder

   )
    {
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _signinValidator = signinValidator;
        _updateValidator = updateValidator;
        _loggedInUser = loggedInUser;
        _fileService = fileService;
        _logger = logger;
        _fileUrlBuilder = fileUrlBuilder;
    }

    public async Task<Result<User>> CreateAsync(SignUpRequest request, RoleValue role = RoleValue.Customer)
    {

        var validationResult = _signinValidator.Validate(request);

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

        if (request.ImageFile != null)
        {
            try
            {
                var newImage = await _fileService.SaveFileAsync(request.ImageFile);
                user.AvatarUrl = newImage;
            }
            catch (Exception err)
            {
                _logger.LogError("Unable to handle saving user image: {v}", err);
                return Result<User>.Fail("Something went wrong, while saving the image");
            }
        }

        user.CreatedAt = DateTime.UtcNow;
        user.PasswordHash = _passwordHasher.Hash(request.Password);
        user.Role = role;

        int userId = await _unitOfWork.Users.AddAsync(user);
        user.Id = userId;

        return Result<User>.Ok(user);
    }

    public bool VerifyPassword(User user, string password)
    {
        return user != null && _passwordHasher.Verify(password, user.PasswordHash);
    }

    public async Task<Result> UpdateLoggedInUserAsync(UpdateUserRequest request)
    {

        int? userId = _loggedInUser.GetUserId();

        if (userId == null)
            return Result<UserResponse>.Fail("No user logged in.");

        var validationResult = _updateValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            return Result.Fail(validationResult.Errors.ToErrorItemList());
        }

        var user = await _unitOfWork.Users.GetByIdAsync(userId.Value);

        if (user == null)
            return Result.Fail("User was not found");

        if (request.ImageFile != null)
        {
            try
            {
                var oldImage = user.AvatarUrl;
                var newImage = await _fileService.SaveFileAsync(request.ImageFile);
                if (!string.IsNullOrEmpty(oldImage))
                {
                    _fileService.DeleteFile(oldImage);
                }
                user.AvatarUrl = newImage;
            }
            catch (Exception err)
            {
                _logger.LogError("Unable to handle saving user image: {v}", err);
                return Result.Fail("Something went wrong, while saving the image");
            }
        }

        request.Adapt(user);


        await _unitOfWork.Users.UpdateAsync(user);

        return Result.Ok();

    }

    public async Task<Result<UserResponse>> GetLoggedInUserAsync()
    {
        int? userId = _loggedInUser.GetUserId();

        if (userId == null)
            return Result<UserResponse>.Fail("No user logged in.");

        var user = await _unitOfWork.Users.GetByIdAsync(userId.Value);

        if (user != null && !string.IsNullOrEmpty(user.AvatarUrl))
            user.AvatarUrl = _fileUrlBuilder.BuildFileUrl(user.AvatarUrl);

        var userResponse = user.Adapt<UserResponse>();
        userResponse.Role = user!.Role.ToString();

        return user.Adapt<UserResponse>();
    }

    public async Task<Result> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var userId = _loggedInUser.GetUserId();
        if (userId == null)
            return Result.Fail("No user logged in.");

        var user = await _unitOfWork.Users.GetByIdAsync(userId.Value);
        if (user == null)
            return Result.Fail("User was not found.");

        _passwordHasher.Hash(request.NewPassword);
        if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
        {
            return Result.Fail(new ErrorItem
            {
                Field = nameof(request.CurrentPassword),
                Message = "The current password is incorrect."
            });
        }

        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
        await _unitOfWork.Users.UpdateAsync(user);

        return Result.Ok();
    }


}
