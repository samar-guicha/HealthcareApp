using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HealthcareApp.Models.Health;

public partial class HealthDBContext : DbContext
{
    public HealthDBContext()
    {
    }

    public HealthDBContext(DbContextOptions<HealthDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; } 
    public virtual DbSet<Doctor> Doctors { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Name=ConnectionStrings:HealthCS");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable("Appointment");

            entity.Property(e => e.Description).HasMaxLength(50);

            entity.Property(e => e.Location).HasMaxLength(50);

            entity.HasOne(d => d.IdDoctorNavigation)
                .WithMany(a => a.Appointments)
                .HasForeignKey(d => d.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointment_Doctor");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("Doctor");

            entity.Property(e => e.LicenseNumber)
                .HasMaxLength(50)
                .HasColumnName("LicenseNumber ");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("Name ");

            entity.Property(e => e.Specialization)
                .HasMaxLength(50)
                .HasColumnName("Specialization ");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
