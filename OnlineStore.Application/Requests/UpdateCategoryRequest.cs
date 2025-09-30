using FluentValidation;
using System.Text.Json.Serialization;

namespace OnlineStore.Application.Requests;
public class UpdateCategoryRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }
}

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(c => c.Id)
            .GreaterThan(0);

        RuleFor(c => c.Name).
            MinimumLength(4);

        RuleFor(c => c.ParentCategoryId)
            .GreaterThan(0)
            .When(c=>c.ParentCategoryId is not null);
    }
}
