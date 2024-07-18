using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Core;
using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Specifications.Customer_Specs;
using Order_Management_System.DTOs;
using Order_Management_System.Errors;
using Order_Management_System.Helpers;

namespace Order_Management_System.Controllers
{
    
    public class CustomerController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        #region Add New Customer
        [Authorize("Customer")]
        [HttpPost]
        [ProducesResponseType(typeof(CustomerDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<CustomerDto>> AddCustomer([FromBody] CustomerDto MappedCustomer)
        {
            if (MappedCustomer is null)
            {
                return BadRequest(new ApiResponse(400));
            }

           
          var  Result =  await RepeatedMethods.addCustomer(MappedCustomer, _mapper, _unitOfWork);

            if (Result > 0)
            {

                return Ok(MappedCustomer);
            }
            else
            {
                return BadRequest(new ApiResponse(400));
            }

        }
        #endregion

        #region Get Customer Orders 


        //not tested 
        [Authorize("Customer")]
        [HttpGet("{customerId}/orders")]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetCustomerOrders(int customerId)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetAsyncById(customerId);

            if (customer == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var Spec = new OrdersOfCustomerSpec(customerId);


            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);

            var MappedOrders = _mapper.Map<IEnumerable<Order>, IEnumerable< OrderToReturnDto >> (orders);

            return Ok(MappedOrders);
        }

        #endregion


    }
}
