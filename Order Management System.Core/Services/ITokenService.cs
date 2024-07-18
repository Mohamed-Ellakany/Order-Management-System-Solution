using Microsoft.AspNetCore.Identity;
using Order_Management_System.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Services
{
    public interface ITokenService
    {

        Task<string> CreateTokenAsync(User user , UserManager<User> userManager);

    }
}
