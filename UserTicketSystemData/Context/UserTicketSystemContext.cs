using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using UserTicketSystemCore.Models;

namespace UserTicketSystemData
{
    public class UserTicketSystemContext : DbContext
    {
        public UserTicketSystemContext(DbContextOptions<UserTicketSystemContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationHelper.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<UserHierarchy>()
                .HasOne(uh => uh.User)
                .WithMany(u => u.ReportingUsers)
                .HasForeignKey(uh => uh.UserId);

            modelBuilder.Entity<UserHierarchy>()
                .HasOne(uh => uh.ReportingUser)
                .WithMany(u => u.ReportedUsers)
                .HasForeignKey(uh => uh.ReportingUserId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.CreatedByUser)
                .WithMany(u => u.CreatedTickets)
                .HasForeignKey(t => t.CreatedBy);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.UpdatedByUser)
                .WithMany(u => u.UpdatedTickets)
                .HasForeignKey(t => t.UpdatedBy);

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<TicketType>()
                .HasIndex(tt => tt.Name)
                .IsUnique();

            modelBuilder.Entity<TicketStatus>()
                .HasIndex(ts => ts.Name)
                .IsUnique();
        }
    }
}
