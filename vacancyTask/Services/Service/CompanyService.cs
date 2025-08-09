using AutoMapper;
using Data;
using Data.DTO;
using Data.ViewResult;
using Microsoft.AspNetCore.Http;
using Repository.IRepository;
using Repository.Repository;
using Services.IService;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.WebRequestMethods;

namespace Services.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _CompanyRepository;
        private readonly IMapper _mapper;
        private readonly IOtpService _otpService;
        private readonly IFileUploadService _fileService;
        private readonly IEmailService _emailService;

        public CompanyService(ICompanyRepository CompanyRepository, IMapper mapper, IOtpService otpService, IEmailService emailService, IFileUploadService fileService)
        {
            _otpService = otpService;
            _emailService = emailService;
            _CompanyRepository = CompanyRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ResultView<SignUpDto>> SignUpAsync(SignUpDto account)
        {
            var allCompanies = await _CompanyRepository.GetAllAsync();
            var oldcampany = allCompanies.Where(c => c.EmailAddress== account.EmailAddress).FirstOrDefault();
            if (oldcampany != null)
            {
                return new ResultView<SignUpDto> { Entity = null, IsSuccess = false, Message = "Company Already Exists" };
            }
            else
            {
                var Company = _mapper.Map<Company>(account);
                account.IsVerified = false;
              //  await HandleFileUploadsAsync(Company, account);
                var newCompany = await _CompanyRepository.SignUpAsync(Company);
                
                await _CompanyRepository.SaveChangesAsync();
                var createdCompany = _mapper.Map<SignUpDto>(newCompany);

                var otp = new Random().Next(100000, 999999).ToString();

                await _otpService.SaveOtpAsync(account.EmailAddress, otp);
                await _emailService.SendEmailAsync(account.EmailAddress, $"Your OTP is {otp}");

                return new ResultView<SignUpDto> { Entity = createdCompany, IsSuccess = true, Message = "Created Successfully" };
            }
        }
        public async Task<string> HandleFileUploadsAsync(IFormFile companyLogo)
        {
            if (companyLogo != null)
            {
                var savedPath = await _fileService.UploadFileAsync(companyLogo, "Logos");
                return savedPath;
            }
            return null;
        }

        public async Task<ResultView<GetCompanyDataDto>> LoginAsync(LoginDto LoginAccount)
        {
            if (LoginAccount == null)
            {
                return new ResultView<GetCompanyDataDto>
                {
                    Entity = null,
                    Message = "Login data is missing",
                    IsSuccess = false
                };
            }

            if (string.IsNullOrWhiteSpace(LoginAccount.emailAddress) || string.IsNullOrWhiteSpace(LoginAccount.passwordHash))
            {
                return new ResultView<GetCompanyDataDto>
                {
                    Entity = null,
                    Message = "Email and password are required",
                    IsSuccess = false
                };
            }

            var company = await _CompanyRepository.GetByEmailAsync(LoginAccount.emailAddress
                );

            if (company == null || !company.IsVerified)
            {
                return new ResultView<GetCompanyDataDto>
                {
                    Entity = null,
                    Message = "Email not found",
                    IsSuccess = false
                };
            }
            else if(company.PasswordHash!= LoginAccount.passwordHash) 
            {
                    return new ResultView<GetCompanyDataDto>
                    {
                        Entity = null,
                        Message = "Invalid password",
                        IsSuccess = false
                    };
            }
               else 
                {
                var CompanyData = _mapper.Map<GetCompanyDataDto>(company);
                return new ResultView<GetCompanyDataDto>
                {
                    Entity = CompanyData,
                    Message = "Login Successful",
                    IsSuccess = true
                };
             }


    
           
        }
        public async Task<string> SetPasswordAsync(SetPasswordDto dto)
        {
            if (dto.PasswordHash != dto.ConfirmPassword)
                return "Passwords do not match.";

          

            var company = await _CompanyRepository.GetByEmailAsync(dto.emailAddress);
            if (company == null || company.IsVerified)
                return "Invalid request.";
           
            company.IsVerified = true;
            company.PasswordHash = dto.PasswordHash;
            await _CompanyRepository.UpdateAsync(company);
            await _CompanyRepository.SaveChangesAsync();

            return "Success";
        }

       
    }
}
