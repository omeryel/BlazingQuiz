using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazingQuiz.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AT_totalQuestion_Quizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalQuesions",
                table: "Quizzes",
                newName: "TotalQuestions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFQJt5Nu8F7uVMWGqOCeTT+O+ls4/UDKZgn75DR9E19lkf/UK1VX7vsdS+LX0ws1iQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalQuestions",
                table: "Quizzes",
                newName: "TotalQuesions");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHdqVvXj+cNxpD+NViVA26UKIfwQEzN6hsT4m8j01NUpHE6jP1YLe50ILOEac9n5fQ==");
        }
    }
}
