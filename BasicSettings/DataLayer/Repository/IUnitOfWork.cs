using BasicSettings.DataLayer.Repository.Repositories.Concrete;

namespace BasicSettings.DataLayer.Repository
{
    public interface IUnitOfWork
    {
        ApplicationDbContext Context { get; }
        IDbConnection DbConnection { get; }
        IDbContextTransaction BeginTransaction();
        Task SaveChangesAsync();
        IDbContextTransaction CurrentTransaction { get; }
        Task ExecuteInTransactionAsync(Func<Task> action);
        void Save();
        Task SaveAsync();
        Task SaveAsync(CancellationToken cancellationToken);
        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
        Task CommitAsync(CancellationToken cancellationToken);
        Task RollbackAsync(CancellationToken cancellationToken);

        #region repositories
        ICustomeIdentityUserRepository CustomeIdentityUserRepository { get; }
        ICacheRepository CacheRepository { get; }
        IHttpContextAccessorCustome HttpContextAccessor { get; }
        IAuthRepository AuthRepository { get; }
        #endregion repositories
    }
}
