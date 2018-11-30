using Microsoft.EntityFrameworkCore.Migrations;

namespace ItsAllAboutTheGame.Data.Migrations
{
    public partial class Restrictions_For_Decimals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "295d00ca-5e59-4999-9c9b-58e102b67e30", "4c12cd5d-a438-4999-ba39-82ae15ed524e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "e6e41ff1-9c13-4e60-b31a-b5ce85b29571", "0584674f-61d5-4b47-b684-f87da4042aeb" });

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Wallets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2a72b974-47b8-4575-a219-bbb9cd97ba3d", "5733777b-b2ad-4282-b012-cc570b96b244", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c9e4112c-de04-4e88-8425-856b7b4ab3d0", "17400254-154e-4847-aedf-00940dbf0201", "MasterAdministrator", "MASTERADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "2a72b974-47b8-4575-a219-bbb9cd97ba3d", "5733777b-b2ad-4282-b012-cc570b96b244" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "c9e4112c-de04-4e88-8425-856b7b4ab3d0", "17400254-154e-4847-aedf-00940dbf0201" });

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Wallets",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "295d00ca-5e59-4999-9c9b-58e102b67e30", "4c12cd5d-a438-4999-ba39-82ae15ed524e", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e6e41ff1-9c13-4e60-b31a-b5ce85b29571", "0584674f-61d5-4b47-b684-f87da4042aeb", "MasterAdministrator", "MASTERADMINISTRATOR" });
        }
    }
}
