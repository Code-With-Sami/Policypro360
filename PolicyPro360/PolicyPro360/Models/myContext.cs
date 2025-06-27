using Microsoft.EntityFrameworkCore;

namespace PolicyPro360.Models
{
    public class myContext : DbContext
    {
        public myContext(DbContextOptions<myContext> options) : base(options) { }
        public DbSet<Admin> Tbl_Admin { get; set; }
    }
}
