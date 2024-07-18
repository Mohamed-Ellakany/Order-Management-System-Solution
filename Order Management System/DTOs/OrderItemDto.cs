using Order_Management_System.Core.Entities;

namespace Order_Management_System.DTOs
{
    public class OrderItemDto
    {

        public int Id { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }



        public int OrderId { get; set; }
       

        public int ProductId { get; set; }
       
    }
}
