using Order_Management_System.Core.Entities.Order_Aggregate;
using Order_Management_System.Core.Entities;

namespace Order_Management_System.DTOs
{
    public class OrderToReturnDto
    {

        public int Id { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;


        public decimal TotalAmount { get; set; }

        public string BuyerEmail { get; set; }

        public string PaymentMethod { get; set; }


        public string Status { get; set; }


        #region Orede - OrderItems Relationship 

        public ICollection<OrderItemDto> OrderItems { get; set; } = new HashSet<OrderItemDto>();
        #endregion


        #region Orede - Customer Relationship 

        public int CustomerId { get; set; }

        #endregion

        public string PaymentIntentId { get; set; }
    }
}
