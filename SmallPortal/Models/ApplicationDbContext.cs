using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmallPortal.Models;

namespace SmallPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SmallPortal.Models.Recipient1099> Recipient1099 { get; set; }
        public DbSet<SmallPortal.Models.Recipient> Recipient { get; set; }
        public DbSet<SmallPortal.Models.BoxValues> Boxvalues { get; set; }
        public DbSet<SmallPortal.Models.Recipient1099InputModel> Recipient1099InputModel { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}