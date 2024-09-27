using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
    /// <inheritdoc />
    public partial class InjectDb : Migration
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
    { "C8", "教育訓練", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) }
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
            columns: new[] { "DepartmentId", "DepartmentName", "CreatedTime", "UpdatedTime" },
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
    INSERT INTO Result (ResultId, ResultDescription, CreatedTime, UpdatedTime) VALUES 
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
    { "05", "處理方三級主管指派(審核)",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "06", "處理方二級主管指派(審核)",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "07", "處理方一級主管指派(審核)",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "08", "處理方人員接單處理",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "09", "需求方驗收",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "10", "驗收完成",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "11", "專案管理",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "12", "瀏覽專案細節",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
            });
            migrationBuilder.InsertData(
            table: "PermissionGroup", // 確保這裡的表名與你的模型相匹配
            columns: new[] { "PermissionGroupId", "PermissionGroupName", "CreatedTime", "UpdatedTime" },
            values: new object[,]
            {
    { "G0001", "系統管理員",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "G0002", "處理方一級主管",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "G0003", "處理方二級主管",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "G0004", "處理方三級主管",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "G0005", "申請方一級主管",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "G0006", "申請方二級主管",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "G0007", "申請方三級主管",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "G0008", "處理方基層人員",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
    { "G0009", "申請方基層人員",  new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
            });
            migrationBuilder.InsertData(
                table: "ProcessTemplate", // 確保這裡的表名與你的模型相匹配
                columns: new[] { "ProcessNodeId", "UserActivityId", "DepartmentId", "CategoryId", "CreatedTime", "UpdatedTime" },
                values: new object[,]
                {
        {"PT01", "01", "Requester", "C1", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
        {"PT02", "02", "Requester", "C1", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
        {"PT03", "03", "Requester", "C1", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT04", "06", "D001", "C1", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
        {"PT05", "07", "D001", "C1", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
        {"PT06", "08", "D001", "C1", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT07", "09", "Requester", "C1", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT08", "10", "Requester", "C1", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT09", "01", "Requester", "C2", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
        {"PT10", "02", "Requester", "C2", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
        {"PT11", "03", "Requester", "C2", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT12", "06", "D001", "C2", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
        {"PT13", "07", "D001", "C2", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
        {"PT14", "08", "D001", "C2", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT15", "09", "Requester", "C2", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT16", "10", "Requester", "C2", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT17", "01", "Requester", "C3", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
        {"PT18", "02", "Requester", "C3", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
        {"PT19", "03", "Requester", "C3", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT20", "06", "D005", "C3", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
        {"PT21", "07", "D005", "C3", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
        {"PT22", "08", "D005", "C3", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT23", "09", "Requester", "C3", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT24", "10", "Requester", "C3", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT25", "01", "Requester", "C4", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT26", "02", "Requester", "C4", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
        {"PT27", "07", "D007", "C4", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
        {"PT28", "08", "D007", "C4", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT29", "09", "Requester", "C4", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT30", "10", "Requester", "C4", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT31", "01", "Requester", "C5", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT32", "02", "Requester", "C5", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
        {"PT33", "07", "D007", "C5", new DateTime(2024, 8, 2), new DateTime(2024, 8, 2) },
        {"PT34", "08", "D007", "C5", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT35", "09", "Requester", "C5", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT36", "10", "Requester", "C5", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT37", "01", "Requester", "C8", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT38", "07", "D002", "C8", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
        {"PT39", "08", "D002", "C8", new DateTime(2024, 8, 1), new DateTime(2024, 8, 1) },
        {"PT40", "09", "Requester", "C8", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) },
        {"PT41", "10", "Requester", "C8", new DateTime(2024, 8, 3), new DateTime(2024, 8, 3) }
                });



        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
