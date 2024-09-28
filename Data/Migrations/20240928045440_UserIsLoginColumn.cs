using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserIsLoginColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLogin",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLogin",
                table: "Users");
        }
    }
}
