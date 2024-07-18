using Order_Management_System.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Services
{
    public interface IPaymentService
    {

        public Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);


    }
}
