using Order_Management_System.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Entities
{
    public class Order :BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, decimal totalAmount, PaymentMethods paymentMethod, ICollection<OrderItem> orderItems, string paymentIntentId , int customerId)
        {
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
            OrderItems = orderItems;
            BuyerEmail = buyerEmail;
            PaymentIntentId = paymentIntentId;
            CustomerId = customerId;
        }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;


        public decimal TotalAmount { get; set; }

         public string BuyerEmail { get; set; }

        public PaymentMethods PaymentMethod { get; set; } 


        public OrderStatus Status { get; set; } = OrderStatus.Pending;


        #region Orede - OrderItems Relationship 

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        #endregion


        #region Orede - Customer Relationship 

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        #endregion

        public string PaymentIntentId { get; set; } 
    }
}
