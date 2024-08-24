using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixtypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meeting_MeetingRooms_MeetingRoom",
                table: "Meeting");

            migrationBuilder.RenameColumn(
                name: "Accomadation",
                table: "MeetingRooms",
                newName: "Accomodation");

            migrationBuilder.RenameColumn(
                name: "MeetingRoom",
                table: "MeetingRooms",
                newName: "MeetingRoomId");

            migrationBuilder.RenameColumn(
                name: "MeetingRoom",
                table: "Meeting",
                newName: "MeetingRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Meeting_MeetingRoom",
                table: "Meeting",
                newName: "IX_Meeting_MeetingRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meeting_MeetingRooms_MeetingRoomId",
                table: "Meeting",
                column: "MeetingRoomId",
                principalTable: "MeetingRooms",
                principalColumn: "MeetingRoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meeting_MeetingRooms_MeetingRoomId",
                table: "Meeting");

            migrationBuilder.RenameColumn(
                name: "Accomodation",
                table: "MeetingRooms",
                newName: "Accomadation");

            migrationBuilder.RenameColumn(
                name: "MeetingRoomId",
                table: "MeetingRooms",
                newName: "MeetingRoom");

            migrationBuilder.RenameColumn(
                name: "MeetingRoomId",
                table: "Meeting",
                newName: "MeetingRoom");

            migrationBuilder.RenameIndex(
                name: "IX_Meeting_MeetingRoomId",
                table: "Meeting",
                newName: "IX_Meeting_MeetingRoom");

            migrationBuilder.AddForeignKey(
                name: "FK_Meeting_MeetingRooms_MeetingRoom",
                table: "Meeting",
                column: "MeetingRoom",
                principalTable: "MeetingRooms",
                principalColumn: "MeetingRoom");
        }
    }
}
