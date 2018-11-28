using Microsoft.EntityFrameworkCore.Migrations;

namespace ItsAllAboutTheGame.Data.Migrations
{
    public partial class Seed_Admin_Account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "fed285c1-b426-44c3-9528-3508e8ede166", "b7b386c6-9b9b-4895-8288-1d5ba024542e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fed285c1-b426-44c3-9528-3508e8ede166", "b7b386c6-9b9b-4895-8288-1d5ba024542e", "Admin", null });
        }
    }
}
