using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }
        public string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string?   websiteURL { get; set; }
        public string? CompanyLogo { get; set; }
        public string? PasswordHash { get; set; }
        public bool IsVerified { get; set; }

    }
}
