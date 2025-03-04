namespace BasicSettings.DataLayer.Repository
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(ApplicationDbContext dbContext, Appsettings appsettings, IServiceProvider serviceProvider)
        {
            this.Context = dbContext;
            Appsettings = appsettings;
            this._serviceProvider = serviceProvider;
        }

        public ApplicationDbContext Context { get; set; }
        public Appsettings Appsettings { get; }

        public TRepository GetRepository<TRepository>() => _serviceProvider.GetRequiredService<TRepository>();

        #region repositories
        public ICustomeIdentityUserRepository CustomeIdentityUserRepository { get => GetRepository<ICustomeIdentityUserRepository>(); }
        public ICacheRepository CacheRepository { get => GetRepository<ICacheRepository>(); }
        public IHttpContextAccessorCustome HttpContextAccessor { get => GetRepository<IHttpContextAccessorCustome>(); }
        #endregion repositories

        public IDbContextTransaction BeginTransaction() => Context.Database.BeginTransaction();

        public async Task SaveChangesAsync() => await Context.SaveChangesAsync();

        public IDbContextTransaction CurrentTransaction => Context.Database.CurrentTransaction;

        public IDbConnection DbConnection => new SqlConnection(Appsettings.ConnectionStrings.ConnectionString);

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    await action();
                    await CommitAsync().ConfigureAwait(false);
                }
                catch (Exception)
                {
                    await RollbackAsync().ConfigureAwait(false);
                    throw;
                }
            }
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }

        public void Commit()
        {
            Save();
            if (Context.Database.CurrentTransaction != null)
                Context.Database.CurrentTransaction.Commit();
        }

        public void Rollback()
        {
            if (Context.Database.CurrentTransaction != null)
                Context.Database.CurrentTransaction.Rollback();
        }

        public async Task CommitAsync()
        {
            await SaveAsync();
            if (Context.Database.CurrentTransaction != null)
                await Context.Database.CurrentTransaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (Context.Database.CurrentTransaction != null)
                await Context.Database.CurrentTransaction.RollbackAsync();
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await SaveAsync(cancellationToken);
            if (Context.Database.CurrentTransaction != null)
                await Context.Database.CurrentTransaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            if (Context.Database.CurrentTransaction != null)
                await Context.Database.CurrentTransaction.RollbackAsync(cancellationToken);
        }

    }
}
