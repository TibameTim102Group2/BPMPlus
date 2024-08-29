using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixConflictedTableNameAndAddDepartmentIdToTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormRecord_Function_FunctionId",
                table: "FormRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessNode_Function_FunctionId",
                table: "ProcessNode");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplate_Function_FunctionId",
                table: "ProcessTemplate");

            migrationBuilder.DropTable(
                name: "FunctionGroup");

            migrationBuilder.DropTable(
                name: "GroupUser");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "Function");

            migrationBuilder.RenameColumn(
                name: "FunctionId",
                table: "ProcessTemplate",
                newName: "UserActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessTemplate_FunctionId",
                table: "ProcessTemplate",
                newName: "IX_ProcessTemplate_UserActivityId");

            migrationBuilder.RenameColumn(
                name: "FunctionId",
                table: "ProcessNode",
                newName: "UserActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessNode_FunctionId",
                table: "ProcessNode",
                newName: "IX_ProcessNode_UserActivityId");

            migrationBuilder.RenameColumn(
                name: "FunctionDescription",
                table: "Function",
                newName: "UserActivityIdDescription");

            migrationBuilder.RenameColumn(
                name: "FunctionId",
                table: "Function",
                newName: "UserActivityId");

            migrationBuilder.RenameColumn(
                name: "FunctionId",
                table: "FormRecord",
                newName: "UserActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_FormRecord_FunctionId",
                table: "FormRecord",
                newName: "IX_FormRecord_UserActivityId");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentId",
                table: "ProcessTemplate",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PermissionGroup",
                columns: table => new
                {
                    PermissionGroupId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    PermissionGroupName = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroup", x => x.PermissionGroupId);
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
                        name: "FK_PermissionGroupUserActivity_Function_UserActivitiesUserActivityId",
                        column: x => x.UserActivitiesUserActivityId,
                        principalTable: "Function",
                        principalColumn: "UserActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGroupUserActivity_PermissionGroup_PermissionGroupsPermissionGroupId",
                        column: x => x.PermissionGroupsPermissionGroupId,
                        principalTable: "PermissionGroup",
                        principalColumn: "PermissionGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupUser_UsersId",
                table: "PermissionGroupUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupUserActivity_UserActivitiesUserActivityId",
                table: "PermissionGroupUserActivity",
                column: "UserActivitiesUserActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormRecord_Function_UserActivityId",
                table: "FormRecord",
                column: "UserActivityId",
                principalTable: "Function",
                principalColumn: "UserActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessNode_Function_UserActivityId",
                table: "ProcessNode",
                column: "UserActivityId",
                principalTable: "Function",
                principalColumn: "UserActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplate_Function_UserActivityId",
                table: "ProcessTemplate",
                column: "UserActivityId",
                principalTable: "Function",
                principalColumn: "UserActivityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormRecord_Function_UserActivityId",
                table: "FormRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessNode_Function_UserActivityId",
                table: "ProcessNode");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplate_Function_UserActivityId",
                table: "ProcessTemplate");

            migrationBuilder.DropTable(
                name: "PermissionGroupUser");

            migrationBuilder.DropTable(
                name: "PermissionGroupUserActivity");

            migrationBuilder.DropTable(
                name: "PermissionGroup");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "ProcessTemplate");

            migrationBuilder.RenameColumn(
                name: "UserActivityId",
                table: "ProcessTemplate",
                newName: "FunctionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessTemplate_UserActivityId",
                table: "ProcessTemplate",
                newName: "IX_ProcessTemplate_FunctionId");

            migrationBuilder.RenameColumn(
                name: "UserActivityId",
                table: "ProcessNode",
                newName: "FunctionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessNode_UserActivityId",
                table: "ProcessNode",
                newName: "IX_ProcessNode_FunctionId");

            migrationBuilder.RenameColumn(
                name: "UserActivityIdDescription",
                table: "Function",
                newName: "FunctionDescription");

            migrationBuilder.RenameColumn(
                name: "UserActivityId",
                table: "Function",
                newName: "FunctionId");

            migrationBuilder.RenameColumn(
                name: "UserActivityId",
                table: "FormRecord",
                newName: "FunctionId");

            migrationBuilder.RenameIndex(
                name: "IX_FormRecord_UserActivityId",
                table: "FormRecord",
                newName: "IX_FormRecord_FunctionId");

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    GroupId = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GroupName = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "FunctionGroup",
                columns: table => new
                {
                    FunctionsFunctionId = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    GroupsGroupId = table.Column<string>(type: "VARCHAR(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionGroup", x => new { x.FunctionsFunctionId, x.GroupsGroupId });
                    table.ForeignKey(
                        name: "FK_FunctionGroup_Function_FunctionsFunctionId",
                        column: x => x.FunctionsFunctionId,
                        principalTable: "Function",
                        principalColumn: "FunctionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FunctionGroup_Group_GroupsGroupId",
                        column: x => x.GroupsGroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupUser",
                columns: table => new
                {
                    GroupsGroupId = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => new { x.GroupsGroupId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GroupUser_Group_GroupsGroupId",
                        column: x => x.GroupsGroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FunctionGroup_GroupsGroupId",
                table: "FunctionGroup",
                column: "GroupsGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUser_UsersId",
                table: "GroupUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormRecord_Function_FunctionId",
                table: "FormRecord",
                column: "FunctionId",
                principalTable: "Function",
                principalColumn: "FunctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessNode_Function_FunctionId",
                table: "ProcessNode",
                column: "FunctionId",
                principalTable: "Function",
                principalColumn: "FunctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplate_Function_FunctionId",
                table: "ProcessTemplate",
                column: "FunctionId",
                principalTable: "Function",
                principalColumn: "FunctionId");
        }
    }
}
