using Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.Service;
using System.Security.Principal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CompanyTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IOtpService _otpService;

        public CompanyController(ICompanyService companyService, IOtpService otpService)
        {
            _companyService = companyService;
            _otpService = otpService;

        }

        // POST api/<CompanyController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SignUpDto account)
        {
            var result = await _companyService.SignUpAsync(account);
            if (result.IsSuccess)
            {
                return Ok(result.Entity);
            }
            else
            { return BadRequest(result); }
        }

        [HttpPost("validate-otp")]
        public async Task<IActionResult> ValidateOtp([FromBody] OtpValidationDto otpDto)
        {
            var isValid = await _otpService.ValidateOtpAsync(otpDto.Email, otpDto.Otp);
            if (!isValid)
                return BadRequest(new { message = "Invalid or expired OTP." });

            return Ok(new { message = "OTP is valid. Proceed to set password." });

        }

        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordDto dto)
        {
            var result = await _companyService.SetPasswordAsync(dto);

            if (result == "Success")
                return Ok(new { message = "Password set successfully. You can now log in." });

            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _companyService.LoginAsync(loginDto);
            if (result.IsSuccess)
            {
                return Ok(result.Entity);
            }
            else
            { return BadRequest(result); }

        }



        [HttpPost("upload-logo")]
        public async Task<IActionResult> UploadLogo(IFormFile companyImage)
        {
            if (companyImage == null || companyImage.Length == 0)
                return BadRequest(new { message = "No file uploaded" });

            var filePath = await _companyService.HandleFileUploadsAsync(companyImage);

            return Ok(new { logoUrl = filePath }); 
        }
    }

}
