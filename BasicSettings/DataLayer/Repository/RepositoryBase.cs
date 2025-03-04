namespace BasicSettings.DataLayer.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IDbConnection DbConnection => _unitOfWork.DbConnection;

        public virtual void Add(T entity) => _unitOfWork.Context.Set<T>().Add(entity);
        public async Task AddAsycn(T entity) => await _unitOfWork.Context.Set<T>().AddAsync(entity);

        public virtual void AddRange(IEnumerable<T> entities) => _unitOfWork.Context.Set<T>().AddRange(entities);

        public Task AddRangeAsync(IEnumerable<T> entities) => _unitOfWork.Context.Set<T>().AddRangeAsync(entities);

        public T? FirstOrDefault(Expression<Func<T, bool>>? predicate = null, bool tracking = true, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _unitOfWork.Context.Set<T>();
            if (!tracking)
                query = query.AsNoTracking();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            return predicate != null ? query.FirstOrDefault(predicate) : query.FirstOrDefault();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, bool tracking = true, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _unitOfWork.Context.Set<T>();
            if (!tracking)
                query = query.AsNoTracking();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            return await (predicate != null ? query.FirstOrDefaultAsync(predicate) : query.FirstOrDefaultAsync());
        }

        public void Remove(T entity) => _unitOfWork.Context.Set<T>().Remove(entity);

        public void RemoveRange(IEnumerable<T> entities) => _unitOfWork.Context.Set<T>().RemoveRange(entities);

        public void Update(T entity) => _unitOfWork.Context.Set<T>().Update(entity);

        public void UpdateRange(IEnumerable<T> entities) => _unitOfWork.Context.Set<T>().UpdateRange(entities);

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool tracking = true, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _unitOfWork.Context.Set<T>().Where(predicate);
            if (!tracking)
                query = query.AsNoTracking();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}

