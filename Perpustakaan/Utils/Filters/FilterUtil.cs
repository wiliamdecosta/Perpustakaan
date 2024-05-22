using JustclickCoreModules.Filters;
using LinqKit;
using Perpustakaan.Configurations.Db;
using System.Linq.Expressions;

namespace Perpustakaan.Utils.Filters
{
    public class FilterUtil<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        public FilterUtil(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public Paginated<T> GetContent(SearchRequest _request, List<string> searchKeywordsColumns)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (_request.Keywords != null && _request.Keywords != "")
            {
                var combinedExpression = PredicateBuilder.New<T>(false);
                foreach (var col in searchKeywordsColumns)
                {
                    var keywordsExpression = GetKeywordsExpression<T>(col, _request.Keywords);
                    if (keywordsExpression != null)
                    {
                        //OR combination
                        combinedExpression = combinedExpression.Or(keywordsExpression);
                    }
                }

                if (combinedExpression.Body != null)
                {
                    query = query.Where(combinedExpression);
                }
            }

            // Filter data based on request
            foreach (var filter in _request.Filters)
            {
                var filterExpression = GetFilterExpression<T>(filter.Key, filter.Operator, filter.Value);
                if (filterExpression != null)
                    query = query.Where(filterExpression);
            }

            if (_request.Sorts != null)
            {
                // Sort data
                foreach (var sort in _request.Sorts)
                {
                    query = ApplySort(query, sort.Key, sort.Order);
                }
            }

            Paginated<T> paginated = PaginateIt(query, _request);
            if (_request.Page <= 0 || _request.Size <= 0)
            {
                return paginated;
            }

            // Pagination
            query = query.Skip((_request.Page - 1) * _request.Size).Take(_request.Size);
            paginated.Data = query;

            return paginated;
        }

        private Paginated<T> PaginateIt(IQueryable<T> query, SearchRequest request)
        {
            Paginated<T> paginated = new Paginated<T>();
            paginated.Data = query;
            paginated.TotalCount = query.Count();
            paginated.PageSize = request.Size;
            paginated.PageNumber = request.Page;
            return paginated;
        }

        private IQueryable<T> ApplySort(IQueryable<T> query, string key, string order)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = order.ToUpper() == "ASC" ? "OrderBy" : "OrderByDescending";
            var methodCallExpression = Expression.Call(typeof(Queryable), methodName, new[] { typeof(T), property.Type },
                                                        query.Expression, Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(methodCallExpression);
        }

        private Expression<Func<T, bool>>? GetKeywordsExpression<T>(string key, string value)
        {
            if (value == null || value == "") return null;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(Convert.ChangeType(value, property.Type));

            Expression body = Expression.Call(property, "Contains", null, constant);
            return body != null ? Expression.Lambda<Func<T, bool>>(body, parameter) : null;
        }

        private Expression<Func<T, bool>>? GetFilterExpression<T>(string key, string @operator, string value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, key);
            var constant = Expression.Constant(Convert.ChangeType(value, property.Type));

            Expression? body = null;

            switch (@operator.ToUpper())
            {
                case "EQUAL":
                    body = Expression.Equal(property, constant);
                    break;
                case "NOT_EQUAL":
                    body = Expression.NotEqual(property, constant);
                    break;
                case "GREATER_THAN":
                    body = Expression.GreaterThan(property, constant);
                    break;
                case "LESS_THAN":
                    body = Expression.LessThan(property, constant);
                    break;
                case "GREATER_THAN_OR_EQUAL":
                    body = Expression.GreaterThanOrEqual(property, constant);
                    break;
                case "LESS_THAN_OR_EQUAL":
                    body = Expression.LessThanOrEqual(property, constant);
                    break;
                case "ILIKE":
                    // Assuming property type is string
                    body = Expression.Call(property, "Contains", null, constant);
                    break;
                default:
                    break;
            }

            return body != null ? Expression.Lambda<Func<T, bool>>(body, parameter) : null;
        }
    }
}
