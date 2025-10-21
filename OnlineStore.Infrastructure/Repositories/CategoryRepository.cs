
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
        dynamicParams.Add("@Name", category.Name, DbType.String, size: 100); // specify size
        dynamicParams.Add("@ParentCategoryId", category.ParentCategoryId, DbType.Int32);
        dynamicParams.Add("@CreatedAt", category.CreatedAt, DbType.DateTime);
        dynamicParams.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await _connection.ExecuteAsync("sp_Category_Add", param: dynamicParams, _transaction,
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

        return BuildSubCategoryTree(result);
    }

    public Task<bool> IsExists(string Name)
    {
        return _connection.ExecuteScalarAsync<bool>("sp_Category_IsExists", new { Name },
              _transaction, commandType: CommandType.StoredProcedure);
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

    private Category? BuildSubCategoryTree(IEnumerable<Category> categories)
    {
        if (categories.Count() == 0)
            return null;

        var map = categories.ToDictionary(c => c.Id);
        var parentCategory = categories.First();

        foreach (var category in categories.Skip(1))
        {
            map[category.ParentCategoryId!.Value].SubCategories.Add(category);
        }

        return parentCategory;
    }
}
