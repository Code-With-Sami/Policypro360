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

        public DbSet<Policy> Tbl_Policy { get; set; }
        public DbSet<PolicyAttribute> Tbl_PolicyAttributes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Policy>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.PolicyTypeId);

            modelBuilder.Entity<Policy>()
                .HasOne(p => p.Company)
                .WithMany()
                .HasForeignKey(p => p.CompanyId);

            modelBuilder.Entity<PolicyAttribute>()
                .HasOne(a => a.Policy)
                .WithMany(p => p.Attributes)
                .HasForeignKey(a => a.PolicyId);
        }
    }
}
