using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renameorderIdcolum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProviderPaymentId",
                table: "Enrollments");

            migrationBuilder.RenameColumn(
                name: "ProviderPaymentId",
                table: "Payments",
                newName: "OrderId");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentId",
                table: "Enrollments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Enrollments");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Payments",
                newName: "ProviderPaymentId");

            migrationBuilder.AddColumn<string>(
                name: "ProviderPaymentId",
                table: "Enrollments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
