using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNet_Practice.Migrations
{
    /// <inheritdoc />
    public partial class Update_Student_Table_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Department_DepartmentId1",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_DepartmentId1",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "DepartmentId1",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "DepartmentId2",
                table: "Student");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId1",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId2",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Student_DepartmentId1",
                table: "Student",
                column: "DepartmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Department_DepartmentId1",
                table: "Student",
                column: "DepartmentId1",
                principalTable: "Department",
                principalColumn: "DepartmentId");
        }
    }
}
