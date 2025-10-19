using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "TaskManagement",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("283ca4f4-6ea2-4048-8da0-b9f974798f43"));

            migrationBuilder.InsertData(
                schema: "TaskManagement",
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "FirstName", "IsActive", "LastName", "PasswordHash", "UpdatedAt", "UpdatedById", "Username" },
                values: new object[] { new Guid("20d0eb93-d4bd-4b39-b72a-275c0ab501de"), new DateTime(2025, 10, 18, 21, 34, 39, 780, DateTimeKind.Utc).AddTicks(1573), null, "Admin", true, "User", "$2a$11$qrxefsUOPlbTGfzEIkbA6.EzhKScqAshAzmh0IMmpby1jRu5.dN6.", null, null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedById",
                schema: "TaskManagement",
                table: "Users",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedById",
                schema: "TaskManagement",
                table: "Users",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_CreatedById",
                schema: "TaskManagement",
                table: "TaskItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_UpdatedById",
                schema: "TaskManagement",
                table: "TaskItems",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_Users_CreatedById",
                schema: "TaskManagement",
                table: "TaskItems",
                column: "CreatedById",
                principalSchema: "TaskManagement",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_Users_UpdatedById",
                schema: "TaskManagement",
                table: "TaskItems",
                column: "UpdatedById",
                principalSchema: "TaskManagement",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatedById",
                schema: "TaskManagement",
                table: "Users",
                column: "CreatedById",
                principalSchema: "TaskManagement",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UpdatedById",
                schema: "TaskManagement",
                table: "Users",
                column: "UpdatedById",
                principalSchema: "TaskManagement",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_Users_CreatedById",
                schema: "TaskManagement",
                table: "TaskItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_Users_UpdatedById",
                schema: "TaskManagement",
                table: "TaskItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreatedById",
                schema: "TaskManagement",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UpdatedById",
                schema: "TaskManagement",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreatedById",
                schema: "TaskManagement",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UpdatedById",
                schema: "TaskManagement",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TaskItems_CreatedById",
                schema: "TaskManagement",
                table: "TaskItems");

            migrationBuilder.DropIndex(
                name: "IX_TaskItems_UpdatedById",
                schema: "TaskManagement",
                table: "TaskItems");

            migrationBuilder.DeleteData(
                schema: "TaskManagement",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("20d0eb93-d4bd-4b39-b72a-275c0ab501de"));

            migrationBuilder.InsertData(
                schema: "TaskManagement",
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "FirstName", "IsActive", "LastName", "PasswordHash", "UpdatedAt", "UpdatedById", "Username" },
                values: new object[] { new Guid("283ca4f4-6ea2-4048-8da0-b9f974798f43"), new DateTime(2025, 10, 18, 20, 18, 38, 931, DateTimeKind.Utc).AddTicks(1292), null, "Admin", true, "User", "$2a$11$t9bKfkMo/wdnCmZGydr2wuDg0QoEG2aZz1siKhQ.gfcD4FOX/dmj2", null, null, "admin" });
        }
    }
}
