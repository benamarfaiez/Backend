using Microsoft.EntityFrameworkCore;
using Reservation.Dal.Entities;

namespace Reservation.Dal;

public partial class ReservationContext : DbContext
{
    public ReservationContext(DbContextOptions<ReservationContext> options) : base(options)
    {
    }

    public virtual DbSet<BookingEntity> Bookings { get; set; }

    public virtual DbSet<PersonEntity> People { get; set; }

    public virtual DbSet<RoomEntity> Rooms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookingEntity>(entity =>
        {
            entity.ToTable("Booking");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.HasOne(d => d.Person).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_Person");

            entity.HasOne(d => d.Room).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_Room");
        });

        modelBuilder.Entity<PersonEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User");

            entity.ToTable("Person");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RoomEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Room");

            entity.ToTable("Room");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.RoomName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        base.OnModelCreating(modelBuilder);
    }

}
