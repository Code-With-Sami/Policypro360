using Microsoft.EntityFrameworkCore;

namespace PolicyPro360.Models
{
    public class myContext : DbContext
    {
        public myContext(DbContextOptions<myContext> options) : base(options) { }
        public DbSet<Admin> Tbl_Admin { get; set; }
        public DbSet<Users> Tbl_Users { get; set; }

        public DbSet<Category> Tbl_Category { get; set; }
    }
}
