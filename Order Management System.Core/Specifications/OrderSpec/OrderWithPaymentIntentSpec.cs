using Order_Management_System.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Specifications.OrderSpec
{
    public class OrderWithPaymentIntentSpec :BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpec(string paymentIntentId ):base(Order=>Order.PaymentIntentId == paymentIntentId) 
        { }

    }
}
