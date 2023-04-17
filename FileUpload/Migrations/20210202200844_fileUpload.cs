using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FileUpload.Migrations
{
    public partial class fileUpload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "FileOnDatabaseModel",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileOnDatabaseModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileOnFileSystemModel",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileOnFileSystemModel", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "app",
                table: "FileOnDatabaseModel",
                columns: new[] { "Id", "CreatedOn", "Data", "Description", "Extension", "FileType", "Name", "UploadedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 2, 2, 0, 0, 0, 0, DateTimeKind.Local), null, "Test file1 on database", "txt", "text", "test1", null },
                    { 2, new DateTime(2021, 2, 2, 0, 0, 0, 0, DateTimeKind.Local), null, "Test file2 on database", "txt", "text", "test2", null }
                });

            migrationBuilder.InsertData(
                schema: "app",
                table: "FileOnFileSystemModel",
                columns: new[] { "Id", "CreatedOn", "Description", "Extension", "FilePath", "FileType", "Name", "UploadedBy" },
                values: new object[,]
                {
                    { 1, null, null, "txt", "c:\\users\\vhaporsunc\\download", "text", "test1", null },
                    { 2, null, null, "txt", "c:\\users\\vhaporsunc\\download", "text", "test2", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Extension",
                schema: "app",
                table: "FileOnDatabaseModel",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FileNameExtension",
                schema: "app",
                table: "FileOnDatabaseModel",
                columns: new[] { "Name", "Extension" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [Extension] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FileNameFileType",
                schema: "app",
                table: "FileOnDatabaseModel",
                columns: new[] { "Name", "FileType" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [FileType] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Extension",
                schema: "app",
                table: "FileOnFileSystemModel",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FileNameExtension",
                schema: "app",
                table: "FileOnFileSystemModel",
                columns: new[] { "Name", "Extension" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [Extension] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FileNameFileType",
                schema: "app",
                table: "FileOnFileSystemModel",
                columns: new[] { "Name", "FileType" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [FileType] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileOnDatabaseModel",
                schema: "app");

            migrationBuilder.DropTable(
                name: "FileOnFileSystemModel",
                schema: "app");
        }
    }
}
