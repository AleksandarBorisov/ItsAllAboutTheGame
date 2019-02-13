using Microsoft.EntityFrameworkCore.Migrations;

namespace ItsAllAboutTheGame.Data.Migrations
{
    public partial class Added_Currency_To_Transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "206b8c29-579b-43f6-975e-b8d39cafbf07", "8c3a04e2-7112-41c6-a63f-b1d9de3eb5f0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "4901627d-334c-4528-9648-514e3b983294", "87fac042-617a-443b-9386-5d4c3fce1705" });

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Transactions",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6db218f5-8151-45f9-81f1-b33628296aa0", "9017dc8f-b371-44f0-a87f-122f19d3e5a7", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1d9f51ad-2e9b-4472-a92e-9aa7c2f11edc", "e406f80a-2fe2-4149-a1b2-5f46a60e923e", "MasterAdministrator", "MASTERADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "1d9f51ad-2e9b-4472-a92e-9aa7c2f11edc", "e406f80a-2fe2-4149-a1b2-5f46a60e923e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "6db218f5-8151-45f9-81f1-b33628296aa0", "9017dc8f-b371-44f0-a87f-122f19d3e5a7" });

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Transactions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4901627d-334c-4528-9648-514e3b983294", "87fac042-617a-443b-9386-5d4c3fce1705", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "206b8c29-579b-43f6-975e-b8d39cafbf07", "8c3a04e2-7112-41c6-a63f-b1d9de3eb5f0", "MasterAdministrator", "MASTERADMINISTRATOR" });
        }
    }
}
