using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync( string BuyerEmail,string BasketId  , PaymentMethods PaymentMethod , int CustomerId);

        Task<Order> GetDetailsForSpecificOrder(int orderId );

        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);

         Task CreateInvoice(int orderId, DateTimeOffset dateTimeOffset, decimal total);

    }
}
