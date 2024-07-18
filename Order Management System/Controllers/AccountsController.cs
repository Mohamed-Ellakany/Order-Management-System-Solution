using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Core;
using Order_Management_System.Core.Entities.Identity;
using Order_Management_System.Core.Services;
using Order_Management_System.DTOs;
using Order_Management_System.Errors;
using Order_Management_System.Helpers;

namespace Order_Management_System.Controllers
{
  
    public class AccountsController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AccountsController(UserManager<User> userManager , SignInManager<User> signInManager , ITokenService tokenService , RoleManager<IdentityRole> roleManager , IMapper mapper ,IUnitOfWork unitOfWork )
        {
            _userManager = userManager;
           _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }




        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if( CheckEmailExists(registerDto.Email).Result.Value) {
                return BadRequest(new ApiResponse(400, "email already exist"));
            }

            if(registerDto.Role != "Customer" && registerDto.Role != "Admin")
            {
                registerDto.Role = "Customer";
            }

            var user = new User()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.Email.Split('@')[0],
                RoleName= registerDto.Role 
                
            };

           var Result =  await _userManager.CreateAsync(user , registerDto.Password) ;
            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));

            if (!string.IsNullOrEmpty(registerDto.Role))
            {
                if (await _roleManager.RoleExistsAsync(registerDto.Role))
                {
                    await _userManager.AddToRoleAsync(user, registerDto.Role);
                }
                
            }
           

            var ReturnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Role = user.RoleName,
               Token = await _tokenService.CreateTokenAsync(user , _userManager)
            };


             await RepeatedMethods.addCustomer(new CustomerDto { Name =registerDto.DisplayName , Email = registerDto.Email}, _mapper, _unitOfWork);

            


            return Ok(ReturnedUser);
        }



        [HttpPost("Login")]
        public  async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User == null) return Unauthorized(new ApiResponse(401));


           var Result = await _signInManager.CheckPasswordSignInAsync(User , model.Password , false);

            if (!Result.Succeeded) return Unauthorized(new ApiResponse(401));


            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Role = User.RoleName,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            });

        }



        [Authorize("admin")]
        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExists(string Email)
        {
            return await _userManager.FindByEmailAsync(Email) is not null ;
        }





    }
}
