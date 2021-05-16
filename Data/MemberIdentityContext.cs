using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeAreFamily.Models.MemberIdentity;

namespace WeAreFamily.MemberIdentity.Api.Data
{
    public class MemberIdentityContext : DbContext
    {
        public MemberIdentityContext(DbContextOptions<MemberIdentityContext> dbContextOptions) : base(dbContextOptions) { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer(_connectionString);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Member>(e =>
            {
                e.HasKey(x => x.MembershipId);
            });

            //Members.
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer(_connectionString);
        //}

        public DbSet<Member> Members { get; set; }
    }
}
