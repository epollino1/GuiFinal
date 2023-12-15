using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FitnisTracker.Models;

public partial class FitnisContext : DbContext
{
    public FitnisContext()
    {
    }

    public FitnisContext(DbContextOptions<FitnisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CaloryLog> CaloryLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WeightLog> WeightLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("DataSource=fitnis.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CaloryLog>(entity =>
        {
            entity.ToTable("CaloryLog");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("INTEGER AUTO INCREMENT")
                .HasColumnName("id");
            entity.Property(e => e.LoggedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("TIMESTAMP")
                .HasColumnName("logged_at");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.CaloryLogs).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Activity).HasColumnName("activity");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Birthday)
                .HasColumnType("Date")
                .HasColumnName("birthday");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.HeightIn).HasColumnName("Height_in");
        });

        modelBuilder.Entity<WeightLog>(entity =>
        {
            entity.ToTable("WeightLog");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("INTEGER AUTO INCREMENT")
                .HasColumnName("id");
            entity.Property(e => e.CurrentWeight).HasColumnName("currentWeight");
            entity.Property(e => e.LoggedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("TIMESTAMP")
                .HasColumnName("logged_at");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.WeightLogs).HasForeignKey(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
