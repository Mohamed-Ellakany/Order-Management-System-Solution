using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Core.Entities.Order_Aggregate
{
    public enum PaymentMethods
    {
        [EnumMember(Value = "card")]
        card,
       
        [EnumMember(Value = "us_bank_account")]
        us_bank_account
           
    }
}
