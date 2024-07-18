using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order_Management_System.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Repository.Data.Configurations
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {


            builder.HasOne(OI => OI.Order)
                .WithMany(O => O.OrderItems)
                .HasForeignKey(O => O.OrderId);


            builder.HasOne(OI => OI.Product)
                .WithMany(P => P.OrderItems)
                .HasForeignKey(O => O.ProductId);

            builder.Property(OI=>OI.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(OI=>OI.Discount).HasColumnType("decimal(18,2)");

        }
    }
}
