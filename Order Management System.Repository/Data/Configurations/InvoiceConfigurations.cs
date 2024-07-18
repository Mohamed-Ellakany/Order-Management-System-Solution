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
    public class InvoiceConfigurations : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            
            builder.HasOne(I => I.Order)
                    .WithMany()
                    .HasForeignKey(I => I.OrderId);

            builder.Property(I=>I.TotalAmount).HasColumnType("decimal(18,2)");

        }
    }
}
