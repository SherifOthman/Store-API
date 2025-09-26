using FluentValidation;
using Microsoft.AspNetCore.Http;
using OnlineStore.Application.Common;

namespace OnlineStore.Application.Requests;

public class SignUpRequest
{
   public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public IFormFile? ImageFile { get; set; }
}


public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required");

        RuleFor(x => x.LastName)
         .NotEmpty().WithMessage("FirstName is required");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required")
            .Length(11).WithMessage("Phone number should be 11 digits");

        RuleFor(x => x.ImageFile).Must(file => file.Length > 0 && file.Length < Constants.MAX_IMAGE_SIZE)
            .WithMessage(Constants.IMAGE_VALIDATE_MESSAGE)
            .When(file => file is not null);
    
    }
}
