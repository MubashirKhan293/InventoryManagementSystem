using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryManagement.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Products_ProductId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Products_ProductId",
                table: "Sales");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Products_ProductId",
                table: "Purchases",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Products_ProductId",
                table: "Sales",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Products_ProductId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Products_ProductId",
                table: "Sales");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedDate", "Description", "Name", "Quantity" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 24, 11, 6, 3, 15, DateTimeKind.Utc).AddTicks(5327), "Gaming laptop with high performance", "Laptop", 10 },
                    { 2, new DateTime(2025, 5, 24, 11, 6, 3, 15, DateTimeKind.Utc).AddTicks(5331), "Wireless optical mouse", "Mouse", 50 },
                    { 3, new DateTime(2025, 5, 24, 11, 6, 3, 15, DateTimeKind.Utc).AddTicks(5334), "Mechanical gaming keyboard", "Keyboard", 30 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Products_ProductId",
                table: "Purchases",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Products_ProductId",
                table: "Sales",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
