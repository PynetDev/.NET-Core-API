using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class DataAddedtoStudentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "dateOfBirth",
                table: "Students",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "id", "address", "dateOfBirth", "email", "name", "password" },
                values: new object[,]
                {
                    { 1, "Miami, USA", new DateOnly(2017, 6, 17), "john@gmail.com", "John", "john@123" },
                    { 2, "Texas, USA", new DateOnly(2014, 3, 29), "smith@gmail.com", "smith", "smith@123" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "dateOfBirth",
                table: "Students");
        }
    }
}
