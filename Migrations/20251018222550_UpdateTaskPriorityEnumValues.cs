using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTaskPriorityEnumValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "TaskManagement",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("20d0eb93-d4bd-4b39-b72a-275c0ab501de"));

            migrationBuilder.InsertData(
                schema: "TaskManagement",
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "FirstName", "IsActive", "LastName", "PasswordHash", "UpdatedAt", "UpdatedById", "Username" },
                values: new object[] { new Guid("ea55379d-2e28-47d3-af4e-c65ec526d831"), new DateTime(2025, 10, 18, 22, 25, 49, 618, DateTimeKind.Utc).AddTicks(4506), null, "Admin", true, "User", "$2a$11$mITQxB3atxFFn290HLXlnujdCtL7WlEOc4kQcmHSmpBrH6GrpjGIe", null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "TaskManagement",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ea55379d-2e28-47d3-af4e-c65ec526d831"));

            migrationBuilder.InsertData(
                schema: "TaskManagement",
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "FirstName", "IsActive", "LastName", "PasswordHash", "UpdatedAt", "UpdatedById", "Username" },
                values: new object[] { new Guid("20d0eb93-d4bd-4b39-b72a-275c0ab501de"), new DateTime(2025, 10, 18, 21, 34, 39, 780, DateTimeKind.Utc).AddTicks(1573), null, "Admin", true, "User", "$2a$11$qrxefsUOPlbTGfzEIkbA6.EzhKScqAshAzmh0IMmpby1jRu5.dN6.", null, null, "admin" });
        }
    }
}
