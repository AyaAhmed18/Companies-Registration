using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class LoginDto
    {
        public string emailAddress { get; set; }
        public string passwordHash { get; set; }
    }
}
