using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;
using Order_Management_System.Core;
using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Entities.Order_Aggregate;
using Order_Management_System.Core.Repositories;
using Order_Management_System.Core.Services;
using Order_Management_System.Core.Specifications.OrderSpec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;



namespace Order_Management_System.Services
{
    public class OrderService : IOrderService
    {

        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository,
           IUnitOfWork unitOfWork, IPaymentService paymentService
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }




        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, PaymentMethods PaymentMethod, int CustomerId)
        {
            var Basket = await _basketRepository.GetBasketAsync(BasketId);



            // get selected item at basket

            var OrderItems = new List<OrderItem>();
            if (Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetAsyncById(item.Id);
                    if (Product == null || Product.Stock < item.Quantity)
                    {
                        return null;
                    }
                    var orderItem = new OrderItem(item.Quantity, item.UnitPrice, item.Discount,Product.Id );

                    OrderItems.Add(orderItem);
                    Product.Stock -= item.Quantity;
                }
            }

            var SubTotal = OrderItems.Sum(item => item.UnitPrice * item.Quantity);
            var Total = SubTotal;


            if (SubTotal > 200)
            {
                Total *= 0.9m;
            }
            else if (SubTotal > 100)
            {
                Total *= 0.95m;
            }

            var Spec = new OrderWithPaymentIntentSpec(Basket.PaymentIntentId);

            var ExOrder = await _unitOfWork.Repository<Order>().GetWithSpecAsyncById(Spec);
            if (ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            }

            var Order = new Order(BuyerEmail, Total, PaymentMethod, OrderItems, Basket.PaymentIntentId , CustomerId);

            // add order localy
            await _unitOfWork.Repository<Order>().CreateAsync(Order);

            // save order to db
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;

            foreach (var orderItem in OrderItems)
            {
                orderItem.OrderId = Order.Id;
                 _unitOfWork.Repository<OrderItem>().Update(orderItem);
                await _unitOfWork.CompleteAsync();
            }

            var Email = new Email()
            {
                To = BuyerEmail,
                Subject = "Order Managment System",
                Body = "You Order Is Created",
            };

          await CreateInvoice(Order.Id, Order.OrderDate, Total);

            SendEmail(Email);



            return Order;
        }






        public async Task<Order> GetDetailsForSpecificOrder(int orderId)
        {
            var Spec = new OrderSpec(orderId);
            var Order = await _unitOfWork.Repository<Order>().GetWithSpecAsyncById(Spec);

            return Order;
        }



        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var Spec = new OrderSpec();
            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);

            return Orders;
        }


        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _unitOfWork.Repository<Order>().GetAsyncById(orderId);
            if (order == null) return false;
            order.Status = status;
            var res = await _unitOfWork.CompleteAsync();
            if (res > 0)
            {
                var Email = new Email()
                {
                    To = order.BuyerEmail,
                    Subject = "Order Managment System",
                    Body = "You Order Status is Updated",
                };

              SendEmail(Email);
            }
            return true;
        }



        public async Task CreateInvoice(int orderId, DateTimeOffset dateTimeOffset, decimal total)
        {
            var Invoice = new Invoice
            {
                InvoiceDate = dateTimeOffset,
                OrderId = orderId,
                TotalAmount = total

            };

            await _unitOfWork.Repository<Invoice>().CreateAsync(Invoice);

            await _unitOfWork.CompleteAsync();

           


        }

        public static void SendEmail(Email email)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress("Order Management System", "esamm612@gmail.com"));

        message.To.Add(new MailboxAddress("", email.To));

        message.Subject = email.Subject;

        message.Body = new TextPart("plain")
        {
            Text = email.Body
        };

        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate("esamm612@gmail.com", "xuvlegrulkfcdeti");

                client.Send(message);
                client.Disconnect(true);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
        }





    }
}