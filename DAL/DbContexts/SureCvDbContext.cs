using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAL.DbContexts
{
    public class SureCvDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        // This CTOR just for migrations 
        public SureCvDbContext() { }
        public DbSet<Candidate> Resources { get; set; }
        public DbSet<CandedatesCvTransaction> Bookings { get; set; }
        public SureCvDbContext(DbContextOptions opt) : base(opt)
        {
            Console.WriteLine("***************** SureCvDbContext Context" + "********");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>()
                        .HasMany(prop => prop.CandedatesCvTransactions)
                        .WithOne(nav => nav.Candidate);
            modelBuilder.Entity<CandedatesCvTransaction>()
                .HasOne(nav => nav.Candidate)
                .WithMany(prop => prop.CandedatesCvTransactions)
                .HasForeignKey(fk => fk.Candedateid);
        }
    }
}
