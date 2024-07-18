using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Core;
using Order_Management_System.Core.Entities;
using Order_Management_System.Errors;


namespace Order_Management_System.Controllers
{
   
    public class InvoiceController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InvoiceController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        #region Get details of a specific invoice (admin only)        [Authorize(policy: "Admin")]
        [HttpGet("{InvoiceId}")]
        public async Task<ActionResult<Invoice>> GetdetailsForSpecificInvoice(int InvoiceId)
        {
            var invoice = await _unitOfWork.Repository<Invoice>().GetAsyncById(InvoiceId);
            if (invoice is null) return NotFound(new ApiResponse(404 , "No Invoice Yet"));
            
            return Ok(invoice);
        
        }

        #endregion


        #region  Get all invoices (admin only)        [Authorize(policy: "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable <Invoice>>> GetAllInvoices()
        {
            var invoices = await _unitOfWork.Repository<Invoice>().GetAllAsync();
            if (!invoices.Any()) return NotFound(new ApiResponse(404 , "No Invoices")); 
            return Ok(invoices);
        }

        #endregion

    }
}
