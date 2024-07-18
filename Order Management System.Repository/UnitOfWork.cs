using Microsoft.EntityFrameworkCore;
using Order_Management_System.Core;
using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Repositories;
using Order_Management_System.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private  readonly OrderManagementDbContext _dbContext;
        private Hashtable _Repositories ;


        public UnitOfWork(OrderManagementDbContext dbContext)
        {
            _dbContext = dbContext;
            _Repositories = new Hashtable();

        }


        public async Task<int> CompleteAsync()
        => await  _dbContext.SaveChangesAsync();
        

        public async ValueTask DisposeAsync()
        => await  _dbContext.DisposeAsync();

        
        public  IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var Type = typeof(T).Name;

            //if (!_Repositories.ContainsKey(Type))
            //{
            //    var repositoryType = typeof(GenericRepository<>);
            //    var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);
            //    _Repositories.Add(Type, repositoryInstance);
            //}

            //return (IGenericRepository<T>)_Repositories[Type];


            if (!_Repositories.ContainsKey(Type))
            {

                var Repository = new GenericRepository<T>(_dbContext);
                _Repositories.Add(Type, Repository);

            }
            
            return  _Repositories[Type] as IGenericRepository<T>;


        }



    }
}
