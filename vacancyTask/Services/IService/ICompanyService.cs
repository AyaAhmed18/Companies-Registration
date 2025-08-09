using Data;
using Data.DTO;
using Data.ViewResult;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface ICompanyService
    {
        Task<ResultView<SignUpDto>> SignUpAsync(SignUpDto account);
        Task<string> SetPasswordAsync(SetPasswordDto dto);

        Task<string> HandleFileUploadsAsync(IFormFile image);
        Task<ResultView<GetCompanyDataDto>> LoginAsync(LoginDto LoginAccount);
       
    }
}
