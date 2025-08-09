using Context;
using Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class CompanyRepository: ICompanyRepository
    {
        protected readonly CompanyContext _context;
        protected readonly DbSet<Company> _companies;
        public CompanyRepository(CompanyContext context)
        {
            _context = context;
            _companies = _context.Set<Company>();
        }

        public async Task<Company> SignUpAsync(Company entity)
        {
            return (await _companies.AddAsync(entity)).Entity;
        }

        public Task<Company> UpdateAsync(Company entity)
        {
            return Task.FromResult(_companies.Update(entity).Entity);
        }

        public Task<Company> DeleteAsync(Company entity)
        {
            return Task.FromResult(_companies.Remove(entity).Entity);
        }

        public Task<IQueryable<Company>> GetAllAsync()
        {
            return Task.FromResult(_companies.Select(e => e));
        }

        public async Task<Company> GetByEmailAsync(string email)
        {
            return await _companies.FirstOrDefaultAsync(c => c.EmailAddress == email); ;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
