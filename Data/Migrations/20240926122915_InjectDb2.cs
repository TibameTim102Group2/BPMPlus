using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPMPlus.Data.Migrations
{
	/// <inheritdoc />
	public partial class InjectDb2 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql($@"
                INSERT INTO PermissionGroup (PermissionGroupId,PermissionGroupName,CreatedTime,UpdatedTime) VALUES
                ('G0010',N'專案管理員','2024-09-16','2024-09-16');
                GO
                INSERT INTO Users (DepartmentId, UserId, GradeId, UserIsActive, CreatedTime, UpdatedTime, TEL, UserName, Email, Password) VALUES
                ('D002', 'A001', '05', 1, '2024-08-19', '2024-08-19', '0912345001', N'風間', 'dawen5@domain.com', '$2b$10$5Gix0lXUXZ.NoDqXx6VtOuBWJQBb/dwXjNHwc8CNlxQ.VlEBXmIji'),
                ('D002', 'A002', '01', 1, '2024-08-19', '2024-08-19', '0912345002', N'阿呆', 'dawen5@domain.com', '$2b$10$TerkGwFIc4vMrzWEie5gzOgNjWtr4MTFD2Qvgu3PUHbSBakgu3AjG'),
                ('D001', 'A003', '05', 1, '2024-08-19', '2024-08-19', '0912345003', N'妮妮', 'dawen5@domain.com', '$2b$10$1BTZnYMyEyUx2YDaS0i.HOGNBh6cJxkVU2515tqZjv9b6bFVw3KKa');
                GO
                INSERT INTO PermissionGroupUserActivity(PermissionGroupsPermissionGroupId, UserActivitiesUserActivityId) VALUES 
                ('G0009','09'),('G0008','08'),('G0006','03'),('G0005','02'),('G0003','06'),('G0002','07'),('G0006', '08'),
                ('G0007','01'),('G0006','01'),('G0005','01'),('G0004','01'),('G0009','01'),('G0003','01'),('G0002','01'),
                ('G0001','01'),('G0001','02'),('G0001','03'),('G0001','04'),('G0001','05'),('G0001','06'),('G0001','07'),('G0001','08'),('G0001','09'),
                ('G0001', '13'),('G0010','11');
                GO
                INSERT INTO PermissionGroupUser 
                VALUES
                ('G0001', 'A001'),
                ('G0001', 'A002'),
                ('G0001', 'A003'),
                ('G0009', 'A001'),
                ('G0009', 'A003');
                GO
                insert into MeetingRooms values
                ('R001','5','2024-09-19','2024-09-19'),
                ('R002','8','2024-09-19','2024-09-19'),
                ('R003','10','2024-09-19','2024-09-19'),
                ('R004','15','2024-09-19','2024-09-19');
                insert into Meeting values
                ('M001','R001','2024-09-28 02:00','2024-09-28 03:00',N'定期會議','2024-09-20','2024-09-20','A001'),
                ('M002','R002','2024-09-28 04:00','2024-09-28 05:00',N'定期會議','2024-09-20','2024-09-20','A002'),
                ('M003','R003','2024-09-28 03:00','2024-09-28 06:00',N'定期會議','2024-09-20','2024-09-20','A003');
            ");

		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
