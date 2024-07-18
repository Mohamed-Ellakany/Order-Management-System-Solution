using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {

        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetAsyncById(int? id);

        Task CreateAsync(T entity);

        void Update(T entity);

        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> Spec);

        Task<T?> GetWithSpecAsyncById(ISpecification<T> Spec);

        void Delete(T entity);
    }
}
