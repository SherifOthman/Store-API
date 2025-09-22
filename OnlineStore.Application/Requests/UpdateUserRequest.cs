
using FluentValidation;
using System.Text.Json.Serialization;

namespace OnlineStore.Application.Requests;
public class UpdateUserRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    
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

    }
}