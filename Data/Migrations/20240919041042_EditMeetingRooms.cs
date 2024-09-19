using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditMeetingRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Accomodation",
                table: "MeetingRooms",
                newName: "Accommodation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Accommodation",
                table: "MeetingRooms",
                newName: "Accomodation");
        }
    }
}
