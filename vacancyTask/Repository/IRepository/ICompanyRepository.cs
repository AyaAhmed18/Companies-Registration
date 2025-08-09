using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface ICompanyRepository
    {
        Task<Company> SignUpAsync(Company entity);
        Task<Company> UpdateAsync(Company entity);
        Task<Company> DeleteAsync(Company entity);
        Task<Company> GetByEmailAsync(string Email);
        Task<IQueryable<Company>> GetAllAsync();
        Task<int> SaveChangesAsync();

    }
}
