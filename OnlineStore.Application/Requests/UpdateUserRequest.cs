
using FluentValidation;
using Microsoft.AspNetCore.Http;
using OnlineStore.Application.Common;
using System.Text.Json.Serialization;

namespace OnlineStore.Application.Requests;
public class UpdateUserRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public IFormFile? ImageFile { get; set; }

}

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Length(11)
            .WithMessage("Phone number must be exactly 11 digits");

        RuleFor(x => x.ImageFile).Must(file => file.Length > 0 && file.Length < Constants.MAX_IMAGE_SIZE)
            .WithMessage(Constants.IMAGE_VALIDATE_MESSAGE)
            .When(x => x.ImageFile != null);

    }
}