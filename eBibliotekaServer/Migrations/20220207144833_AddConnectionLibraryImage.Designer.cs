﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eBibliotekaServer.Data;

namespace eBibliotekaServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220207144833_AddConnectionLibraryImage")]
    partial class AddConnectionLibraryImage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("eBibliotekaServer.AuthModule.Models.Account", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Accounts");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Account");
                });

            modelBuilder.Entity("eBibliotekaServer.ImageModule.Models.Image", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("eBibliotekaServer.LibraryModule.Models.Library", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AboutLong")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AboutShort")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("BannerImageID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProfileImageID")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("BannerImageID");

                    b.HasIndex("ProfileImageID");

                    b.ToTable("Libraries");
                });

            modelBuilder.Entity("eBibliotekaServer.AuthModule.Models.Librarian", b =>
                {
                    b.HasBaseType("eBibliotekaServer.AuthModule.Models.Account");

                    b.Property<int>("LibraryID")
                        .HasColumnType("int");

                    b.HasIndex("LibraryID");

                    b.HasDiscriminator().HasValue("Librarian");
                });

            modelBuilder.Entity("eBibliotekaServer.AuthModule.Models.User", b =>
                {
                    b.HasBaseType("eBibliotekaServer.AuthModule.Models.Account");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("User");
                });

            modelBuilder.Entity("eBibliotekaServer.LibraryModule.Models.Library", b =>
                {
                    b.HasOne("eBibliotekaServer.ImageModule.Models.Image", "BannerImage")
                        .WithMany()
                        .HasForeignKey("BannerImageID");

                    b.HasOne("eBibliotekaServer.ImageModule.Models.Image", "ProfileImage")
                        .WithMany()
                        .HasForeignKey("ProfileImageID");

                    b.Navigation("BannerImage");

                    b.Navigation("ProfileImage");
                });

            modelBuilder.Entity("eBibliotekaServer.AuthModule.Models.Librarian", b =>
                {
                    b.HasOne("eBibliotekaServer.LibraryModule.Models.Library", "Library")
                        .WithMany()
                        .HasForeignKey("LibraryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Library");
                });
#pragma warning restore 612, 618
        }
    }
}
