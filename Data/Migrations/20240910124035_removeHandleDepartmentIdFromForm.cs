using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeHandleDepartmentIdFromForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HandleDepartmentId",
                table: "Form");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HandleDepartmentId",
                table: "Form",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
