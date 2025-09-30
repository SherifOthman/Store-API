
using Dapper;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Interfaces;
using System.Data;

namespace OnlineStore.Infrastructure.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction _transaction;

    public CategoryRepository(IDbConnection connection, IDbTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task<int> AddAsync(Category category)
    {
        var dynamicParams = new DynamicParameters();
        dynamicParams.Add("Id", direction: ParameterDirection.Output);

        await _connection.ExecuteAsync("sp_Category_Add", category, _transaction,
             commandType: CommandType.StoredProcedure);

        return dynamicParams.Get<int>("Id");
    }

    public Task DeleteAsync(int id)
    {
        return _connection.ExecuteAsync("sp_Category_Delete", new { Id = id }, _transaction,
              commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        var result = await _connection.QueryAsync<Category>("sp_Category_GetAll", transaction: _transaction,
                   commandType: CommandType.StoredProcedure);

        return BuildCategoriesTree(result);
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        var result = await _connection.QueryAsync<Category>("sp_Category_GetById", new { Id = id },
              _transaction, commandType: CommandType.StoredProcedure);

        return BuildCategoriesTree(result).FirstOrDefault();
    }

    public Task UpdateAsync(Category category)
    {
        return _connection.ExecuteAsync("sp_Category_Update", category, _transaction,
              commandType: CommandType.StoredProcedure);
    }


    private IEnumerable<Category> BuildCategoriesTree(IEnumerable<Category> categories)
    {
        if (categories.Count() == 0)
            return categories;

        var map = categories.ToDictionary(c => c.Id);

        List<Category> parents = new List<Category>();

        foreach (var category in categories)
        {
            if (category.ParentCategoryId == null)
            {
                parents.Add(category);
            }
            else
            {
                map[category.ParentCategoryId.Value].SubCategories.Add(category);
            }
        }

        return parents;
    }
}
