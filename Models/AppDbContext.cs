using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedicineSystemAPI.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblDoctor> TblDoctors { get; set; }

    public virtual DbSet<TblMedicine> TblMedicines { get; set; }

    public virtual DbSet<TblPrescription> TblPrescriptions { get; set; }

    public virtual DbSet<TblStatus> TblStatuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("-- my connection string --");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblDoctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__TblDocto__2DC00EDF1EC26BD7");

            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Note)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Sip)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SIP");
            entity.Property(e => e.Speciality)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblMedicine>(entity =>
        {
            entity.HasKey(e => e.MedicineId).HasName("PK__TblMedic__4F2128F016CB6075");

            entity.ToTable("TblMedicine");

            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");
            entity.Property(e => e.Code)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Note)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<TblPrescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("PK__TblPresc__4013081222A12BAA");

            entity.ToTable("TblPrescription");

            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DoctorsId).HasColumnName("DoctorsID");
            entity.Property(e => e.Medicines).HasColumnType("text");
            entity.Property(e => e.Note)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PatientName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RefNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__TblStatu__C8EE2043D8F836E7");

            entity.ToTable("TblStatus");

            entity.Property(e => e.StatusId)
                .ValueGeneratedOnAdd()
                .HasColumnName("StatusID");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Note)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("PK__Users__1797D024D2DE8BB0");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DoctorId).HasDefaultValue(0);
            entity.Property(e => e.IsActive)
                .HasDefaultValue((byte)1)
                .HasColumnName("isActive");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PharmacistId).HasDefaultValue(0);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC07DF703B43");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
