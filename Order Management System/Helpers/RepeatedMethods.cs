using AutoMapper;
using Order_Management_System.Core;
using Order_Management_System.Core.Entities;
using Order_Management_System.DTOs;

namespace Order_Management_System.Helpers
{
    public static class RepeatedMethods
    {
       
       public static async Task<int> addCustomer(CustomerDto MappedCustomer, IMapper _mapper , IUnitOfWork _unitOfWork)
        {
            

        var NewCustomer = _mapper.Map<CustomerDto, Customer>(MappedCustomer);

        await _unitOfWork.Repository<Customer>().CreateAsync(NewCustomer);

        int Result = await _unitOfWork.CompleteAsync();

            return Result;
        }
        
    }
}
