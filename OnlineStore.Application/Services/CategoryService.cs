
using FluentValidation;
using Mapster;
using OnlineStore.Application.Common;
using OnlineStore.Application.Requests;
using OnlineStore.Application.Responses;
using OnlineStore.Application.utils;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Interfaces;

namespace OnlineStore.Application.Services;
public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCategoryRequest> _createValidator;
    private readonly IValidator<UpdateCategoryRequest> _updateValidator;

    public CategoryService(IUnitOfWork unitOfWork,
        IValidator<CreateCategoryRequest> createValidator,
        IValidator<UpdateCategoryRequest> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<Category>> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var result = _createValidator.Validate(request);
        if (!result.IsValid)
            return Result<Category>.Fail(result.Errors.ToErrorItemList());

        var category = request.Adapt<Category>();
        category.CreatedAt = DateTime.UtcNow;

        var newInd = await _unitOfWork.Categories.AddAsync(category);
        category.Id = newInd;

        return category;
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null)
            return Result.Fail($"Category with {id} was not found");

        if (category.SubCategories.Count > 0)
            return Result.Fail($"This category cannot be removed because off its subcategories, remove them first.");

            await _unitOfWork.Categories.DeleteAsync(id);

        return Result.Ok();
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
    {
        var result = await _unitOfWork.Categories.GetAllAsync();

        return result.Adapt<IEnumerable<CategoryResponse>>();
    }

    public async Task<Result<CategoryResponse>> GetByIdAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null)
            return Result<CategoryResponse>.Fail($"Category with id {id} was not found");

        return category.Adapt<CategoryResponse>();
    }

    public async Task<Result> UpdateAsync(UpdateCategoryRequest request)
    {
        var result = _updateValidator.Validate(request);
        if (!result.IsValid)
            return Result.Fail(result.Errors.ToErrorItemList());

        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);

        if (category == null)
            return Result.Fail($"Category with id {request.Id} was not found");

        request.Adapt(category);

        await _unitOfWork.Categories.UpdateAsync(category);

        return Result.Ok();
    }
}
