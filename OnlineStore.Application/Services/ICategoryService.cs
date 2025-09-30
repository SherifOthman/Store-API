using OnlineStore.Application.Common;
using OnlineStore.Application.Requests;
using OnlineStore.Application.Responses;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Services;
public interface ICategoryService
{
    Task<Result<Category>> CreateCategoryAsync(CreateCategoryRequest request);
    Task<IEnumerable<CategoryResponse>> GetAllAsync();
    Task<Result<CategoryResponse>> GetByIdAsync(int id);
    Task<Result> UpdateAsync(UpdateCategoryRequest request);
    Task<Result> DeleteAsync(int id);
}
