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
    [Migration("20240916063653_ProjectUserUpdate")]
    partial class ProjectUserUpdate
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
                        .HasColumnType("datetime");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime");

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

                    b.Property<DateTime>("UpdatedTime")
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
                        .HasColumnType("datetime");

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

                    b.Property<int?>("ManDay")
                        .HasColumnType("int");

                    b.Property<string>("ProcessNodeId")
                        .IsRequired()
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
                        .HasColumnType("datetime");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.HasKey("FormId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

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

                    b.Property<string>("GradeId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Remark")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar");

                    b.Property<string>("ResultId")
                        .HasMaxLength(20)
                        .HasColumnType("varchar");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("UserActivityId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.HasKey("ProcessingRecordId");

                    b.HasIndex("FormId");

                    b.HasIndex("GradeId");

                    b.HasIndex("ResultId");

                    b.HasIndex("UserActivityId");

                    b.HasIndex("UserId");

                    b.ToTable("FormRecord");
                });

            modelBuilder.Entity("BPMPlus.Models.Grade", b =>
                {
                    b.Property<string>("GradeId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("GradeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime");

                    b.HasKey("GradeId");

                    b.ToTable("Grade");
                });

            modelBuilder.Entity("BPMPlus.Models.Meeting", b =>
                {
                    b.Property<string>("MeetingId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("MeetingRoomId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime");

                    b.HasKey("MeetingId");

                    b.HasIndex("MeetingRoomId");

                    b.ToTable("Meeting");
                });

            modelBuilder.Entity("BPMPlus.Models.MeetingRooms", b =>
                {
                    b.Property<string>("MeetingRoomId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<int>("Accomodation")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime");

                    b.HasKey("MeetingRoomId");

                    b.ToTable("MeetingRooms");
                });

            modelBuilder.Entity("BPMPlus.Models.PermissionGroup", b =>
                {
                    b.Property<string>("PermissionGroupId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("PermissionGroupName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("NVARCHAR");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime");

                    b.HasKey("PermissionGroupId");

                    b.ToTable("PermissionGroup");
                });

            modelBuilder.Entity("BPMPlus.Models.ProcessNode", b =>
                {
                    b.Property<string>("ProcessNodeId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("DepartmentId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("FormId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("UserActivityId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.HasKey("ProcessNodeId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("FormId");

                    b.HasIndex("UserActivityId");

                    b.ToTable("ProcessNode");
                });

            modelBuilder.Entity("BPMPlus.Models.ProcessTemplate", b =>
                {
                    b.Property<string>("ProcessTemplateId")
                        .HasMaxLength(20)
                        .HasColumnType("varchar");

                    b.Property<string>("CategoryId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("DepartmentId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("UserActivityId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.HasKey("ProcessTemplateId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserActivityId");

                    b.ToTable("ProcessTemplate");
                });

            modelBuilder.Entity("BPMPlus.Models.Project", b =>
                {
                    b.Property<string>("ProjectId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("DeadLine")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProjectManagerId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("NVARCHAR");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("NCHAR");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime");

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

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime");

                    b.HasKey("ResultId");

                    b.ToTable("Result");
                });

            modelBuilder.Entity("BPMPlus.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("DepartmentId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("GradeId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TEL")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime");

                    b.Property<bool>("UserIsActive")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("UserId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("GradeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BPMPlus.Models.UserActivity", b =>
                {
                    b.Property<string>("UserActivityId")
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime");

                    b.Property<string>("UserActivityIdDescription")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("UserActivityId");

                    b.ToTable("UserActivity");
                });

            modelBuilder.Entity("MeetingUser", b =>
                {
                    b.Property<string>("MeetingsMeetingId")
                        .HasColumnType("VARCHAR(20)");

                    b.Property<string>("UsersUserId")
                        .HasColumnType("VARCHAR(20)");

                    b.HasKey("MeetingsMeetingId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("MeetingUser");
                });

            modelBuilder.Entity("PermissionGroupUser", b =>
                {
                    b.Property<string>("PermissionGroupsPermissionGroupId")
                        .HasColumnType("VARCHAR(20)");

                    b.Property<string>("UsersUserId")
                        .HasColumnType("VARCHAR(20)");

                    b.HasKey("PermissionGroupsPermissionGroupId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("PermissionGroupUser", (string)null);
                });

            modelBuilder.Entity("PermissionGroupUserActivity", b =>
                {
                    b.Property<string>("PermissionGroupsPermissionGroupId")
                        .HasColumnType("VARCHAR(20)");

                    b.Property<string>("UserActivitiesUserActivityId")
                        .HasColumnType("VARCHAR(20)");

                    b.HasKey("PermissionGroupsPermissionGroupId", "UserActivitiesUserActivityId");

                    b.HasIndex("UserActivitiesUserActivityId");

                    b.ToTable("PermissionGroupUserActivity", (string)null);
                });

            modelBuilder.Entity("ProjectUser", b =>
                {
                    b.Property<string>("ProjectsProjectId")
                        .HasColumnType("VARCHAR(20)");

                    b.Property<string>("UsersUserId")
                        .HasColumnType("VARCHAR(20)");

                    b.HasKey("ProjectsProjectId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("ProjectUser", (string)null);
                });

            modelBuilder.Entity("BPMPlus.Models.Form", b =>
                {
                    b.HasOne("BPMPlus.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("BPMPlus.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BPMPlus.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");

                    b.HasOne("BPMPlus.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Department");

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BPMPlus.Models.FormRecord", b =>
                {
                    b.HasOne("BPMPlus.Models.Form", "Form")
                        .WithMany("FormRecord")
                        .HasForeignKey("FormId");

                    b.HasOne("BPMPlus.Models.Grade", "Grade")
                        .WithMany()
                        .HasForeignKey("GradeId");

                    b.HasOne("BPMPlus.Models.Result", "Result")
                        .WithMany()
                        .HasForeignKey("ResultId");

                    b.HasOne("BPMPlus.Models.UserActivity", "UserActivity")
                        .WithMany()
                        .HasForeignKey("UserActivityId");

                    b.HasOne("BPMPlus.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Form");

                    b.Navigation("Grade");

                    b.Navigation("Result");

                    b.Navigation("User");

                    b.Navigation("UserActivity");
                });

            modelBuilder.Entity("BPMPlus.Models.Meeting", b =>
                {
                    b.HasOne("BPMPlus.Models.MeetingRooms", "MeetingRooms")
                        .WithMany()
                        .HasForeignKey("MeetingRoomId");

                    b.Navigation("MeetingRooms");
                });

            modelBuilder.Entity("BPMPlus.Models.ProcessNode", b =>
                {
                    b.HasOne("BPMPlus.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");

                    b.HasOne("BPMPlus.Models.Form", "Form")
                        .WithMany("ProcessNode")
                        .HasForeignKey("FormId");

                    b.HasOne("BPMPlus.Models.UserActivity", "UserActivity")
                        .WithMany("ProcessNode")
                        .HasForeignKey("UserActivityId");

                    b.Navigation("Department");

                    b.Navigation("Form");

                    b.Navigation("UserActivity");
                });

            modelBuilder.Entity("BPMPlus.Models.ProcessTemplate", b =>
                {
                    b.HasOne("BPMPlus.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("BPMPlus.Models.UserActivity", "UserActivity")
                        .WithMany()
                        .HasForeignKey("UserActivityId");

                    b.Navigation("Category");

                    b.Navigation("UserActivity");
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
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PermissionGroupUser", b =>
                {
                    b.HasOne("BPMPlus.Models.PermissionGroup", null)
                        .WithMany()
                        .HasForeignKey("PermissionGroupsPermissionGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BPMPlus.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PermissionGroupUserActivity", b =>
                {
                    b.HasOne("BPMPlus.Models.PermissionGroup", null)
                        .WithMany()
                        .HasForeignKey("PermissionGroupsPermissionGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BPMPlus.Models.UserActivity", null)
                        .WithMany()
                        .HasForeignKey("UserActivitiesUserActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectUser", b =>
                {
                    b.HasOne("BPMPlus.Models.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BPMPlus.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BPMPlus.Models.Form", b =>
                {
                    b.Navigation("FormRecord");

                    b.Navigation("ProcessNode");
                });

            modelBuilder.Entity("BPMPlus.Models.UserActivity", b =>
                {
                    b.Navigation("ProcessNode");
                });
#pragma warning restore 612, 618
        }
    }
}
