using Microsoft.Extensions.Caching.Memory;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class OtpService : IOtpService
    {
        private readonly IMemoryCache _cache;

        public OtpService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task SaveOtpAsync(string email, string otp)
        {
            _cache.Set(email, otp, TimeSpan.FromMinutes(10)); 
            return Task.CompletedTask;
        }

        public Task<bool> ValidateOtpAsync(string email, string otp)
        {
            if (_cache.TryGetValue(email, out string storedOtp) && storedOtp == otp)
            {
             //   _cache.Remove(email); 
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }

}
