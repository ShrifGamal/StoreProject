using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Store.Core.Entites;
using Store.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public static class SpecificationsEvaluator<TEntity , TKey> where TEntity : BaseEntity<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecifications<TEntity , TKey> specifications)
        {
            var query = inputQuery;

            if(specifications.Criteria is not null)
            {
                query = query.Where(specifications.Criteria);
            }

            if(specifications.OrderBy is not null)
            {
                query = query.OrderBy(specifications.OrderBy);
            }

            if(specifications.OrderByDesc is not null)
            {
                query = query.OrderByDescending(specifications.OrderByDesc);
            }

            if (specifications.IsPaginated)
            {
                query = query.Skip(specifications.Skip).Take(specifications.Take);
            }

            query = specifications.Includes.Aggregate(query, (CurrentQuery, includeExpression) => CurrentQuery.Include(includeExpression));

            return query;
        }
    }
}
