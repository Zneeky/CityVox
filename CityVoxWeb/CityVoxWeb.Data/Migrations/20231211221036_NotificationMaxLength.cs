using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVoxWeb.Data.Migrations
{
    public partial class NotificationMaxLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Notifications",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("446c78ea-0251-4b2c-809f-6f30ba8afcf9"),
                column: "ConcurrencyStamp",
                value: "d2e9a802-0a4d-4492-92a3-17f005946eb2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("99ae3bb9-5e34-42ce-92e0-ea07d45fe244"),
                column: "ConcurrencyStamp",
                value: "b6603ddd-d5e1-47ea-b57e-173d16f80d08");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f86b1ef8-08cd-49a0-8112-18ffb9fea577"),
                column: "ConcurrencyStamp",
                value: "f62e3935-fc6a-4a9d-bd35-2beb9ebabb8d");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Notifications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("446c78ea-0251-4b2c-809f-6f30ba8afcf9"),
                column: "ConcurrencyStamp",
                value: "4b35a3f0-ef90-42f3-8b18-f305c0d45a6d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("99ae3bb9-5e34-42ce-92e0-ea07d45fe244"),
                column: "ConcurrencyStamp",
                value: "6c58d5e4-7438-4d12-826a-b63cd03c2c62");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f86b1ef8-08cd-49a0-8112-18ffb9fea577"),
                column: "ConcurrencyStamp",
                value: "f3de3520-d777-4134-ab37-bd5225696e4e");
        }
    }
}
