using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "InvoiceItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "InvoiceId",
                table: "InvoiceItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InvoiceId1",
                table: "InvoiceItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Employees",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId1",
                table: "InvoiceItems",
                column: "InvoiceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItems_Invoices_InvoiceId1",
                table: "InvoiceItems",
                column: "InvoiceId1",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItems_Invoices_InvoiceId1",
                table: "InvoiceItems");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItems_InvoiceId1",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "InvoiceId1",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Employees");

            migrationBuilder.AlterColumn<Guid>(
                name: "InvoiceId",
                table: "InvoiceItems",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "InvoiceItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "InvoiceItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "InvoiceItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
