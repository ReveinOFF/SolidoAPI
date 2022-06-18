using Core.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SolidoDbContext : IdentityDbContext
    {
        public SolidoDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Main>()
            .Property(e => e.Images)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Roof> Roofs { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Main> Mains { get; set; }
    }
}
