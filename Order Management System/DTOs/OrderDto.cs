using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Entities.Order_Aggregate;

namespace Order_Management_System.DTOs
{
    public class OrderDto
    {
       public string basketId {  get; set; }


        //public int Id { get; set; }


        //public DateTime OrderDate { get; set; }


        //public decimal TotalAmount { get; set; }

       // public string BuyerEmail { get; set; }


        public string PaymentMethod { get; set; }


       // public OrderStatus Status { get; set; }


        #region Orede - OrderItems Relationship 

        //public ICollection<OrderItem> OrderItems { get; set; }

        #endregion


        #region Orede - Customer Relationship 

        public int CustomerId { get; set; }

       

        #endregion

    }
}
