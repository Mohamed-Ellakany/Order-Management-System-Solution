using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            
        }
        public Product(string name, decimal price, int stock, ICollection<OrderItem> orderItems)
        {
            Name = name;
            Price = price;
            Stock = stock;
            OrderItems = orderItems;
        }

        public string Name { get; set; }
       

        public decimal Price { get; set; }
       

        public int Stock { get; set; }
       
        
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();


    }
}
