using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserActivityForAddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "UserActivity", // 確保這裡的表名與你的模型相匹配
            columns: new[] { "UserActivityId", "UserActivityIdDescription", "CreatedTime", "UpdatedTime" },
            values: new object[,]
            {
                { "13", "增加需求類別",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
