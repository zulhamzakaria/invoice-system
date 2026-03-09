using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class setInvoiceApprovalDatetonullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ApprovalDate",
                table: "Invoices",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<double>(
                name: "RiskScore",
                table: "Invoices",
                type: "double precision",
                nullable: true,
                computedColumnSql: "(\"RiskAssessment\"->>'RiskScore')::double precision",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_RiskScore",
                table: "Invoices",
                column: "RiskScore");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ApprovalDate",
                table: "Invoices",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.DropIndex(name: "IX_Invoices_RiskScore", table: "Invoices");
            migrationBuilder.DropColumn(name: "RiskScore", table: "Invoices");
        }
    }
}
