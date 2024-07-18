using Microsoft.EntityFrameworkCore;
using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery , ISpecification<T> spec)
        {
            var query = inputQuery;  // _dbcontext.set<T>()

            if (spec.Criteria is not null )
            {
                query =query.Where(spec.Criteria);  // _dbcontext.set<T>().where(E=>E.???? == ????)
            }



            query = spec.Includes.Aggregate(query, (CurrentQuery, newExpression) => CurrentQuery.Include(newExpression));

         


            return query;
        }

    }
}
