using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace flip_flop.Models
{
    public partial class FlipFlopContext : DbContext
    {
        public FlipFlopContext()
        {
        }

        public FlipFlopContext(DbContextOptions<FlipFlopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AirPortCompany> AirPortCompany { get; set; }
        public virtual DbSet<Complains> Complains { get; set; }
        public virtual DbSet<ComplainsStatus> ComplainsStatus { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<PlainTickets> PlainTickets { get; set; }
        public virtual DbSet<Targets> Targets { get; set; }
        public virtual DbSet<TicketsHistory> TicketsHistory { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-SRR5P4I;Initial Catalog=FlipFlop;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AirPortCompany>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("text");
            });

            modelBuilder.Entity<Complains>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.UserKey).HasColumnName("User_key");

                entity.HasOne(d => d.UserKeyNavigation)
                    .WithMany(p => p.Complains)
                    .HasForeignKey(d => d.UserKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Complains_User");
            });

            modelBuilder.Entity<ComplainsStatus>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.Comments)
                    .IsRequired()
                    .HasColumnName("comments")
                    .HasColumnType("text");

                entity.Property(e => e.ComplainKey).HasColumnName("Complain_key");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasColumnType("text");

                entity.HasOne(d => d.ComplainKeyNavigation)
                    .WithMany(p => p.ComplainsStatus)
                    .HasForeignKey(d => d.ComplainKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComplainsStatus_Complains");
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasColumnName("Country_name")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnType("text");
            });

            modelBuilder.Entity<PlainTickets>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.ToTable("Plain_Tickets");

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.CancleFee).HasColumnName("Cancle_Fee");

                entity.Property(e => e.ClassKey).HasColumnName("Class_key");

                entity.Property(e => e.DateOfFlight)
                    .HasColumnName("Date_of_flight")
                    .HasColumnType("datetime");

                entity.Property(e => e.FlightNumber)
                    .IsRequired()
                    .HasColumnName("Flight_Number")
                    .HasColumnType("text");

                entity.Property(e => e.OwnerId).HasColumnName("Owner_ID");

                entity.Property(e => e.TargetKey).HasColumnName("Target_key");
            });

            modelBuilder.Entity<Targets>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasColumnName("City_Name")
                    .HasColumnType("text");

                entity.Property(e => e.CountryKey).HasColumnName("Country_key");

                entity.HasOne(d => d.CountryKeyNavigation)
                    .WithMany(p => p.Targets)
                    .HasForeignKey(d => d.CountryKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Targets_Countries");
            });

            modelBuilder.Entity<TicketsHistory>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.ToTable("Tickets_History");

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.DateOfTrade)
                    .HasColumnName("dateOfTrade")
                    .HasColumnType("date");

                entity.Property(e => e.KeyBuyer).HasColumnName("key_buyer");

                entity.Property(e => e.KeySeller).HasColumnName("key_seller");

                entity.Property(e => e.KeyTicket).HasColumnName("key_ticket");

                entity.HasOne(d => d.KeyBuyerNavigation)
                    .WithMany(p => p.TicketsHistoryKeyBuyerNavigation)
                    .HasForeignKey(d => d.KeyBuyer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tickets_History_BuyUser");

                entity.HasOne(d => d.KeySellerNavigation)
                    .WithMany(p => p.TicketsHistoryKeySellerNavigation)
                    .HasForeignKey(d => d.KeySeller)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tickets_History_SellUser");

                entity.HasOne(d => d.KeyTicketNavigation)
                    .WithMany(p => p.TicketsHistory)
                    .HasForeignKey(d => d.KeyTicket)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tickets_History_Tickets");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("text");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("First_name")
                    .HasColumnType("text");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("Last_Name")
                    .HasColumnType("text");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("Phone_Number")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });
        }
    }
}
