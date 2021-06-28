using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Solve> Solves { get; set; }
        public DbSet<StatusCode> StatusCodes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>()
             .HasMany(ct => ct.Case)
             .WithOne(cs => cs.Category);

            modelBuilder.Entity<StatusCode>()
            .HasMany(sc => sc.Case)
            .WithOne(cs => cs.StatusCode);

            modelBuilder.Entity<Priority>()
            .HasMany(p => p.Case)
            .WithOne(cs => cs.Priority);

            modelBuilder.Entity<Case>()
            .HasOne(cs => cs.Solve)
            .WithOne(s => s.Case)
            .HasForeignKey<Solve>(s => s.Id);

            modelBuilder.Entity<Case>()
            .HasOne(cs => cs.Attachment)
            .WithOne(a => a.Case)
            .HasForeignKey<Attachment>(s => s.Id);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(b => b.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(c => c.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
