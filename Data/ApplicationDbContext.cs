﻿using BPMPlus.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BPMPlus.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration Config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseSqlServer(
                    Config.GetConnectionString("DefaultConnection"));
            }
        }
        public DbSet<BPMPlus.Models.Meeting> Meeting { get; set; }
        public DbSet<BPMPlus.Models.Form> Form { get; set; }
        public DbSet<BPMPlus.Models.ProcessNode> ProcessNodes { get; set; }
        public DbSet<BPMPlus.Models.Project> Project { get; set; }
        public DbSet<BPMPlus.Models.Category> Category { get; set; }
        public DbSet<BPMPlus.Models.Department> Department { get; set; }
        public DbSet<BPMPlus.Models.FormRecord> FormRecord { get; set; }
        public DbSet<BPMPlus.Models.UserActivity> UserActivity { get; set; }
        public DbSet<BPMPlus.Models.Grade> Grade { get; set; }
        public DbSet<BPMPlus.Models.Result> Result { get; set; }
        public DbSet<BPMPlus.Models.ProcessTemplate> ProcessTemplate { get; set; }
        public DbSet<BPMPlus.Models.MeetingRooms> MeetingRooms { get; set; }
        public DbSet<BPMPlus.Models.PermissionGroup> PermissionGroup { get; set; }
    }
}
