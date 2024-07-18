using Order_Management_System.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecification<T> where T : BaseEntity
    {

        public Expression<Func<T, bool>>? Criteria { get; set; } = null;


        public List<Expression<Func<T, object>>> Includes { get ; set ; }




        public BaseSpecifications()
        {
            Includes= new List<Expression<Func<T, object>>>();


        }

        public BaseSpecifications(Expression<Func<T, bool>>? CriteriaExpression)
        {
            Criteria = CriteriaExpression;
            Includes = new List<Expression<Func<T, object>>>();

        }
      


    }
}
