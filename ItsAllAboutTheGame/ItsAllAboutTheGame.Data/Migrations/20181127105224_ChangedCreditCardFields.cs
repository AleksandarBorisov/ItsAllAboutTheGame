using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItsAllAboutTheGame.Data.Migrations
{
    public partial class ChangedCreditCardFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "da9f7866-35e2-40a6-87be-894d332646cb", "19284af7-586b-4982-a63a-e7b3f9961f77" });

            migrationBuilder.DropColumn(
                name: "CardName",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "PaymentToken",
                table: "CreditCards");

            migrationBuilder.AddColumn<string>(
                name: "CVV",
                table: "CreditCards",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "CreditCards",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "CreditCards",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastDigits",
                table: "CreditCards",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fed285c1-b426-44c3-9528-3508e8ede166", "b7b386c6-9b9b-4895-8288-1d5ba024542e", "Admin", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "fed285c1-b426-44c3-9528-3508e8ede166", "b7b386c6-9b9b-4895-8288-1d5ba024542e" });

            migrationBuilder.DropColumn(
                name: "CVV",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "LastDigits",
                table: "CreditCards");

            migrationBuilder.AddColumn<string>(
                name: "CardName",
                table: "CreditCards",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentToken",
                table: "CreditCards",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "da9f7866-35e2-40a6-87be-894d332646cb", "19284af7-586b-4982-a63a-e7b3f9961f77", "Admin", null });
        }
    }
}
