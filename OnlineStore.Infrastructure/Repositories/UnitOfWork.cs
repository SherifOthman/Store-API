using Microsoft.Data.SqlClient;
using OnlineStore.Domain.Interfaces;
using System.Data;
using System.Transactions;

namespace OnlineStore.Infrastructure.Repositories;
internal class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _connection;

    private IDbTransaction _transaction;

    public UnitOfWork(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
        _connection.Open();
        _transaction = null!;
    }
    public IUserRepository Users => field ??
        new UserRepository(_connection,_transaction);
    public IRefreshTokenRepository RefreshTokens => field ??
        new RefreshTokenRepository(_connection, _transaction);

    public void BeginTransaction()
    {
        _transaction = _connection .BeginTransaction();
    }

    public void Commit()
    {
        _transaction?.Commit();
        _transaction?.Dispose();
        _transaction = null!;
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null!;
    }

    public void Dispose()
    {

        _transaction?.Dispose();
        _connection?.Dispose();
    }
}
