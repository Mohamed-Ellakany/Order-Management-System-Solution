using Order_Management_System.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Specifications.OrderSpec
{
    public class OrderSpec : BaseSpecifications<Order>
    {
        public OrderSpec(string email) : base(O => O.BuyerEmail == email)
        {
            Includes.Add(O => O.PaymentMethod);
            Includes.Add(O => O.Status);
            Includes.Add(O => O.OrderItems);
            Includes.Add(O => O.PaymentMethod);
            Includes.Add(O => O.Customer);

        }

        public OrderSpec(string email, int OrderId) : base(O => O.BuyerEmail == email && O.Id == OrderId)
        {
            Includes.Add(O => O.PaymentMethod);
            Includes.Add(O => O.Status);
            Includes.Add(O => O.OrderItems);
            Includes.Add(O => O.PaymentMethod);
            Includes.Add(O => O.Customer);


        }
        public OrderSpec(int id):base(O =>O.Id == id)
        {
         
            Includes.Add(O => O.OrderItems);
          
        } 
        
        public OrderSpec()
        {
          
            Includes.Add(O => O.OrderItems);
           
        }
    }
}
