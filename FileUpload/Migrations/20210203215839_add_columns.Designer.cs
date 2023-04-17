﻿// <auto-generated />
using System;
using FileUpload.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FileUpload.Migrations
{
    [DbContext(typeof(PCC_FITContext))]
    [Migration("20210203215839_add_columns")]
    partial class add_columns
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("app")
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("FileUpload.Models.FilesOnDatabase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Data")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Extension")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("FY")
                        .HasColumnType("int");

                    b.Property<int>("FacilityID")
                        .HasColumnType("int");

                    b.Property<string>("FileType")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UploadedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .HasDatabaseName("IX_Extension");

                    b.HasIndex("Name", "Extension")
                        .IsUnique()
                        .HasDatabaseName("IX_FileNameExtension")
                        .HasFilter("[Name] IS NOT NULL AND [Extension] IS NOT NULL");

                    b.HasIndex("Name", "FileType")
                        .IsUnique()
                        .HasDatabaseName("IX_FileNameFileType")
                        .HasFilter("[Name] IS NOT NULL AND [FileType] IS NOT NULL");

                    b.ToTable("FilesOnDatabase");
                });

            modelBuilder.Entity("FileUpload.Models.FilesOnFileSystem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Extension")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("FY")
                        .HasColumnType("int");

                    b.Property<int>("FacilityID")
                        .HasColumnType("int");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileType")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UploadedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .HasDatabaseName("IX_Extension");

                    b.HasIndex("Name", "Extension")
                        .IsUnique()
                        .HasDatabaseName("IX_FileNameExtension")
                        .HasFilter("[Name] IS NOT NULL AND [Extension] IS NOT NULL");

                    b.HasIndex("Name", "FileType")
                        .IsUnique()
                        .HasDatabaseName("IX_FileNameFileType")
                        .HasFilter("[Name] IS NOT NULL AND [FileType] IS NOT NULL");

                    b.ToTable("FilesOnFileSystem");
                });
#pragma warning restore 612, 618
        }
    }
}
