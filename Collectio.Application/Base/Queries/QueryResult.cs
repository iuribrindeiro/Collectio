using System.Linq;

namespace Collectio.Application.Base.Queries
{
    public class QueryResult<R> where R : class
    {
        protected QueryResult() 
            => Results = new R[] {}.AsQueryable();

        public static QueryResult<R> Success(IQueryable<R> results) 
            => new QueryResult<R>() { Results = results, IsSuccess = true };

        public static QueryResult<R> Failed() 
            => new QueryResult<R>() { IsSuccess = false };

        public IQueryable<R> Results { get; protected set; }
        public bool IsSuccess { get; protected set; }
    }
}