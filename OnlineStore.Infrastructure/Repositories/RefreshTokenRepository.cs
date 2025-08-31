using Dapper;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Interfaces;
using System.Data;

namespace OnlineStore.Infrastructure.Repositories;
internal class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction _transaction;

    public RefreshTokenRepository(IDbConnection connection, IDbTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task<int> AddAsync(RefreshToken refreshToken)
    {
        var paramters = new DynamicParameters(refreshToken);
        paramters.Add("Id", direction: ParameterDirection.Output);

        await _connection.ExecuteAsync("sp_RefreshToken_Add"
              ,paramters ,_transaction, commandType: CommandType.StoredProcedure);

        int insertedId = paramters.Get<int>("Id");

        return insertedId;
    }

    public Task<RefreshToken?> GetByValueAsync(string value)
    {
        return _connection.QueryFirstOrDefaultAsync<RefreshToken>("sp_RefreshToken_GetByValue",
              value, _transaction, commandType: CommandType.StoredProcedure);
    }

    public Task UpdateAsync(RefreshToken refreshToken)
    {
        return _connection.ExecuteAsync("sp_RefreshToken_Update",
             refreshToken, _transaction, commandType: CommandType.StoredProcedure);
    }
}
