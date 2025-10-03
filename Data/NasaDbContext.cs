using Microsoft.EntityFrameworkCore;
using SharkTracking.Core.Entities;
using SharkTracking.Core.Entities.SharkTracking.InfrastructureData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.InfrastructureData.Data
{
    public class NasaDbContext : DbContext
    {
        public NasaDbContext(DbContextOptions<NasaDbContext> options) : base(options) { }

        // DbSets
        public DbSet<Shark> Sharks { get; set; }
        public DbSet<SharkTagData> SharkTagData { get; set; }
        public DbSet<SatelliteData> SatelliteData { get; set; }
        public DbSet<PredictionAlert> PredictionAlerts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAlert> UserAlerts { get; set; }
        public DbSet<SharkMedia> SharkMedia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Shark -> SharkTagData (1-to-Many)
            modelBuilder.Entity<Shark>()
                .HasMany(s => s.SharkTagData)
                .WithOne(t => t.Shark)
                .HasForeignKey(t => t.SharkId)
                .OnDelete(DeleteBehavior.Cascade);

            // Shark -> PredictionAlerts (1-to-Many)
            modelBuilder.Entity<Shark>()
                .HasMany(s => s.PredictionAlerts)
                .WithOne(a => a.Shark)
                .HasForeignKey(a => a.SharkId)
                .OnDelete(DeleteBehavior.SetNull);

            // Many-to-Many (User <-> PredictionAlert) via UserAlert
            modelBuilder.Entity<UserAlert>()
                .HasKey(ua => new { ua.UserId, ua.PredictionAlertId });

            modelBuilder.Entity<UserAlert>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAlerts)
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserAlert>()
                .HasOne(ua => ua.PredictionAlert)
                .WithMany(a => a.UserAlerts)
                .HasForeignKey(ua => ua.PredictionAlertId);

            // SatelliteData is standalone (no special relations for now)
        }
    }
}
