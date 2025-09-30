using FluentValidation;


namespace OnlineStore.Application.Requests;
public class CreateCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }

}

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(4);
    }
}
