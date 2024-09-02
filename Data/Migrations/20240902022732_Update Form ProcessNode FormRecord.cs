using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFormProcessNodeFormRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Form_ProcessNode_ProcessNodeId",
                table: "Form");

            migrationBuilder.DropIndex(
                name: "IX_Form_ProcessNodeId",
                table: "Form");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProcessNode",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(450)",
                oldMaxLength: 450);

            migrationBuilder.AddColumn<string>(
                name: "FormId",
                table: "ProcessNode",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FormRecord",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Form",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "ProcessNodeId",
                table: "Form",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessNode_FormId",
                table: "ProcessNode",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormRecord_UserId",
                table: "FormRecord",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_DepartmentId",
                table: "Form",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_UserId",
                table: "Form",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Form_Department_DepartmentId",
                table: "Form",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Form_Users_UserId",
                table: "Form",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormRecord_Users_UserId",
                table: "FormRecord",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessNode_Form_FormId",
                table: "ProcessNode",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "FormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Form_Department_DepartmentId",
                table: "Form");

            migrationBuilder.DropForeignKey(
                name: "FK_Form_Users_UserId",
                table: "Form");

            migrationBuilder.DropForeignKey(
                name: "FK_FormRecord_Users_UserId",
                table: "FormRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessNode_Form_FormId",
                table: "ProcessNode");

            migrationBuilder.DropIndex(
                name: "IX_ProcessNode_FormId",
                table: "ProcessNode");

            migrationBuilder.DropIndex(
                name: "IX_FormRecord_UserId",
                table: "FormRecord");

            migrationBuilder.DropIndex(
                name: "IX_Form_DepartmentId",
                table: "Form");

            migrationBuilder.DropIndex(
                name: "IX_Form_UserId",
                table: "Form");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "ProcessNode");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProcessNode",
                type: "NVARCHAR(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FormRecord",
                type: "NVARCHAR(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Form",
                type: "NVARCHAR(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "ProcessNodeId",
                table: "Form",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "IX_Form_ProcessNodeId",
                table: "Form",
                column: "ProcessNodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Form_ProcessNode_ProcessNodeId",
                table: "Form",
                column: "ProcessNodeId",
                principalTable: "ProcessNode",
                principalColumn: "ProcessNodeId");
        }
    }
}
