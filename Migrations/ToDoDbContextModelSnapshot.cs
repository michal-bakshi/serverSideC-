﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoApi;

#nullable disable

namespace TodoApi.Migrations
{
    [DbContext(typeof(ToDoDbContext))]
    partial class ToDoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("TodoApi.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("IsComplete")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("items", (string)null);
                });

            modelBuilder.Entity("TodoApi.User", b =>
                {
                    b.Property<int>("Idusers")
                        .HasColumnType("int")
                        .HasColumnName("idusers");

                    b.Property<string>("Email")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("password");

                    b.Property<string>("Role")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("role");

                    b.HasKey("Idusers")
                        .HasName("PRIMARY");

                    b.ToTable("users", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
