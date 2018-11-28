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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b225df6e-f7c8-454c-b9fc-c070bda8913a", "628bf654-d973-458e-adad-50975f8a117f", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4f171067-1442-498b-9a2e-21fb47f82851", "73e17b37-e688-427a-b410-01f8e54a271b", "MasterAdministrator", "MASTERADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "4f171067-1442-498b-9a2e-21fb47f82851", "73e17b37-e688-427a-b410-01f8e54a271b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "b225df6e-f7c8-454c-b9fc-c070bda8913a", "628bf654-d973-458e-adad-50975f8a117f" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fed285c1-b426-44c3-9528-3508e8ede166", "b7b386c6-9b9b-4895-8288-1d5ba024542e", "Admin", null });
        }
    }
}
