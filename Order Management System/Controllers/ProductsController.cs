using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order_Management_System.Contants;
using Order_Management_System.Core;
using Order_Management_System.Core.Entities;
using Order_Management_System.Core.Repositories;
using Order_Management_System.DTOs;
using Order_Management_System.Errors;

namespace Order_Management_System.Controllers
{

    public class ProductsController : BaseController
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        #region Get All Products EndPoint
        [Authorize("Customer")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {

            var products =await _unitOfWork.Repository<Product>().GetAllAsync();
            
            if (products == null) { return NotFound(new ApiResponse(404)); }

            var MappedProducts = _mapper.Map<IEnumerable<Product> , IEnumerable< ProductDto> >(products);
           
            return Ok(MappedProducts);
        }


        #endregion

        #region Get Product By Id
        [Authorize("Customer")]
        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<ProductDto>> GetProductById(int? Id)
        {
            if (Id is null)
            {
               return BadRequest(new ApiResponse(400));
            }
            else
            {
                var product = await _unitOfWork.Repository<Product>().GetAsyncById(Id);
                if (product is null) 
                {
                return NotFound(new ApiResponse(404));
                }

                else
                {
                    var MappedProducts = _mapper.Map<Product, ProductDto>(product);

                    return Ok(MappedProducts);

                }

            }
           
        }
        #endregion



        #region Add New Product 
        [Authorize(policy: "Admin")]
        [HttpPost]
        // [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<Product>> AddProduct([FromBody] ProductDto Mappedproduct)
        {
            if (Mappedproduct == null)
            {
                return BadRequest(new ApiResponse(400));
            }

          var NewProduct= _mapper.Map<ProductDto, Product>(Mappedproduct);

            await _unitOfWork.Repository<Product>().CreateAsync(NewProduct);

            int Result = await _unitOfWork.CompleteAsync();

            if(Result > 0)
            {
                 
                return Ok(Mappedproduct);
            }
            else
            {
                return BadRequest(new ApiResponse(400));
            }
           
        }

        #endregion

        #region Update Product Details (Admin Only)


        [Authorize(policy: "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromBody] ProductDto mappedProduct)
        {
            if (id != mappedProduct.Id)
            {
                return BadRequest(new ApiResponse(400));
            }

            var existingProduct = await _unitOfWork.Repository<Product>().GetAsyncById(id);

            if (existingProduct == null)
            {
                return NotFound(new ApiResponse(404));
            }

            _mapper.Map(mappedProduct, existingProduct);

            try
            {
                _unitOfWork.Repository<Product>().Update(existingProduct);
                int result = await _unitOfWork.CompleteAsync();

                if (result > 0)
                {
                    return Ok(mappedProduct);
                }
                else
                {
                    return BadRequest(new ApiResponse(400));
                }
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }








        #endregion
    }
}
