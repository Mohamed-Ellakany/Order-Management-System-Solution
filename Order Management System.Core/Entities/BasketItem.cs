using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Entities
{
    public class BasketItem
    {
        public int Id { get; set; }

       public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }
     

        public int Quantity { get; set; }



        public void ApplyDiscount()
        {
            decimal Price = UnitPrice * Quantity;
            if (Price > 200)
            {
                Discount = .1m * Price;
            }
            else if (Price > 100)
            {
                Discount = .05m * Price;
            }
        }
    }
}
