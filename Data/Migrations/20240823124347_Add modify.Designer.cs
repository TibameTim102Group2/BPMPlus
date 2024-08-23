﻿// <auto-generated />
using System;
using BPMPlus.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240823124347_Add modify")]
    partial class Addmodify
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BPMPlus.Models.Category", b =>
                {
                    b.Property<string>("CategoryId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("CategoryDescription")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("NVARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("CategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("BPMPlus.Models.Department", b =>
                {
                    b.Property<string>("DepartmentId")
                        .HasMaxLength(20)
                        .HasColumnType("varchar");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime");

                    b.HasKey("DepartmentId");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("BPMPlus.Models.Form", b =>
                {
                    b.Property<string>("FormId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("CategoryId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("NVARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartmentId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("ExpectedFinishedDay")
                        .HasColumnType("datetime2");

                    b.Property<bool>("FormIsActive")
                        .HasColumnType("bit");

                    b.Property<string>("HandleDepartmentId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("ProcessNodeId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("ProjectId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Tel")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("NVARCHAR");

                    b.Property<int>("man_day")
                        .HasColumnType("int");

                    b.HasKey("FormId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProcessNodeId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Form");
                });

            modelBuilder.Entity("BPMPlus.Models.FormRecord", b =>
                {
                    b.Property<string>("ProcessingRecordId")
                        .HasMaxLength(20)
                        .HasColumnType("varchar");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("DepartmentId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("FormId")
                        .HasMaxLength(20)
                        .HasColumnType("varchar");

                    b.Property<string>("FunctionId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("GradeId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar");

                    b.Property<string>("ResultId")
                        .HasMaxLength(20)
                        .HasColumnType("varchar");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("ProcessingRecordId");

                    b.HasIndex("FormId");

                    b.HasIndex("FunctionId");

                    b.HasIndex("GradeId");

                    b.HasIndex("ResultId");

                    b.ToTable("FormRecord");
                });

            modelBuilder.Entity("BPMPlus.Models.Function", b =>
                {
                    b.Property<string>("FunctionId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FunctionDescription")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("FunctionId");

                    b.ToTable("Function");
                });

            modelBuilder.Entity("BPMPlus.Models.Grade", b =>
                {
                    b.Property<string>("GradeId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("GradeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("GradeId");

                    b.ToTable("Grade");
                });

            modelBuilder.Entity("BPMPlus.Models.Group", b =>
                {
                    b.Property<string>("GroupId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("NVARCHAR");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("GroupId");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("BPMPlus.Models.Meeting", b =>
                {
                    b.Property<string>("MeetingId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("MeetingRoom")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("MeetingId");

                    b.HasIndex("MeetingRoom");

                    b.ToTable("Meeting");
                });

            modelBuilder.Entity("BPMPlus.Models.MeetingRooms", b =>
                {
                    b.Property<string>("MeetingRoom")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<int>("Accomadation")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("MeetingRoom");

                    b.ToTable("MeetingRooms");
                });

            modelBuilder.Entity("BPMPlus.Models.ProcessNode", b =>
                {
                    b.Property<string>("ProcessNodeId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartmentId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("FunctionId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("ProcessNodeId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("FunctionId");

                    b.ToTable("ProcessNode");
                });

            modelBuilder.Entity("BPMPlus.Models.ProcessTemplate", b =>
                {
                    b.Property<string>("ProcessNodeId")
                        .HasMaxLength(20)
                        .HasColumnType("varchar");

                    b.Property<string>("CategoryId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("FunctionId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime");

                    b.HasKey("ProcessNodeId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("FunctionId");

                    b.ToTable("ProcessTemplate");
                });

            modelBuilder.Entity("BPMPlus.Models.Project", b =>
                {
                    b.Property<string>("ProjectId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeadLine")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("NCHAR");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ProjectId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("BPMPlus.Models.Result", b =>
                {
                    b.Property<string>("ResultId")
                        .HasMaxLength(20)
                        .HasColumnType("varchar");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("ResultDescription")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime");

                    b.HasKey("ResultId");

                    b.ToTable("Result");
                });

            modelBuilder.Entity("BPMPlus.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartmentId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("GradeId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TEL")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("UserIsActive")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("GradeId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("MeetingUser", b =>
                {
                    b.Property<string>("MeetingsMeetingId")
                        .HasColumnType("VARCHAR(20)");

                    b.Property<string>("UsersId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MeetingsMeetingId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("MeetingUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BPMPlus.Models.Form", b =>
                {
                    b.HasOne("BPMPlus.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("BPMPlus.Models.ProcessNode", "ProcessNode")
                        .WithMany()
                        .HasForeignKey("ProcessNodeId");

                    b.HasOne("BPMPlus.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");

                    b.Navigation("Category");

                    b.Navigation("ProcessNode");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("BPMPlus.Models.FormRecord", b =>
                {
                    b.HasOne("BPMPlus.Models.Form", "Form")
                        .WithMany()
                        .HasForeignKey("FormId");

                    b.HasOne("BPMPlus.Models.Function", "Function")
                        .WithMany()
                        .HasForeignKey("FunctionId");

                    b.HasOne("BPMPlus.Models.Grade", "Grade")
                        .WithMany()
                        .HasForeignKey("GradeId");

                    b.HasOne("BPMPlus.Models.Result", "Result")
                        .WithMany()
                        .HasForeignKey("ResultId");

                    b.Navigation("Form");

                    b.Navigation("Function");

                    b.Navigation("Grade");

                    b.Navigation("Result");
                });

            modelBuilder.Entity("BPMPlus.Models.Meeting", b =>
                {
                    b.HasOne("BPMPlus.Models.MeetingRooms", "MeetingRooms")
                        .WithMany()
                        .HasForeignKey("MeetingRoom");

                    b.Navigation("MeetingRooms");
                });

            modelBuilder.Entity("BPMPlus.Models.ProcessNode", b =>
                {
                    b.HasOne("BPMPlus.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");

                    b.HasOne("BPMPlus.Models.Function", "Function")
                        .WithMany()
                        .HasForeignKey("FunctionId");

                    b.Navigation("Department");

                    b.Navigation("Function");
                });

            modelBuilder.Entity("BPMPlus.Models.ProcessTemplate", b =>
                {
                    b.HasOne("BPMPlus.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("BPMPlus.Models.Function", "Function")
                        .WithMany()
                        .HasForeignKey("FunctionId");

                    b.Navigation("Category");

                    b.Navigation("Function");
                });

            modelBuilder.Entity("BPMPlus.Models.User", b =>
                {
                    b.HasOne("BPMPlus.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");

                    b.HasOne("BPMPlus.Models.Grade", "Grade")
                        .WithMany()
                        .HasForeignKey("GradeId");

                    b.Navigation("Department");

                    b.Navigation("Grade");
                });

            modelBuilder.Entity("MeetingUser", b =>
                {
                    b.HasOne("BPMPlus.Models.Meeting", null)
                        .WithMany()
                        .HasForeignKey("MeetingsMeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BPMPlus.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BPMPlus.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BPMPlus.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BPMPlus.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BPMPlus.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
