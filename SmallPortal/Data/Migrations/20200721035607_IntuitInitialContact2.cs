using Microsoft.EntityFrameworkCore.Migrations;

namespace SmallPortal.Data.Migrations
{
    public partial class IntuitInitialContact2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "AspNetUsers");
        }
    }
}
