using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Repository.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.Property(O => O.Status).HasConversion(OS => OS.ToString(), OS => (OrderStatus)Enum.Parse(typeof(OrderStatus), OS));
            builder.Property(O => O.PaymentMethod).HasConversion(PM => PM.ToString(), PM => (PaymentMethods)Enum.Parse(typeof(PaymentMethods), PM));


            builder.HasOne(O => O.Customer)
                .WithMany(C => C.Orders)
                .HasForeignKey(O => O.CustomerId);

            builder.Property(O=>O.TotalAmount).HasColumnType("decimal(18,2)");
        }
    }
}
