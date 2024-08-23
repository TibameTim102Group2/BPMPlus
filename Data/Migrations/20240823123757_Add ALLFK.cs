using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddALLFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GradeId",
                table: "Users",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "Users",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "MeetingRoom",
                table: "Meeting",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    CategoryDescription = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Function",
                columns: table => new
                {
                    FunctionId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    FunctionDescription = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Function", x => x.FunctionId);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    GradeId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    GradeName = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.GradeId);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    GroupId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    GroupName = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "MeetingRooms",
                columns: table => new
                {
                    MeetingRoom = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Accomadation = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingRooms", x => x.MeetingRoom);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ProjectId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    ProjectName = table.Column<string>(type: "NVARCHAR(64)", maxLength: 64, nullable: false),
                    Summary = table.Column<string>(type: "NCHAR(500)", maxLength: 500, nullable: false),
                    DeadLine = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Result", x => x.ResultId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessTemplate",
                columns: table => new
                {
                    ProcessNodeId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    FunctionId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    CategoryId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false)
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
                        name: "FK_ProcessTemplate_Function_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Function",
                        principalColumn: "FunctionId");
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
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    FunctionId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    GradeId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: false)
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
                        name: "FK_FormRecord_Function_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Function",
                        principalColumn: "FunctionId");
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
                });

            migrationBuilder.CreateTable(
                name: "ProcessNode",
                columns: table => new
                {
                    ProcessNodeId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    FunctionId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    UserId = table.Column<string>(type: "NVARCHAR(450)", maxLength: 450, nullable: false),
                    DepartmentId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    FormId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        name: "FK_ProcessNode_Form_FormId",
                        column: x => x.FormId,
                        principalTable: "Form",
                        principalColumn: "FormId");
                    table.ForeignKey(
                        name: "FK_ProcessNode_Function_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Function",
                        principalColumn: "FunctionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GradeId",
                table: "Users",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Meeting_MeetingRoom",
                table: "Meeting",
                column: "MeetingRoom");

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
                name: "IX_FormRecord_FunctionId",
                table: "FormRecord",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_FormRecord_GradeId",
                table: "FormRecord",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormRecord_ResultId",
                table: "FormRecord",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessNode_DepartmentId",
                table: "ProcessNode",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessNode_FormId",
                table: "ProcessNode",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessNode_FunctionId",
                table: "ProcessNode",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplate_CategoryId",
                table: "ProcessTemplate",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplate_FunctionId",
                table: "ProcessTemplate",
                column: "FunctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meeting_MeetingRooms_MeetingRoom",
                table: "Meeting",
                column: "MeetingRoom",
                principalTable: "MeetingRooms",
                principalColumn: "MeetingRoom");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Department_DepartmentId",
                table: "Users",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Grade_GradeId",
                table: "Users",
                column: "GradeId",
                principalTable: "Grade",
                principalColumn: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Form_ProcessNode_ProcessNodeId",
                table: "Form",
                column: "ProcessNodeId",
                principalTable: "ProcessNode",
                principalColumn: "ProcessNodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meeting_MeetingRooms_MeetingRoom",
                table: "Meeting");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Department_DepartmentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Grade_GradeId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Form_Category_CategoryId",
                table: "Form");

            migrationBuilder.DropForeignKey(
                name: "FK_Form_ProcessNode_ProcessNodeId",
                table: "Form");

            migrationBuilder.DropTable(
                name: "FormRecord");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "MeetingRooms");

            migrationBuilder.DropTable(
                name: "ProcessTemplate");

            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropTable(
                name: "Result");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "ProcessNode");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Form");

            migrationBuilder.DropTable(
                name: "Function");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Users_DepartmentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_GradeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Meeting_MeetingRoom",
                table: "Meeting");

            migrationBuilder.AlterColumn<string>(
                name: "GradeId",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "Users",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MeetingRoom",
                table: "Meeting",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
