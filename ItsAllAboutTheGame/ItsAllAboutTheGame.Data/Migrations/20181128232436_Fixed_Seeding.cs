using Microsoft.EntityFrameworkCore.Migrations;

namespace ItsAllAboutTheGame.Data.Migrations
{
    public partial class Fixed_Seeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "25b1168a-7d4d-4420-9506-299cd815ad92", "02c3341f-aced-4eee-be7e-96778dedad4f" });

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
                values: new object[] { "295d00ca-5e59-4999-9c9b-58e102b67e30", "4c12cd5d-a438-4999-ba39-82ae15ed524e", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e6e41ff1-9c13-4e60-b31a-b5ce85b29571", "0584674f-61d5-4b47-b684-f87da4042aeb", "MasterAdministrator", "MASTERADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "295d00ca-5e59-4999-9c9b-58e102b67e30", "4c12cd5d-a438-4999-ba39-82ae15ed524e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "e6e41ff1-9c13-4e60-b31a-b5ce85b29571", "0584674f-61d5-4b47-b684-f87da4042aeb" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "25b1168a-7d4d-4420-9506-299cd815ad92", "02c3341f-aced-4eee-be7e-96778dedad4f", "Admin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b225df6e-f7c8-454c-b9fc-c070bda8913a", "628bf654-d973-458e-adad-50975f8a117f", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4f171067-1442-498b-9a2e-21fb47f82851", "73e17b37-e688-427a-b410-01f8e54a271b", "MasterAdministrator", "MASTERADMINISTRATOR" });
        }
    }
}
