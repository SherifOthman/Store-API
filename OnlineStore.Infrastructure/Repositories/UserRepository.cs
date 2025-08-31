using Dapper;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Interfaces;
using System.Data;

namespace OnlineStore.Infrastructure.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction _transaction;

    public UserRepository(IDbConnection connection, IDbTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task<int> AddAsync(User user)
    {
        DynamicParameters parameters = new DynamicParameters(user);
        parameters.Add("Id", direction: ParameterDirection.Output);

        await _connection.ExecuteAsync("sp_User_Add",
            parameters, _transaction, commandType: CommandType.StoredProcedure);

        return parameters.Get<int>("Id");
    }
    public Task UpdateAsync(User user)
    {
        return _connection.ExecuteAsync("sp_User_Update",
             user, _transaction, commandType: CommandType.StoredProcedure);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await _connection.QueryFirstOrDefaultAsync<User>("sp_User_GetByEmail",
                new { Email = email }, _transaction, commandType: CommandType.StoredProcedure);

        return user;
    }

    public Task<User?> GetByIdAsync(int Id)
    {
        return _connection.QueryFirstOrDefaultAsync<User>("sp_User_GetById",
              _transaction, commandType: CommandType.StoredProcedure);
    }


}
