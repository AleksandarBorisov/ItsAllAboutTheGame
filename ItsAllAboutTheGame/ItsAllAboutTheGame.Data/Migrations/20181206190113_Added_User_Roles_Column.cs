using Microsoft.EntityFrameworkCore.Migrations;

namespace ItsAllAboutTheGame.Data.Migrations
{
    public partial class Added_User_Roles_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "2a72b974-47b8-4575-a219-bbb9cd97ba3d", "5733777b-b2ad-4282-b012-cc570b96b244" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "c9e4112c-de04-4e88-8425-856b7b4ab3d0", "17400254-154e-4847-aedf-00940dbf0201" });

            migrationBuilder.DropColumn(
                name: "Image",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4901627d-334c-4528-9648-514e3b983294", "87fac042-617a-443b-9386-5d4c3fce1705", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "206b8c29-579b-43f6-975e-b8d39cafbf07", "8c3a04e2-7112-41c6-a63f-b1d9de3eb5f0", "MasterAdministrator", "MASTERADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "206b8c29-579b-43f6-975e-b8d39cafbf07", "8c3a04e2-7112-41c6-a63f-b1d9de3eb5f0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "4901627d-334c-4528-9648-514e3b983294", "87fac042-617a-443b-9386-5d4c3fce1705" });

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2a72b974-47b8-4575-a219-bbb9cd97ba3d", "5733777b-b2ad-4282-b012-cc570b96b244", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c9e4112c-de04-4e88-8425-856b7b4ab3d0", "17400254-154e-4847-aedf-00940dbf0201", "MasterAdministrator", "MASTERADMINISTRATOR" });
        }
    }
}
