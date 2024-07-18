using Order_Management_System.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Specifications.Customer_Specs
{
    public class OrdersWithOrderItemSpec :BaseSpecifications<OrderItem>
    {


        public OrdersWithOrderItemSpec() : base()
        {

            Includes.Add(O => O.Product);

        }
        public OrdersWithOrderItemSpec(int OrderId) : base(O => O.Id == OrderId)
        {

            Includes.Add(O => O.Product);



        }



    }
}
