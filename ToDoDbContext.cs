using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace TodoApi;

public partial class ToDoDbContext : DbContext
{
    public ToDoDbContext()
    {
    }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("Server=bjkelasuimbhk5uk1llj-mysql.services.clever-cloud.com;Port=3306;User=u16sbkz97rt4sygi;Password=NImuUmo0RkuRJG0FO9Tc;Database=bjkelasuimbhk5uk1llj;", 
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("items");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Idusers).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.Idusers)
                .ValueGeneratedNever()
                .HasColumnName("idusers");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(45)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
