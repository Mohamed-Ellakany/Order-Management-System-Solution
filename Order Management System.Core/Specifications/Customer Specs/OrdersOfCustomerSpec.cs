using Order_Management_System.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Specifications.Customer_Specs
{
    public class OrdersOfCustomerSpec :BaseSpecifications<Order>
    {

        public OrdersOfCustomerSpec() : base() 
        {
            
            Includes.Add(O => O.OrderItems);
            
            
        }
        public OrdersOfCustomerSpec(int CustomerId) : base(O => O.CustomerId == CustomerId) 
        {
            
            Includes.Add(O =>O.OrderItems);
            
            
            
        }

      


    }
}
