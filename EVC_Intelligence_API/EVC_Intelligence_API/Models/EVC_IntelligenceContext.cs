using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace EVC_Intelligence_API.Models
{
    public partial class EVC_IntelligenceContext : DbContext
    {
        public EVC_IntelligenceContext()
        {
        }

        public EVC_IntelligenceContext(DbContextOptions<EVC_IntelligenceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookingDetail> BookingDetails { get; set; }
        public virtual DbSet<ChargingStation> ChargingStations { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<LoginDatum> LoginData { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<UserDetail> UserDetails { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BookingDetail>(entity =>
            {
                entity.HasKey(e => e.BookingId);

                entity.ToTable("Booking_Details");

                entity.Property(e => e.BookingId).HasColumnName("Booking_Id");

                entity.Property(e => e.CounterId).HasColumnName("Counter_Id");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.SlotId).HasColumnName("Slot_Id");

                entity.Property(e => e.SlotTime).HasColumnName("Slot_Time");

                entity.Property(e => e.StationId).HasColumnName("Station_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.VehicleType)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Vehicle_Type");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.BookingDetails)
                    .HasForeignKey(d => d.StationId)
                    .HasConstraintName("FK_Booking_Details_Charging_Stations");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BookingDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Booking_Details_Login_Data");
            });

            modelBuilder.Entity<ChargingStation>(entity =>
            {
                entity.HasKey(e => e.StationId);

                entity.ToTable("Charging_Stations");

                entity.Property(e => e.StationId).HasColumnName("Station_Id");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CityId).HasColumnName("City_Id");

                entity.Property(e => e.ClosingTime).HasColumnName("Closing_Time");

                entity.Property(e => e.OpenClose).HasColumnName("Open_Close");

                entity.Property(e => e.OpeningTime).HasColumnName("Opening_Time");

                entity.Property(e => e.StationName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Station_Name");

                entity.Property(e => e.TotalCounters).HasColumnName("Total_Counters");

                entity.Property(e => e.TotalSlotsPerCounter).HasColumnName("Total_Slots_Per_Counter");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.ChargingStations)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Charging_Stations_City");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.Property(e => e.CityId).HasColumnName("City_Id");

                entity.Property(e => e.CityName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("City_Name");

                entity.Property(e => e.StateId).HasColumnName("State_Id");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("FK_City_State");
            });

            modelBuilder.Entity<LoginDatum>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("Login_Data");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.Property(e => e.Answer)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.Property(e => e.SecurityQuestion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Security_Question");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.LoginData)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Login_Data_User_Roles");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.Property(e => e.StateId).HasColumnName("State_Id");

                entity.Property(e => e.StateName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("State_Name");
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.ToTable("User_Detail");

                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber).HasColumnName("Phone_Number");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserDetails)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Detail_Login_Data");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("User_Roles");

                entity.Property(e => e.RoleId).HasColumnName("Role_Id");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Role_Name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
