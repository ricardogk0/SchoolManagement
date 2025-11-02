using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BugCreatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "students",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreateBy",
                table: "students",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "registrations",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreateBy",
                table: "registrations",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "classes",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreateBy",
                table: "classes",
                newName: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "students",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "students",
                newName: "CreateBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "registrations",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "registrations",
                newName: "CreateBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "classes",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "classes",
                newName: "CreateBy");
        }
    }
}
