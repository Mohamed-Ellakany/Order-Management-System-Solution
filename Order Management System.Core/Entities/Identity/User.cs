using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Entities.Identity
{
    public class User : IdentityUser
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }


        public string RoleName { get; set; }

    }
}
