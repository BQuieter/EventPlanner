using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerServer.Models;

public partial class EventPlannerDbContext : DbContext
{
    public EventPlannerDbContext()
    {
    }

    public EventPlannerDbContext(DbContextOptions<EventPlannerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActionType> ActionTypes { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventImportance> EventImportances { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ActionTy__3214EC07B99C7647");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events__3214EC07BAE8AC4E");

            entity.Property(e => e.DateTime).HasColumnType("smalldatetime");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Owner)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.ImportanceNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.Importance)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__Importan__5FB337D6");

            entity.HasOne(d => d.OwnerNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.Owner)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__Owner__5EBF139D");
        });

        modelBuilder.Entity<EventImportance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventImp__3214EC0703AC13BB");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC074D806094");

            entity.Property(e => e.DateTime).HasColumnType("smalldatetime");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Owner)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.OwnerNavigation).WithMany(p => p.Logs)
                .HasForeignKey(d => d.Owner)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Logs__Owner__6477ECF3");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Logs)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Logs__Type__656C112C");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC0780AD4B43");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.RoleName).HasMaxLength(20);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Login).HasName("PK__Users__5E55825A172605B0");

            entity.Property(e => e.Login)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasDefaultValue((byte)2);

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__Role__4CA06362");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
