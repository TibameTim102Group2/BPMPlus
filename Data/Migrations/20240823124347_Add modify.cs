using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addmodify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessNode_Form_FormId",
                table: "ProcessNode");

            migrationBuilder.DropIndex(
                name: "IX_ProcessNode_FormId",
                table: "ProcessNode");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "ProcessNode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormId",
                table: "ProcessNode",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessNode_FormId",
                table: "ProcessNode",
                column: "FormId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessNode_Form_FormId",
                table: "ProcessNode",
                column: "FormId",
                principalTable: "Form",
                principalColumn: "FormId");
        }
    }
}
