using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Core;
using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Entities.Order_Aggregate;
using Order_Management_System.Core.Services;
using Order_Management_System.Core.Specifications.OrderSpec;
using Order_Management_System.DTOs;
using Order_Management_System.Errors;
using Order_Management_System.Helpers;
using System.Security.Claims;

namespace Order_Management_System.Controllers
{

    public class OrdersController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork unitOfWork, IOrderService orderService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
            _mapper = mapper;
        }

        #region  Create a new order
        [Authorize("Customer" )]
        [ProducesResponseType(typeof(OrderToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            try
            {
                var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);

                var order = await _orderService.CreateOrderAsync(BuyerEmail, orderDto.basketId,(PaymentMethods)Enum.Parse(typeof(PaymentMethods), orderDto.PaymentMethod) ,orderDto.CustomerId);
                if (order is null) return BadRequest(new ApiResponse(400 ));
             var MappedOrder=   _mapper.Map<Order , OrderToReturnDto>(order);

                await _orderService.CreateInvoice(order.Id, order.OrderDate, order.TotalAmount);

              

                return Ok(MappedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }
        #endregion



        #region Get details of a specific order
        [Authorize("Customer")]
        [ProducesResponseType(typeof(OrderToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [HttpGet("{OrderId}")]
        public async Task<ActionResult<OrderToReturnDto>> GetDetailsOfOrder(int OrderId)
        {
            var Order = await _orderService.GetDetailsForSpecificOrder(OrderId);
            if (Order is null) return NotFound(new ApiResponse(404));
            var MappedOrder = _mapper.Map<Order, OrderToReturnDto>(Order);

            return Ok(MappedOrder);

        }


        #endregion




        #region Get All Orders (admin only)

        [Authorize("Admin")]
        
        [ProducesResponseType(typeof(IEnumerable<OrderToReturnDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrders()
        {


            var Orders = await _orderService.GetAllOrdersAsync();


            if (Orders is null)
            {
                return NotFound(new ApiResponse(404));
            }
            else
            {
                var MappedOrders = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDto>>(Orders);

                return Ok(MappedOrders);
            }


        }


        #endregion


        #region Update order status (admin only)




        [Authorize("Admin")]
        [HttpPost("{orderId}/{status}")]
        public async Task<ActionResult<OrderToReturnDto>> UpdateOrderStatus(int orderId, OrderStatus status)
        {
         var res =  await _orderService.UpdateOrderStatusAsync(orderId, status);
            if(res == true)
            {
                var order = await _unitOfWork.Repository<Order>().GetAsyncById(orderId);
                var Mapped = _mapper.Map<Order,OrderToReturnDto>(order);

                return Ok(Mapped);
            }
            else
            {
                return BadRequest(new ApiResponse(400));
            }

        }


  #endregion
    
    
    
    
    }
}