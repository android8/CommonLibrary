using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FileUpload.Migrations
{
    public partial class rename_tables2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileOnDatabase",
                schema: "app");

            migrationBuilder.DropTable(
                name: "FileOnFileSystem",
                schema: "app");

            migrationBuilder.CreateTable(
                name: "FilesOnDatabase",
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
                    table.PrimaryKey("PK_FilesOnDatabase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilesOnFileSystem",
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
                    table.PrimaryKey("PK_FilesOnFileSystem", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Extension",
                schema: "app",
                table: "FilesOnDatabase",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FileNameExtension",
                schema: "app",
                table: "FilesOnDatabase",
                columns: new[] { "Name", "Extension" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [Extension] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FileNameFileType",
                schema: "app",
                table: "FilesOnDatabase",
                columns: new[] { "Name", "FileType" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [FileType] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Extension",
                schema: "app",
                table: "FilesOnFileSystem",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FileNameExtension",
                schema: "app",
                table: "FilesOnFileSystem",
                columns: new[] { "Name", "Extension" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [Extension] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FileNameFileType",
                schema: "app",
                table: "FilesOnFileSystem",
                columns: new[] { "Name", "FileType" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [FileType] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilesOnDatabase",
                schema: "app");

            migrationBuilder.DropTable(
                name: "FilesOnFileSystem",
                schema: "app");

            migrationBuilder.CreateTable(
                name: "FileOnDatabase",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileOnDatabase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileOnFileSystem",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileOnFileSystem", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "app",
                table: "FileOnDatabase",
                columns: new[] { "Id", "CreatedOn", "Data", "Description", "Extension", "FileType", "Name", "UploadedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 2, 2, 0, 0, 0, 0, DateTimeKind.Local), null, "Test file1 on database", "txt", "text", "test1", null },
                    { 2, new DateTime(2021, 2, 2, 0, 0, 0, 0, DateTimeKind.Local), null, "Test file2 on database", "txt", "text", "test2", null }
                });

            migrationBuilder.InsertData(
                schema: "app",
                table: "FileOnFileSystem",
                columns: new[] { "Id", "CreatedOn", "Description", "Extension", "FilePath", "FileType", "Name", "UploadedBy" },
                values: new object[,]
                {
                    { 1, null, null, "txt", "c:\\users\\vhaporsunc\\download", "text", "test1", null },
                    { 2, null, null, "txt", "c:\\users\\vhaporsunc\\download", "text", "test2", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Extension",
                schema: "app",
                table: "FileOnDatabase",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FileNameExtension",
                schema: "app",
                table: "FileOnDatabase",
                columns: new[] { "Name", "Extension" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [Extension] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FileNameFileType",
                schema: "app",
                table: "FileOnDatabase",
                columns: new[] { "Name", "FileType" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [FileType] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Extension",
                schema: "app",
                table: "FileOnFileSystem",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FileNameExtension",
                schema: "app",
                table: "FileOnFileSystem",
                columns: new[] { "Name", "Extension" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [Extension] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FileNameFileType",
                schema: "app",
                table: "FileOnFileSystem",
                columns: new[] { "Name", "FileType" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [FileType] IS NOT NULL");
        }
    }
}
