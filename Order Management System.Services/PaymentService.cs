using Microsoft.Extensions.Configuration;
using Order_Management_System.Core;
using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Repositories;
using Order_Management_System.Core.Services;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Order_Management_System.Core.Entities.Product;

namespace Order_Management_System.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration , IBasketRepository basketRepository , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
           _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];

            var Basket = await _basketRepository.GetBasketAsync(BasketId);

            if (Basket is null) return null;

            if(Basket.Items.Count > 0 )
            {
                foreach(var item in Basket.Items) 
                {
                    var Product = await _unitOfWork.Repository<Product>().GetAsyncById(item.Id);
                    if(item.UnitPrice != Product.Price)
                        item.UnitPrice = Product.Price;

                }
            }

            var SubTotal = Basket.Items.Sum(x => x.UnitPrice * x.Quantity);

        
            var Total = SubTotal;


            if (SubTotal > 200)
            {
                Total *= 0.9m; // 10% discount
            }
            else if (SubTotal > 100)
            {
                Total *= 0.95m; // 5% discount
            }

            long minimumChargeAmount = 50; // Example: 50 cents for USD
            long totalAmountInCents = (long)(Total * 100);

            if (totalAmountInCents < minimumChargeAmount)
            {
                throw new Exception("The amount must be greater than or equal to the minimum charge amount allowed for your account and the currency set.");
            }



            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if(string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long) (Total * 100 ) ,
                    Currency ="usd",
                    PaymentMethodTypes = new List<string>() { "card" , "us_bank_account" }
                };
                paymentIntent =await Service.CreateAsync(Options);


                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;

            }
            else
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(Total * 100)
                };
              paymentIntent= await Service.UpdateAsync(Basket.PaymentIntentId , Options);

                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }

            await _basketRepository.UpdateBasketAsync(Basket);

            return Basket;


        }
    }
}
