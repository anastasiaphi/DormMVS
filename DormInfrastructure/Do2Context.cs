using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DormDomain.Model;

public partial class Do2Context : DbContext
{
    public Do2Context()
    {
    }

    public Do2Context(DbContextOptions<Do2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Degree> Degrees { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentsStatus> PaymentsStatuses { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomAssignment> RoomAssignments { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Tariff> Tariffs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=DO2; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Degree>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Degrees__3214EC2730044CA2");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DegreeName)
                .HasMaxLength(1)
                .HasColumnName("degreeName");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC273AB1EDE5");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FacultyId).HasColumnName("FacultyID");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Faculty).WithMany(p => p.Departments)
                .HasForeignKey(d => d.FacultyId)
                .HasConstraintName("FK__Departmen__Facul__286302EC");
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Facultie__3214EC2740FFC535");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC278CCA5A5F");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.PaymentDate).HasColumnName("paymentDate");
            entity.Property(e => e.PaymentsStatusId).HasColumnName("paymentsStatusID");
            entity.Property(e => e.RoomAssignmentId).HasColumnName("roomAssignmentID");

            entity.HasOne(d => d.PaymentsStatus).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentsStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_PaymentsStatus");

            entity.HasOne(d => d.RoomAssignment).WithMany(p => p.Payments)
                .HasForeignKey(d => d.RoomAssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_RoomAssignments");
        });

        modelBuilder.Entity<PaymentsStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC27AB8C418E");

            entity.ToTable("PaymentsStatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(1)
                .HasColumnName("statusName");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rooms__3214EC2754F5C26E");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.RoomNum).HasColumnName("roomNum");
            entity.Property(e => e.TariffsId).HasColumnName("tariffsID");

            entity.HasOne(d => d.Tariffs).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.TariffsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rooms_Tariffs");
        });

        modelBuilder.Entity<RoomAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoomAssi__3214EC272031FB5E");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CheckInDate).HasColumnName("checkInDate");
            entity.Property(e => e.CheckOutDate).HasColumnName("checkOutDate");
            entity.Property(e => e.RoomId).HasColumnName("roomID");
            entity.Property(e => e.StudentId).HasColumnName("studentID");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomAssignments)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomAssignments_Rooms");

            entity.HasOne(d => d.Student).WithMany(p => p.RoomAssignments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomAssignments_Students");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC27A324E11F");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DegreeId).HasColumnName("degreeID");
            entity.Property(e => e.DepartmentId).HasColumnName("departmentID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("lastName");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middleName");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phoneNumber");

            entity.HasOne(d => d.Degree).WithMany(p => p.Students)
                .HasForeignKey(d => d.DegreeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_Degrees");

            entity.HasOne(d => d.Department).WithMany(p => p.Students)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_Departments");
        });

        modelBuilder.Entity<Tariff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tariffs__3214EC270C844032");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PricePerMonth)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("pricePerMonth");
            entity.Property(e => e.TariffsName)
                .HasMaxLength(1)
                .HasColumnName("tariffsName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
