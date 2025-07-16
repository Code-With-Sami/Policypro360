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
            // Policy relationships
            modelBuilder.Entity<Policy>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.PolicyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Policy>()
                .HasOne(p => p.Company)
                .WithMany()
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // PolicyAttribute relationship
            modelBuilder.Entity<PolicyAttribute>()
                .HasOne(a => a.Policy)
                .WithMany(p => p.Attributes)
                .HasForeignKey(a => a.PolicyId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserPolicy relationships
            modelBuilder.Entity<UserPolicy>()
                .HasOne(up => up.Policy)
                .WithMany()
                .HasForeignKey(up => up.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserPolicy>()
                .HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserClaim relationships
            modelBuilder.Entity<UserClaim>()
                .HasOne(uc => uc.Category)
                .WithMany()
                .HasForeignKey(uc => uc.PolicyCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserClaim>()
                .HasOne(uc => uc.Policy)
                .WithMany()
                .HasForeignKey(uc => uc.PolicyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserClaim>()
                .HasOne(uc => uc.Users)
                .WithMany()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // AdminWallet relationships
            modelBuilder.Entity<AdminWallet>()
                .HasOne(aw => aw.User)
                .WithMany()
                .HasForeignKey(aw => aw.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<AdminWallet>()
                .HasOne(aw => aw.Company)
                .WithMany()
                .HasForeignKey(aw => aw.CompanyId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<AdminWallet>()
                .HasOne(aw => aw.Policy)
                .WithMany()
                .HasForeignKey(aw => aw.PolicyId)
                .OnDelete(DeleteBehavior.Restrict); 

            // CompanyWallet relationships
            modelBuilder.Entity<CompanyWallet>()
                .HasOne(cw => cw.User)
                .WithMany()
                .HasForeignKey(cw => cw.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<CompanyWallet>()
                .HasOne(cw => cw.Company)
                .WithMany()
                .HasForeignKey(cw => cw.CompanyId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<CompanyWallet>()
                .HasOne(cw => cw.Policy)
                .WithMany()
                .HasForeignKey(cw => cw.PolicyId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<TransactionHistory>()
                .HasOne(th => th.Company)
                .WithMany()
                .HasForeignKey(th => th.CompanyId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<TransactionHistory>()
                .HasOne(th => th.Policy)
                .WithMany()
                .HasForeignKey(th => th.PolicyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AdminWallet>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<CompanyWallet>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<TransactionHistory>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            });
        }
        

        public DbSet<UserPolicy> Tbl_UserPolicy { get; set; }
        public DbSet<UserPayment> Tbl_UserPayment { get; set; }
        public DbSet<AdminWallet> Tbl_AdminWallet { get; set; }
        public DbSet<CompanyWallet> Tbl_CompanyWallet { get; set; }
        public DbSet<TransactionHistory> Tbl_TransactionHistory { get; set; }
        public DbSet<UserClaim> Tbl_UserClaims { get; set; }


    }
}
