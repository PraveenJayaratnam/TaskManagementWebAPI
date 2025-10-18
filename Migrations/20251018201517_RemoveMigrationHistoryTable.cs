using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMigrationHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MigrationHistory",
                schema: "TaskManagement");

            migrationBuilder.DeleteData(
                schema: "TaskManagement",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2dac8de8-4763-4888-ae7a-9dcdfb0460a4"));

            migrationBuilder.InsertData(
                schema: "TaskManagement",
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "FirstName", "IsActive", "LastName", "PasswordHash", "UpdatedAt", "UpdatedById", "Username" },
                values: new object[] { new Guid("ea2a6d8e-54c0-4f52-b224-9faed20950c2"), new DateTime(2025, 10, 18, 20, 15, 17, 244, DateTimeKind.Utc).AddTicks(2723), null, "Admin", true, "User", "$2a$11$4F5jICmzftf/zk3YJiu9HO2Tq0ULiuaRFysfZAI4K4hAfJsbZ6ZCS", null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "TaskManagement",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ea2a6d8e-54c0-4f52-b224-9faed20950c2"));

            migrationBuilder.CreateTable(
                name: "MigrationHistory",
                schema: "TaskManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppliedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Environment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsRolledBack = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MigrationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RollbackScript = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RolledBackAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RolledBackBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MigrationHistory", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "TaskManagement",
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "FirstName", "IsActive", "LastName", "PasswordHash", "UpdatedAt", "UpdatedById", "Username" },
                values: new object[] { new Guid("2dac8de8-4763-4888-ae7a-9dcdfb0460a4"), new DateTime(2025, 10, 18, 20, 12, 30, 834, DateTimeKind.Utc).AddTicks(8582), null, "Admin", true, "User", "$2a$11$8eSLzmEdDTOa1j8lzFooPe63Fn4s4xqvcd35uW0.DyjONnFM8U6Fy", null, null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_MigrationHistory_AppliedAt",
                schema: "TaskManagement",
                table: "MigrationHistory",
                column: "AppliedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MigrationHistory_Environment",
                schema: "TaskManagement",
                table: "MigrationHistory",
                column: "Environment");

            migrationBuilder.CreateIndex(
                name: "IX_MigrationHistory_MigrationName_Version",
                schema: "TaskManagement",
                table: "MigrationHistory",
                columns: new[] { "MigrationName", "Version" },
                unique: true);
        }
    }
}
