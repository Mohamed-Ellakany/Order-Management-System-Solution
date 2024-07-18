using Order_Management_System.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Repositories
{
    public interface IBasketRepository
    {

         Task<CustomerBasket?> GetBasketAsync(string BasketId);


         Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);


         Task<bool> DeleteBasketAsync(string BasketId);

    }
}
