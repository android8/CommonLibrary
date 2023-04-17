using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FileUpload.Migrations
{
    public partial class add_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "app",
                table: "FilesOnDatabase",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "app",
                table: "FilesOnDatabase",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "app",
                table: "FilesOnFileSystem",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "app",
                table: "FilesOnFileSystem",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "FY",
                schema: "app",
                table: "FilesOnFileSystem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FacilityID",
                schema: "app",
                table: "FilesOnFileSystem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FY",
                schema: "app",
                table: "FilesOnDatabase",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FacilityID",
                schema: "app",
                table: "FilesOnDatabase",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FY",
                schema: "app",
                table: "FilesOnFileSystem");

            migrationBuilder.DropColumn(
                name: "FacilityID",
                schema: "app",
                table: "FilesOnFileSystem");

            migrationBuilder.DropColumn(
                name: "FY",
                schema: "app",
                table: "FilesOnDatabase");

            migrationBuilder.DropColumn(
                name: "FacilityID",
                schema: "app",
                table: "FilesOnDatabase");

            migrationBuilder.InsertData(
                schema: "app",
                table: "FilesOnDatabase",
                columns: new[] { "Id", "CreatedOn", "Data", "Description", "Extension", "FileType", "Name", "UploadedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 2, 2, 0, 0, 0, 0, DateTimeKind.Local), null, "Test file1 on database", "txt", "text", "test1", null },
                    { 2, new DateTime(2021, 2, 2, 0, 0, 0, 0, DateTimeKind.Local), null, "Test file2 on database", "txt", "text", "test2", null }
                });

            migrationBuilder.InsertData(
                schema: "app",
                table: "FilesOnFileSystem",
                columns: new[] { "Id", "CreatedOn", "Description", "Extension", "FilePath", "FileType", "Name", "UploadedBy" },
                values: new object[,]
                {
                    { 1, null, null, "txt", "c:\\users\\vhaporsunc\\download", "text", "test1", null },
                    { 2, null, null, "txt", "c:\\users\\vhaporsunc\\download", "text", "test2", null }
                });
        }
    }
}
