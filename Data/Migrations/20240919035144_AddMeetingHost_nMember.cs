using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMeetingHost_nMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingUser_Meeting_MeetingsMeetingId",
                table: "MeetingUser");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingUser_Users_UsersUserId",
                table: "MeetingUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingUser",
                table: "MeetingUser");

            migrationBuilder.RenameTable(
                name: "MeetingUser",
                newName: "MeetingMember");

            migrationBuilder.RenameIndex(
                name: "IX_MeetingUser_UsersUserId",
                table: "MeetingMember",
                newName: "IX_MeetingMember_UsersUserId");

            migrationBuilder.AddColumn<string>(
                name: "MeetingHost",
                table: "Meeting",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingMember",
                table: "MeetingMember",
                columns: new[] { "MeetingsMeetingId", "UsersUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingMember_Meeting_MeetingsMeetingId",
                table: "MeetingMember",
                column: "MeetingsMeetingId",
                principalTable: "Meeting",
                principalColumn: "MeetingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingMember_Users_UsersUserId",
                table: "MeetingMember",
                column: "UsersUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingMember_Meeting_MeetingsMeetingId",
                table: "MeetingMember");

            migrationBuilder.DropForeignKey(
                name: "FK_MeetingMember_Users_UsersUserId",
                table: "MeetingMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingMember",
                table: "MeetingMember");

            migrationBuilder.DropColumn(
                name: "MeetingHost",
                table: "Meeting");

            migrationBuilder.RenameTable(
                name: "MeetingMember",
                newName: "MeetingUser");

            migrationBuilder.RenameIndex(
                name: "IX_MeetingMember_UsersUserId",
                table: "MeetingUser",
                newName: "IX_MeetingUser_UsersUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingUser",
                table: "MeetingUser",
                columns: new[] { "MeetingsMeetingId", "UsersUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingUser_Meeting_MeetingsMeetingId",
                table: "MeetingUser",
                column: "MeetingsMeetingId",
                principalTable: "Meeting",
                principalColumn: "MeetingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingUser_Users_UsersUserId",
                table: "MeetingUser",
                column: "UsersUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
