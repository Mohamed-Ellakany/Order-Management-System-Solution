using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Services;
using Order_Management_System.DTOs;
using Order_Management_System.Errors;

namespace Order_Management_System.Controllers
{
    public class PaymentController : BaseController
    {


        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        //create or update endpoint
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var customerBasket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (customerBasket is null) return BadRequest(new ApiResponse(400, "there is problem with your basket"));

            var MappedBasket = _mapper.Map<CustomerBasket, CustomerBasketDto>(customerBasket);

            return Ok(MappedBasket);

        }


    }
}
