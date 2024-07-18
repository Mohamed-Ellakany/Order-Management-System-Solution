using Microsoft.EntityFrameworkCore;
using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Repositories;
using Order_Management_System.Core.Specifications;
using Order_Management_System.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private protected readonly OrderManagementDbContext _dbContext;

        public GenericRepository(OrderManagementDbContext dbContext)
        {
           _dbContext = dbContext;
        }

        public async Task CreateAsync(T entity)
        =>   await _dbContext.Set<T>().AddAsync(entity);




        public async Task<IEnumerable<T>> GetAllAsync()
       => await _dbContext.Set<T>().ToListAsync();

      
        public async Task<T?> GetAsyncById(int? id)
        => await  _dbContext.Set<T>().FindAsync(id);

       
        public  void Update(T entity)
        => _dbContext.Set<T>().Update(entity);
         


          public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> Spec)
        {
           return await ApplySpec(Spec).AsNoTracking().ToListAsync();
        }



        public async Task<T?> GetWithSpecAsyncById(ISpecification<T> Spec)
        {
           return await ApplySpec(Spec).FirstOrDefaultAsync();
        }

      private IQueryable<T> ApplySpec(ISpecification<T> Spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec);
        }

        public void Delete(T entity)
        => _dbContext.Set<T>().Remove(entity);
    }
}
