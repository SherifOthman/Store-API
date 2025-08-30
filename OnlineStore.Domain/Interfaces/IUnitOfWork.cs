using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain.Interfaces;
public interface IUnitOfWork : IDisposable
{
     IRefreshTokenRepository RefreshTokens { get; }
     IUserRepository Users { get; }
    void BeginTransaction();
    void Commit();
    void Rollback();

}
