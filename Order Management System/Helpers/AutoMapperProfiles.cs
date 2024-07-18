using AutoMapper;
using Order_Management_System.Core.Entities;
using Order_Management_System.DTOs;

namespace Order_Management_System.Helpers
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            
            CreateMap<Customer, CustomerDto>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();

            CreateMap<CustomerBasket , CustomerBasketDto>().ReverseMap();

            CreateMap<BasketItem , BasketItemDto>().ReverseMap();

            CreateMap<OrderDto, Order>().ReverseMap();

            CreateMap<OrderToReturnDto, Order>().ReverseMap();
        
        
        CreateMap<OrderItem , OrderItemDto>().ReverseMap();

        }

        




    }
}
