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

                INSERT INTO Project (ProjectId,ProjectManagerId, ProjectName, Summary, DeadLine, CreatedTime, UpdatedTime) VALUES
                    ('P001', 'A009',N'春日部音樂會', N'音樂會', '2024-09-30', '2024-09-25', '2024-09-25'),
                    ('P002', 'A009',N'春日部大遊行', N'遊行', '2024-09-30', '2024-09-25', '2024-09-25'),
                    ('P003','A009', N'春日部大遠足', N'遠足', '2024-09-30', '2024-09-25', '2024-09-25'),
                    ('P004','A004',N'光榮燒肉之路', N'燒肉放題', '2024-09-30', '2024-09-25', '2024-09-25'),
                    ('P005', 'A003',N'我們的恐龍日記', N'恐龍', '2024-09-30', '2024-09-25', '2024-09-25'),
                    ('P006','A003', N'春日部野生王國', N'野生王國', '2024-09-30', '2024-09-25', '2024-09-25'),
                    ('P007','A002', N'動感光波發射器', N'動~感~光~波~', '2024-09-30', '2024-09-25', '2024-09-25'),
                    ('P008','A006', N'動感超人VS高衩魔王', N'動感光波 逼逼逼', '2024-09-30', '2024-09-25', '2024-09-25'),
                    ('P009', 'A006',N'超級美味！B級美食大逃亡！！', N'吃美食', '2024-09-30', '2024-09-25', '2024-09-25'),
                    ('P010', 'A005',N'春日部幼稚園改建', N'春日部幼稚園', '2024-09-30', '2024-09-25', '2024-09-25');
                GO
                    INSERT INTO ProjectUser (ProjectsProjectId, UsersUserId) VALUES
                    ('P001', 'A009'),
                    ('P002', 'A009'),
                    ('P003', 'A009'),
                    ('P004', 'A004'),
                    ('P005', 'A003'),
                    ('P006', 'A003'),
                    ('P007', 'A002'),
                    ('P008', 'A006'),
                    ('P009', 'A006'),
                    ('P010', 'A005');
                GO
                INSERT INTO Users (DepartmentId, UserId, GradeId, UserIsActive, CreatedTime, UpdatedTime, TEL, UserName, Email, Password) VALUES
                    ('D002', 'A001', '05', 1, '2024-08-19', '2024-08-19', '0912345001', N'風間', 'dawen5@domain.com', '$2b$10$5Gix0lXUXZ.NoDqXx6VtOuBWJQBb/dwXjNHwc8CNlxQ.VlEBXmIji'),
                    ('D002', 'A002', '01', 1, '2024-08-19', '2024-08-19', '0912345002', N'阿呆', 'dawen5@domain.com', '$2b$10$TerkGwFIc4vMrzWEie5gzOgNjWtr4MTFD2Qvgu3PUHbSBakgu3AjG'),
                    ('D001', 'A003', '05', 1, '2024-08-19', '2024-08-19', '0912345003', N'妮妮', 'dawen5@domain.com', '$2b$10$a0P.SVqYtRkw44VVSQDn2OfjMaxKIdOF9d.WbsEIDFVb1EJRTiE1u'),
                    ('D003', 'A004', '01', 1, '2024-08-19', '2024-08-19', '0912345004', N'小白', 'dawen5@domain.com', '$2b$10$a0P.SVqYtRkw44VVSQDn2OfjMaxKIdOF9d.WbsEIDFVb1EJRTiE1u'),
                    ('D004', 'A005', '02', 1, '2024-08-19', '2024-08-19', '0912345005', N'園長', 'dawen5@domain.com', '$2b$10$/SK4Q607gsBhEpySWpehAOACA76zvl0rGBJsS8YayPryMVHEM.Sfa'),
                    ('D005', 'A006', '03', 1, '2024-08-19', '2024-08-19', '0912345006', N'吉永', 'dawen5@domain.com', '$2b$10$CzvuZ5.Tr9pc1o2kW2m9G.NI/1NuKi6/dGt5UWaPjsgGRmkEfPLfa'),
                    ('D005', 'A007', '04', 1, '2024-08-19', '2024-08-19', '0912345007', N'娜娜子', 'dawen5@domain.com', '$2b$10$iL/pyleJEwCyZCFuPdAW7OrnsUYGqylRyjITvhw3cj0lISB0QS2C2'),
                    ('D006', 'A008', '01', 1, '2024-08-19', '2024-08-19', '0912345008', N'小新', 'bpmplusa024@zohomail.com', '$2b$10$tjstqLk0Z1Ua/TJ/7WRSp.dq1l3ca2yseZv35fUVAGloHuJ0VCkjy'),
                    ('D006', 'A009', '05', 1, '2024-08-19', '2024-08-19', '0912345009', N'正男', 'jangnanforthewin@gmail.com', '$2b$10$SMCWgjVLxgM2HUe0iAhcuOVtlnqYfvtDsQZCA0ZxzeJkOzWFSBAaS'),
                    ('D003', 'A010', '05', 1, '2024-08-19', '2024-08-19', '0912345010', N'小葵', 'bpmplus102@gmail.com', '$2b$10$Dc8ggWvs95ZtilMyBb.q8uK8O1TNxR9gzofXMQ5reU6Eyu18lNlNG'),
                    ('D003', 'A011', '01', 1, '2024-08-19', '2024-08-19', '0912345011', N'小愛', 'bpmplusa027@yahoo.com', '$2b$10$sNX9mLIvwyZ2KOVDJkaFOeXFO0LDxxqRM9Qn4RyzaSz3I95vgUpla');
                GO

                INSERT INTO PermissionGroupUserActivity(PermissionGroupsPermissionGroupId, UserActivitiesUserActivityId) VALUES 
                    ('G0009','09'),('G0008','08'),('G0006','03'),('G0005','02'),('G0003','06'),('G0002','07'),('G0006', '08'),
                    ('G0007','01'),('G0006','01'),('G0005','01'),('G0004','01'),('G0009','01'),('G0003','01'),('G0002','01'),
                    ('G0001','01'),('G0001','02'),('G0001','03'),('G0001','04'),('G0001','05'),('G0001','06'),('G0001','07'),('G0001','08'),('G0001','09'),
                    ('G0001', '13'),('G0010','11');
                GO

                INSERT INTO PermissionGroupUser 
                VALUES
                    ('G0009', 'A001'),
                    ('G0009', 'A002'),
                    ('G0009', 'A003'),
                    ('G0009', 'A004'),
                    ('G0009', 'A005'),
                    ('G0009', 'A006'),
                    ('G0009', 'A007'),
                    ('G0009', 'A008'),
                    ('G0009', 'A009'),
                    ('G0009', 'A010'),
                    ('G0009', 'A011'),
                    ('G0010', 'A001'),
                    ('G0010', 'A002'),
                    ('G0010', 'A003'),
                    ('G0010', 'A004'),
                    ('G0010', 'A005'),
                    ('G0010', 'A006'),
                    ('G0010', 'A007'),
                    ('G0010', 'A008'),
                    ('G0010', 'A009'),
                    ('G0010', 'A010'),
                    ('G0010', 'A011'),
                    ('G0008', 'A001'),
                    ('G0008', 'A002'),
                    ('G0008', 'A003'),
                    ('G0008', 'A004'),
                    ('G0008', 'A005'),
                    ('G0008', 'A006'),
                    ('G0008', 'A007'),
                    ('G0008', 'A008'),
                    ('G0008', 'A009'),
                    ('G0008', 'A010'),
                    ('G0008', 'A011'),
                    ('G0001', 'A001'),
                    ('G0001', 'A003');
                GO

                insert into MeetingRooms values
                    ('R001','5','2024-09-19','2024-09-19'),
                    ('R002','8','2024-09-19','2024-09-19'),
                    ('R003','10','2024-09-19','2024-09-19'),
                    ('R004','15','2024-09-19','2024-09-19');
                go

                insert into Meeting values
                    ('M001','R001','2024-09-28 02:00','2024-09-28 03:00',N'定期會議','2024-09-20','2024-09-20','A001'),
                    ('M002','R002','2024-09-28 04:00','2024-09-28 05:00',N'定期會議','2024-09-20','2024-09-20','A002'),
                    ('M003','R003','2024-09-27 03:00','2024-09-27 06:00',N'定期會議','2024-09-20','2024-09-20','A003'),
                    ('M004','R004','2024-09-29 01:00','2024-09-29 03:00',N'定期會議','2024-09-20','2024-09-20','A001'),
                    ('M005','R002','2024-09-30 04:00','2024-09-30 05:00',N'定期會議','2024-09-20','2024-09-20','A002'),
                    ('M006','R003','2024-10-03 03:00','2024-10-03 05:00',N'定期會議','2024-09-20','2024-09-20','A001'),
                    ('M007','R002','2024-10-02 03:00','2024-10-02 05:00',N'定期會議','2024-09-20','2024-09-20','A006'),
                    ('M008','R003','2024-10-02 07:00','2024-10-02 11:00',N'定期會議','2024-09-20','2024-09-20','A004');
                go

                INSERT INTO Form (FormId, UserId, Date, CategoryId, ProjectId, DepartmentId, Content, ExpectedFinishedDay, Tel, ProcessNodeId, FormIsActive, ManDay, CreatedTime, UpdatedTime) VALUES
                ('F00001', 'A008', '2024-09-25', 'C8', 'P001', 'D006', 'Hello', '2024-09-27', '0987654321', 'PN000002', 1, 5, '2024-09-25','2024-09-25');
                GO

                INSERT INTO ProcessNode(ProcessNodeId, UserActivityId, UserId, DepartmentId, CreatedTime, UpdatedTime, FormId) VALUES
                ('PN000001'        ,'01',        'A008','D006'        ,'2024-08-01', '2024-08-01', 'F00001'),
                ('PN000002'        ,'02',        'A001','D002'        ,'2024-08-02', '2024-08-02', 'F00001'),
                ('PN000003'        ,'03',        'A002','D002'        ,'2024-08-03', '2024-08-03', 'F00001'),
                ('PN000004'        ,'06',        'A008','D006'        ,'2024-08-01', '2024-08-01', 'F00001'),
                ('PN000005'        ,'07',        'A008','D006'        ,'2024-08-02', '2024-08-02', 'F00001');
                GO

                insert into FormRecord(ProcessingRecordId,Remark,FormId,DepartmentId,UserId,ResultId,UserActivityId,Date,GradeId,CreatedTime,UpdatedTime)
                values('PR00000001','null','F00001','D006','A008','RS2','01','2024-08-01','01','2024-08-01','2024-08-01'),
                      ('PR00000002','good','F00001','D001','A001','RS4','02','2024-08-01','05','2024-08-01','2024-08-01');
            ");

		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
