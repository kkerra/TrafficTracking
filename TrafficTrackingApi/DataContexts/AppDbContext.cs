using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TrafficTrackingApi.Models;

namespace TrafficTrackingApi.DataContexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventLog> EventLogs { get; set; }

    public virtual DbSet<EventTrafficImpactView> EventTrafficImpactViews { get; set; }

    public virtual DbSet<Intersection> Intersections { get; set; }

    public virtual DbSet<TrafficLight> TrafficLights { get; set; }

    public virtual DbSet<TrafficLightSummaryView> TrafficLightSummaryViews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=pr;Trusted_Connection=True; Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Event__7944C810781F0D53");

            entity.ToTable("Event", tb => tb.HasTrigger("tr_AddedEvent"));

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<EventLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__EventLog__5E54864857C5B2D9");

            entity.ToTable("EventLog");

            entity.Property(e => e.EventType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LogMessage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LogTimestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<EventTrafficImpactView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("EventTrafficImpactView");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.EventId).ValueGeneratedOnAdd();
            entity.Property(e => e.EventType).HasMaxLength(100);
        });

        modelBuilder.Entity<Intersection>(entity =>
        {
            entity.HasKey(e => e.IntersectionId).HasName("PK__Intersec__DAE29894A17E586E");

            entity.ToTable("Intersection");

            entity.HasIndex(e => new { e.Latitude, e.Longitude }, "UQ_Intersection_Latitude_Longitude").IsUnique();

            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");

            entity.HasMany(d => d.Events).WithMany(p => p.Intersections)
                .UsingEntity<Dictionary<string, object>>(
                    "IntersectionEvent",
                    r => r.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Intersect__Event__3F466844"),
                    l => l.HasOne<Intersection>().WithMany()
                        .HasForeignKey("IntersectionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Intersect__Inter__3E52440B"),
                    j =>
                    {
                        j.HasKey("IntersectionId", "EventId").HasName("PK__Intersec__CD76D41585E1E9F6");
                        j.ToTable("IntersectionEvent");
                    });
        });

        modelBuilder.Entity<TrafficLight>(entity =>
        {
            entity.HasKey(e => e.TrafficLightId).HasName("PK__TrafficL__E79FBD3B895FDDFB");

            entity.ToTable("TrafficLight");

            entity.Property(e => e.State).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(100);

            entity.HasOne(d => d.Intersection).WithMany(p => p.TrafficLights)
                .HasForeignKey(d => d.IntersectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TrafficLi__Inter__3B75D760");
        });

        modelBuilder.Entity<TrafficLightSummaryView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TrafficLightSummaryView");

            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.TrafficLightState).HasMaxLength(50);
            entity.Property(e => e.TrafficLightType).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
