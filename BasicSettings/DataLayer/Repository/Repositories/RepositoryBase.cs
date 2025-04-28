using BasicSettings.Extensions;

namespace BasicSettings.DataLayer.Repository.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IDbConnection DbConnection => _unitOfWork.DbConnection;

        public async Task<IPageCollection<T>> GetList(Expression<Func<T, bool>> expression, int pageNumber = 1, int pageSize = 5, params Expression<Func<T, object>>[] includes)
        {
            const int DefaultPageNumber = 1;
            const int DefaultPageSize = 5;
            const int MinPageSize = 1;
            const int MaxPageSize = 5;

            pageNumber = pageNumber < DefaultPageNumber ? DefaultPageNumber : pageNumber;
            pageSize = pageSize < MinPageSize ? DefaultPageSize : pageSize;
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            IQueryable<T> query = _unitOfWork.Context.Set<T>().AsNoTracking().Where(expression);
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            var response = await query.AsNoTracking().ToPagedListAsync(pageNumber, pageSize);
            return response;
        }

        public virtual void Add(T entity) => _unitOfWork.Context.Set<T>().Add(entity);
        public async Task AddAsycn(T entity) => await _unitOfWork.Context.Set<T>().AddAsync(entity);

        public virtual void AddRange(IEnumerable<T> entities) => _unitOfWork.Context.Set<T>().AddRange(entities);

        public async Task AddRangeAsync(IEnumerable<T> entities) => await _unitOfWork.Context.Set<T>().AddRangeAsync(entities);
        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken) => await _unitOfWork.Context.Set<T>().AddRangeAsync(entities, cancellationToken);

        public T? FirstOrDefault(Expression<Func<T, bool>>? predicate = null, bool tracking = false, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _unitOfWork.Context.Set<T>();
            if (!tracking)
                query = query.AsNoTracking();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            return predicate != null ? query.FirstOrDefault(predicate) : query.FirstOrDefault();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, bool tracking = false, params Expression<Func<T, object>>[] includes)
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

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool tracking = false, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _unitOfWork.Context.Set<T>().Where(predicate);
            if (!tracking)
                query = query.AsNoTracking();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }

        public IQueryable<T> AsQueryable(bool tracking = false, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _unitOfWork.Context.Set<T>();
            if (!tracking)
                query = query.AsNoTracking();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}

