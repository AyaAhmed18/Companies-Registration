using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class SetPasswordDto
    {
        public string emailAddress { get; set; }
       // public string Otp { get; set; }
        public string PasswordHash { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
