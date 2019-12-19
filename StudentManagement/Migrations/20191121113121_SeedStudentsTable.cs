using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentManagement.Migrations
{
    public partial class SeedStudentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Department", "Email", "FirstName", "LastName", "PhotoPath" },
                values: new object[] { 1, 6, "marypoppins@school.com", "Mary", "Poppins", null });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Department", "Email", "FirstName", "LastName", "PhotoPath" },
                values: new object[] { 2, 1, "johnwayne@school.com", "John", "Wayne", null });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Department", "Email", "FirstName", "LastName", "PhotoPath" },
                values: new object[] { 3, 7, "brucelee@school.com", "Bruce", "Lee", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
