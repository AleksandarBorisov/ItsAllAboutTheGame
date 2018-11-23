using Microsoft.EntityFrameworkCore.Migrations;

namespace ItsAllAboutTheGame.Data.Migrations
{
    public partial class RemoveRequiredFromDepositParams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deposits_UserId",
                table: "Deposits");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "d112ab87-de52-4861-ba43-7a23cd2d7086", "93a818ec-e050-4d9d-afca-778f15611e02" });

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Deposits",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "82cbb7d3-9068-4ae5-9d99-37fe0875d05f", "fb367c64-8f2e-4b4d-90c4-c7d0409efbad", "Admin", null });

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_UserId",
                table: "Deposits",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deposits_UserId",
                table: "Deposits");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "82cbb7d3-9068-4ae5-9d99-37fe0875d05f", "fb367c64-8f2e-4b4d-90c4-c7d0409efbad" });

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Deposits",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d112ab87-de52-4861-ba43-7a23cd2d7086", "93a818ec-e050-4d9d-afca-778f15611e02", "Admin", null });

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_UserId",
                table: "Deposits",
                column: "UserId",
                unique: true);
        }
    }
}
