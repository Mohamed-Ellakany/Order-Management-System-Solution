using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Repositories;
using Order_Management_System.DTOs;
using Order_Management_System.Errors;

namespace Order_Management_System.Controllers
{
   
    public class BasketsController : BaseController
    {

        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }



        [Authorize("Customer")]
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string BasketId)
        {
            var Basket = await _basketRepository.GetBasketAsync(BasketId);

           

            return Basket is null ? new CustomerBasket(BasketId) : Basket;
        }



        [Authorize("Customer")]
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto Basket)
        {

           

            var MappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(Basket);





            var CreatedOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(MappedBasket);





            if (CreatedOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));

            return Ok(CreatedOrUpdatedBasket);

        }


        [Authorize("Customer")]
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
            return await _basketRepository.DeleteBasketAsync(BasketId);

        }


    }
}
