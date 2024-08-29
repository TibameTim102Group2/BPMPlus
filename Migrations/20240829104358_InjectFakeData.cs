using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Migrations
{
    /// <inheritdoc />
    public partial class InjectFakeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "Category", // 確保這裡的表名與你的模型相匹配
            columns: new[] { "CategoryId", "CategoryDescription", "CreatedTime", "UpdatedTime" },
            values: new object[,]
            {
                { "C1", "資訊需求", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "C2", "權限修改", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
                { "C3", "項目採購", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                { "C4", "名片申請", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                { "C5", "辦公用品申請", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                { "C6", "差旅申請", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                { "C7", "費用申請", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
            });
            migrationBuilder.InsertData(
            table: "Grade", // 確保這裡的表名與你的模型相匹配
            columns: new[] { "GradeId", "GradeName", "CreatedTime", "UpdatedTime" },
            values: new object[,]
            {
                { "01", "辦事員", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "02", "專員", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "03", "襄理", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "04", "副理", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "05", "經理", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "06", "協理", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "07", "副總", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "08", "總經理", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "09", "副董事長", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "10", "董事長", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
            });
            migrationBuilder.InsertData(
            table: "Department", // 確保這裡的表名與你的模型相匹配
            columns: new[] { "DepartmentId", "DepartmentName", "CreatedTime", "UpdateTime" },
            values: new object[,]
            {
                 { "D001", "資訊部",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                 { "D002", "人資部",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                 { "D003", "行銷部",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                 { "D004", "財會部",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                 { "D005", "採購部",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                 { "D006", "商品部",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                 { "D007", "總務部",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                 { "D008", "業務部",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) }

            });
            migrationBuilder.Sql($@"
                INSERT INTO Result (ResultId, ResultDescription, CreatedTime, UpdateTime) VALUES 
                ('RS1', N'退回', '2024-01-01 12:15:04', '2024-02-01 12:15:04'),
                ('RS2', N'核准', '2024-01-01 12:15:04', '2024-02-01 12:15:04'),
                ('RS3', N'結案', '2024-01-01 12:15:04', '2024-02-01 12:15:04'),
                ('RS4', N'審核中', '2024-01-01 12:15:04', '2024-02-01 12:15:04');
            ");
            migrationBuilder.InsertData(
            table: "UserActivity", // 確保這裡的表名與你的模型相匹配
            columns: new[] { "UserActivityId", "UserActivityIdDescription", "CreatedTime", "UpdatedTime" },
            values: new object[,]
            {
                { "01", "需求方申請人送出",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "02", "需求方一級主管審核",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "03", "需求方二級主管審核",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "04", "需求方三級主管審核",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "05", "接收方三級主管指派(審核)",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "06", "接收方二級主管指派(審核)",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "07", "接收方一級主管指派(審核)",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "08", "接收方人員接單處理",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "09", "需求方驗收",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "10", "專案管理",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "11", "瀏覽專案細節",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
            });
            migrationBuilder.InsertData(
            table: "PermissionGroup", // 確保這裡的表名與你的模型相匹配
            columns: new[] { "PermissionGroupId", "PermissionGroupName", "CreatedTime", "UpdatedTime" },
            values: new object[,]
            {
                { "G0001", "系統管理員",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "G0002", "接收方一級主管",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "G0003", "接收方二級主管",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "G0004", "接收方三級主管",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "G0005", "申請方一級主管",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "G0006", "申請方二級主管",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "G0007", "申請方三級主管",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "G0008", "接收方基層人員",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                { "G0009", "申請方基層人員",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
            });
            migrationBuilder.InsertData(
                table: "ProcessTemplate", // 確保這裡的表名與你的模型相匹配
                columns: new[] { "ProcessNodeId", "UserActivityId", "DepartmentId", "CategoryId", "CreatedTime", "UpdateTime" },
                values: new object[,]
                {
                    {"PT01", "01", "C001", "C1", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                    {"PT02", "02", "C001", "C1", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
                    {"PT03", "03", "C001", "C1", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT04", "06", "D001", "C1", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                    {"PT05", "07", "D001", "C1", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
                    {"PT06", "08", "D001", "C1", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT07", "09", "C001", "C1", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT08", "01", "C001", "C2", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                    {"PT09", "02", "C001", "C2", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
                    {"PT10", "03", "C001", "C2", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT11", "06", "D001", "C2", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                    {"PT12", "07", "D001", "C2", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
                    {"PT13", "08", "D001", "C2", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT14", "09", "C001", "C2", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT15", "01", "C001", "C3", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                    {"PT16", "02", "C001", "C3", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
                    {"PT17", "03", "C001", "C3", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT18", "06", "D005", "C3", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                    {"PT19", "07", "D005", "C3", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
                    {"PT20", "08", "D005", "C3", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT21", "09", "C001", "C3", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT22", "01", "C001", "C4", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT23", "02", "C001", "C4", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                    {"PT24", "07", "D007", "C4", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
                    {"PT25", "08", "D007", "C4", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT26", "09", "C001", "C4", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT27", "01", "C001", "C5", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT28", "02", "C001", "C5", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
                    {"PT29", "07", "D007", "C5", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
                    {"PT30", "08", "D007", "C5", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
                    {"PT31", "09", "C001", "C5", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
