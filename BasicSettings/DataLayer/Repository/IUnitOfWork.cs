﻿namespace BasicSettings.DataLayer.Repository
{
    public interface IUnitOfWork
    {
        ApplicationDbContext Context { get; }
        IDbConnection DbConnection { get; }
        IDbContextTransaction BeginTransaction();
        Task SaveChangesAsync();
        Task SaveChangesAsync(CancellationToken cancellationToken);
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
        IIdentityUserRepository IdentityUserRepository { get; }
        ISystemTaskRepository SystemTaskRepository { get; }
        IRoleProfilesRepository RoleProfilesRepository { get; }
        #endregion repositories
    }
}
