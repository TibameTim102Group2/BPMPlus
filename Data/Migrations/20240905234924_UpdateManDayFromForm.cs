using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateManDayFromForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "man_day",
                table: "Form");

            migrationBuilder.AddColumn<int>(
                name: "ManDay",
                table: "Form",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManDay",
                table: "Form");

            migrationBuilder.AddColumn<int>(
                name: "man_day",
                table: "Form",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
