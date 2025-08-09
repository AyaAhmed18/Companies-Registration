using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class SignUpDto
    {
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "Please enter a valid email address")]
        public string EmailAddress { get; set; }
        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$",
        ErrorMessage = "Please enter a valid phone number")]
        public string? PhoneNumber { get; set; }
        public string ?websiteURL { get; set; }
        public string? CompanyLogo { get; set; }
        public IFormFile? CompanyImage { get; set; }
        public bool IsVerified { get; set; }
    }
}
