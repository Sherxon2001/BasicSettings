namespace BasicSettings.DataLayer.Repository.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        IDbConnection DbConnection { get; }
        Task<IPageCollection<T>> GetList(Expression<Func<T, bool>> expression, int pageNumber = 1, int pageSize = 5, params Expression<Func<T, object>>[] includes);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool tracking = false, params Expression<Func<T, object>>[] includes);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, bool tracking = false, params Expression<Func<T, object>>[] includes);
        T? FirstOrDefault(Expression<Func<T, bool>>? predicate = null, bool tracking = false, params Expression<Func<T, object>>[] includes);
        void Add(T entity);
        Task AddAsycn(T entity);
        void AddRange(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        public IQueryable<T> AsQueryable(bool tracking = false, params Expression<Func<T, object>>[] includes);
    }
}
