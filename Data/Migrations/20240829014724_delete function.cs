using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class deletefunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormRecord_Function_UserActivityId",
                table: "FormRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionGroupUserActivity_Function_UserActivitiesUserActivityId",
                table: "PermissionGroupUserActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessNode_Function_UserActivityId",
                table: "ProcessNode");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplate_Function_UserActivityId",
                table: "ProcessTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Function",
                table: "Function");

            migrationBuilder.RenameTable(
                name: "Function",
                newName: "UserActivity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserActivity",
                table: "UserActivity",
                column: "UserActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormRecord_UserActivity_UserActivityId",
                table: "FormRecord",
                column: "UserActivityId",
                principalTable: "UserActivity",
                principalColumn: "UserActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionGroupUserActivity_UserActivity_UserActivitiesUserActivityId",
                table: "PermissionGroupUserActivity",
                column: "UserActivitiesUserActivityId",
                principalTable: "UserActivity",
                principalColumn: "UserActivityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessNode_UserActivity_UserActivityId",
                table: "ProcessNode",
                column: "UserActivityId",
                principalTable: "UserActivity",
                principalColumn: "UserActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplate_UserActivity_UserActivityId",
                table: "ProcessTemplate",
                column: "UserActivityId",
                principalTable: "UserActivity",
                principalColumn: "UserActivityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormRecord_UserActivity_UserActivityId",
                table: "FormRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionGroupUserActivity_UserActivity_UserActivitiesUserActivityId",
                table: "PermissionGroupUserActivity");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessNode_UserActivity_UserActivityId",
                table: "ProcessNode");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplate_UserActivity_UserActivityId",
                table: "ProcessTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserActivity",
                table: "UserActivity");

            migrationBuilder.RenameTable(
                name: "UserActivity",
                newName: "Function");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Function",
                table: "Function",
                column: "UserActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormRecord_Function_UserActivityId",
                table: "FormRecord",
                column: "UserActivityId",
                principalTable: "Function",
                principalColumn: "UserActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionGroupUserActivity_Function_UserActivitiesUserActivityId",
                table: "PermissionGroupUserActivity",
                column: "UserActivitiesUserActivityId",
                principalTable: "Function",
                principalColumn: "UserActivityId",
                onDelete: ReferentialAction.Cascade);

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
    }
}
