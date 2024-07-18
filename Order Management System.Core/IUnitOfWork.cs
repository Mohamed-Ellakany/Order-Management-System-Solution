using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core
{
    public interface IUnitOfWork :IAsyncDisposable 
    {

        IGenericRepository<T> Repository<T>() where T :BaseEntity  ;

        Task<int> CompleteAsync();

    }
}
