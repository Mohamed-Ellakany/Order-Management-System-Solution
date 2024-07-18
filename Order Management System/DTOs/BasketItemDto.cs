using System.ComponentModel.DataAnnotations;

namespace Order_Management_System.DTOs
{
    public class BasketItemDto
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }


        [Required]
        [Range(1,int.MaxValue , ErrorMessage ="quantity must be one item at least")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.1 , double.MaxValue, ErrorMessage = "Price must be there")]
        public decimal UnitPrice { get; set; }

        [Required]
        //public decimal Discount { get; set; }
        private decimal Discount;

        public decimal discount
        {
            get { return Discount; }
            set {
                decimal Price = UnitPrice*Quantity; 
            if (Price > 200)
            {
                Discount = .1m * Price; 
            }
            else if (Price > 100)
            {
                Discount = .05m * Price;
            } }
        }

    }
}
