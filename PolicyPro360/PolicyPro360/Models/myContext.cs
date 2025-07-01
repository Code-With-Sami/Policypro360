using Microsoft.EntityFrameworkCore;
using PolicyPro360.Models;

namespace PolicyPro360.Models
{
    public class myContext : DbContext
    {
        public myContext(DbContextOptions<myContext> options) : base(options) { }
        public DbSet<Admin> Tbl_Admin { get; set; }
        public DbSet<Users> Tbl_Users { get; set; }
        public DbSet<Category> Tbl_Category { get; set; }
        public DbSet<Company> Tbl_Company { get; set; }
    }
}
