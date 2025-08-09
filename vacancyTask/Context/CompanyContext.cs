
using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Context
{
    public class CompanyContext:DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public CompanyContext(DbContextOptions<CompanyContext> options) : base(options) { }


    }
}
