﻿// <auto-generated />
using ForumApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ForumApp.Infrastructure.Migrations
{
    [DbContext(typeof(ForumAppDbContext))]
    partial class ForumAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ForumApp.Infrastructure.Data.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Post Identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)")
                        .HasComment("Post Content");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Post Title");

                    b.HasKey("Id");

                    b.ToTable("Posts");

                    b.HasComment("Posts table");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Content = "My first post content",
                            Title = "My First Post"
                        },
                        new
                        {
                            Id = 2,
                            Content = "My second post content",
                            Title = "My Second Post"
                        },
                        new
                        {
                            Id = 3,
                            Content = "My third post content",
                            Title = "My Third Post"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
