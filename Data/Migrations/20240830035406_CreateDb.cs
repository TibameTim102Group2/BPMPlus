using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    CategoryDescription = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    GradeId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    GradeName = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.GradeId);
                });

            migrationBuilder.CreateTable(
                name: "MeetingRooms",
                columns: table => new
                {
                    MeetingRoomId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Accomodation = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingRooms", x => x.MeetingRoomId);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroup",
                columns: table => new
                {
                    PermissionGroupId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    PermissionGroupName = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroup", x => x.PermissionGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ProjectId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    ProjectName = table.Column<string>(type: "NVARCHAR(64)", maxLength: 64, nullable: false),
                    Summary = table.Column<string>(type: "NCHAR(500)", maxLength: 500, nullable: false),
                    DeadLine = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Result",
                columns: table => new
                {
                    ResultId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    ResultDescription = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Result", x => x.ResultId);
                });

            migrationBuilder.CreateTable(
                name: "UserActivity",
                columns: table => new
                {
                    UserActivityId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    UserActivityIdDescription = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivity", x => x.UserActivityId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    EmployeeId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    GradeId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    UserIsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    TEL = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_EmployeeId", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Users_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentId");
                    table.ForeignKey(
                        name: "FK_Users_Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grade",
                        principalColumn: "GradeId");
                });

            migrationBuilder.CreateTable(
                name: "Meeting",
                columns: table => new
                {
                    MeetingId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    MeetingRoomId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meeting", x => x.MeetingId);
                    table.ForeignKey(
                        name: "FK_Meeting_MeetingRooms_MeetingRoomId",
                        column: x => x.MeetingRoomId,
                        principalTable: "MeetingRooms",
                        principalColumn: "MeetingRoomId");
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroupUserActivity",
                columns: table => new
                {
                    PermissionGroupsPermissionGroupId = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    UserActivitiesUserActivityId = table.Column<string>(type: "VARCHAR(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroupUserActivity", x => new { x.PermissionGroupsPermissionGroupId, x.UserActivitiesUserActivityId });
                    table.ForeignKey(
                        name: "FK_PermissionGroupUserActivity_PermissionGroup_PermissionGroupsPermissionGroupId",
                        column: x => x.PermissionGroupsPermissionGroupId,
                        principalTable: "PermissionGroup",
                        principalColumn: "PermissionGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGroupUserActivity_UserActivity_UserActivitiesUserActivityId",
                        column: x => x.UserActivitiesUserActivityId,
                        principalTable: "UserActivity",
                        principalColumn: "UserActivityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessNode",
                columns: table => new
                {
                    ProcessNodeId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    UserActivityId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    UserId = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    DepartmentId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessNode", x => x.ProcessNodeId);
                    table.ForeignKey(
                        name: "FK_ProcessNode_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentId");
                    table.ForeignKey(
                        name: "FK_ProcessNode_UserActivity_UserActivityId",
                        column: x => x.UserActivityId,
                        principalTable: "UserActivity",
                        principalColumn: "UserActivityId");
                });

            migrationBuilder.CreateTable(
                name: "ProcessTemplate",
                columns: table => new
                {
                    ProcessNodeId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    UserActivityId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    DepartmentId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    CategoryId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTemplate", x => x.ProcessNodeId);
                    table.ForeignKey(
                        name: "FK_ProcessTemplate_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_ProcessTemplate_UserActivity_UserActivityId",
                        column: x => x.UserActivityId,
                        principalTable: "UserActivity",
                        principalColumn: "UserActivityId");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroupUser",
                columns: table => new
                {
                    PermissionGroupsPermissionGroupId = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroupUser", x => new { x.PermissionGroupsPermissionGroupId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_PermissionGroupUser_PermissionGroup_PermissionGroupsPermissionGroupId",
                        column: x => x.PermissionGroupsPermissionGroupId,
                        principalTable: "PermissionGroup",
                        principalColumn: "PermissionGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGroupUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUser",
                columns: table => new
                {
                    ProjectsProjectId = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser", x => new { x.ProjectsProjectId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ProjectUser_Project_ProjectsProjectId",
                        column: x => x.ProjectsProjectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeetingUser",
                columns: table => new
                {
                    MeetingsMeetingId = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingUser", x => new { x.MeetingsMeetingId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_MeetingUser_Meeting_MeetingsMeetingId",
                        column: x => x.MeetingsMeetingId,
                        principalTable: "Meeting",
                        principalColumn: "MeetingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetingUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Form",
                columns: table => new
                {
                    FormId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    ProjectId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    DepartmentId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Content = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: false),
                    ExpectedFinishedDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HandleDepartmentId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Tel = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: false),
                    ProcessNodeId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    FormIsActive = table.Column<bool>(type: "bit", nullable: false),
                    man_day = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Form", x => x.FormId);
                    table.ForeignKey(
                        name: "FK_Form_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_Form_ProcessNode_ProcessNodeId",
                        column: x => x.ProcessNodeId,
                        principalTable: "ProcessNode",
                        principalColumn: "ProcessNodeId");
                    table.ForeignKey(
                        name: "FK_Form_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId");
                });

            migrationBuilder.CreateTable(
                name: "FormRecord",
                columns: table => new
                {
                    ProcessingRecordId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FormId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    DepartmentId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    ResultId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    UserActivityId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    GradeId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormRecord", x => x.ProcessingRecordId);
                    table.ForeignKey(
                        name: "FK_FormRecord_Form_FormId",
                        column: x => x.FormId,
                        principalTable: "Form",
                        principalColumn: "FormId");
                    table.ForeignKey(
                        name: "FK_FormRecord_Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grade",
                        principalColumn: "GradeId");
                    table.ForeignKey(
                        name: "FK_FormRecord_Result_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Result",
                        principalColumn: "ResultId");
                    table.ForeignKey(
                        name: "FK_FormRecord_UserActivity_UserActivityId",
                        column: x => x.UserActivityId,
                        principalTable: "UserActivity",
                        principalColumn: "UserActivityId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_CategoryId",
                table: "Form",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_ProcessNodeId",
                table: "Form",
                column: "ProcessNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_ProjectId",
                table: "Form",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FormRecord_FormId",
                table: "FormRecord",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormRecord_GradeId",
                table: "FormRecord",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormRecord_ResultId",
                table: "FormRecord",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_FormRecord_UserActivityId",
                table: "FormRecord",
                column: "UserActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Meeting_MeetingRoomId",
                table: "Meeting",
                column: "MeetingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingUser_UsersId",
                table: "MeetingUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupUser_UsersId",
                table: "PermissionGroupUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupUserActivity_UserActivitiesUserActivityId",
                table: "PermissionGroupUserActivity",
                column: "UserActivitiesUserActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessNode_DepartmentId",
                table: "ProcessNode",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessNode_UserActivityId",
                table: "ProcessNode",
                column: "UserActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplate_CategoryId",
                table: "ProcessTemplate",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplate_UserActivityId",
                table: "ProcessTemplate",
                column: "UserActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_UsersId",
                table: "ProjectUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GradeId",
                table: "Users",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FormRecord");

            migrationBuilder.DropTable(
                name: "MeetingUser");

            migrationBuilder.DropTable(
                name: "PermissionGroupUser");

            migrationBuilder.DropTable(
                name: "PermissionGroupUserActivity");

            migrationBuilder.DropTable(
                name: "ProcessTemplate");

            migrationBuilder.DropTable(
                name: "ProjectUser");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Form");

            migrationBuilder.DropTable(
                name: "Result");

            migrationBuilder.DropTable(
                name: "Meeting");

            migrationBuilder.DropTable(
                name: "PermissionGroup");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "ProcessNode");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "MeetingRooms");

            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "UserActivity");
        }
    }
}
