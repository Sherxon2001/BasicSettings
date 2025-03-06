using BasicSettings.DataLayer.Repository.Repositories.Concrete;

namespace BasicSettings.DataLayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnitOfWork
        (
            ApplicationDbContext dbContext,
            Appsettings appsettings,
            IServiceProvider serviceProvider,
            IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAccessor
        )
        {
            this.Context = dbContext;
            Appsettings = appsettings;
            this._serviceProvider = serviceProvider;
            this._memoryCache = memoryCache;
            this._httpContextAccessor = httpContextAccessor;
        }



        #region repositories
        private IRoleRepository _roleRepository;
        public IRoleRepository RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new RoleRepository(this);
                return _roleRepository;

            }
        }

        private ISystemTaskRepository _systemTaskRepository;
        public ISystemTaskRepository SystemTaskRepository
        {
            get
            {
                if (_systemTaskRepository == null)
                    _systemTaskRepository = new SystemTaskRepository(this);
                return _systemTaskRepository;
            }
        }

        private IApplicantRoleRepository _applicantRoleRepository;
        public IApplicantRoleRepository ApplicantRoleRepository
        {
            get
            {
                if (_applicantRoleRepository == null)
                    _applicantRoleRepository = new ApplicantRoleRepository(this);
                return _applicantRoleRepository;
            }
        }

        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(this);
                return _userRepository;
            }
        }

        private ICacheRepository _cacheRepository;
        public ICacheRepository CacheRepository
        {
            get
            {
                if (_cacheRepository == null)
                    _cacheRepository = new CacheRepository(_memoryCache);
                return _cacheRepository;
            }
        }

        private IHttpContextAccessorCustome _httpContextAccessorCustome;
        public IHttpContextAccessorCustome HttpContextAccessor
        {
            get
            {
                if (_httpContextAccessorCustome == null)
                    _httpContextAccessorCustome = new HttpContextAccessorCustome(_httpContextAccessor);
                return _httpContextAccessorCustome;
            }
        }

        private IRoleProfilesRepository _roleProfilesRepository;
        public IRoleProfilesRepository RoleProfilesRepository
        {
            get
            {
                if (_roleProfilesRepository == null)
                    _roleProfilesRepository = new RoleProfilesRepository(this);
                return _roleProfilesRepository;
            }
        }

        private IUsersRolesRepository _usersRolesRepository;
        public IUsersRolesRepository UsersRolesRepository
        {
            get
            {
                if (_usersRolesRepository == null)
                    _usersRolesRepository = new UsersRolesRepository(this);
                return _usersRolesRepository;
            }
        }
        #endregion repositories




        public ApplicationDbContext Context { get; set; }
        public Appsettings Appsettings { get; }

        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }

        public IDbContextTransaction CurrentTransaction
        {
            get
            {
                return Context.Database.CurrentTransaction;
            }
        }

        public IDbConnection DbConnection
        {
            get
            {
                var connection = new SqlConnection(Appsettings.ConnectionStrings.DefaultConnection);
                return connection;
            }
        }

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
