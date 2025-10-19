using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDateTimeFieldsToDateTimeOffset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "TaskManagement",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ea55379d-2e28-47d3-af4e-c65ec526d831"));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                schema: "TaskManagement",
                table: "Users",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "TaskManagement",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                schema: "TaskManagement",
                table: "TaskItems",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DueDate",
                schema: "TaskManagement",
                table: "TaskItems",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "TaskManagement",
                table: "TaskItems",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CompletedAt",
                schema: "TaskManagement",
                table: "TaskItems",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.InsertData(
                schema: "TaskManagement",
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "FirstName", "IsActive", "LastName", "PasswordHash", "UpdatedAt", "UpdatedById", "Username" },
                values: new object[] { new Guid("22f507b0-70b0-4db7-9d5f-889e6a0b52f0"), new DateTimeOffset(new DateTime(2025, 10, 19, 15, 10, 28, 406, DateTimeKind.Unspecified).AddTicks(462), new TimeSpan(0, 0, 0, 0, 0)), null, "Admin", true, "User", "$2a$11$i/oDv1AMbnpb3aKjDCbUruU7jd7aooC4kduKtSF2RWu4x84qawiOy", null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "TaskManagement",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22f507b0-70b0-4db7-9d5f-889e6a0b52f0"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "TaskManagement",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "TaskManagement",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "TaskManagement",
                table: "TaskItems",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                schema: "TaskManagement",
                table: "TaskItems",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "TaskManagement",
                table: "TaskItems",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedAt",
                schema: "TaskManagement",
                table: "TaskItems",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.InsertData(
                schema: "TaskManagement",
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "FirstName", "IsActive", "LastName", "PasswordHash", "UpdatedAt", "UpdatedById", "Username" },
                values: new object[] { new Guid("ea55379d-2e28-47d3-af4e-c65ec526d831"), new DateTime(2025, 10, 18, 22, 25, 49, 618, DateTimeKind.Utc).AddTicks(4506), null, "Admin", true, "User", "$2a$11$mITQxB3atxFFn290HLXlnujdCtL7WlEOc4kQcmHSmpBrH6GrpjGIe", null, null, "admin" });
        }
    }
}
