using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Core;
using Order_Management_System.Core.Repositories;
using Order_Management_System.Core.Services;
using Order_Management_System.Errors;
using Order_Management_System.Helpers;
using Order_Management_System.Services;
using OrderManagementSystem.Repository;

namespace Order_Management_System.Extensions
{
    public static class AppServicesExtension
    {

        public static IServiceCollection AppServices (this IServiceCollection services)
        {

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddAutoMapper(M => M.AddProfile(new AutoMapperProfiles())) ;




            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                                 .SelectMany(P => P.Value.Errors)
                                                                 .Select(E => E.ErrorMessage)
                                                                 .ToArray();

                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };

            });







            return services;
        }

    }
}
