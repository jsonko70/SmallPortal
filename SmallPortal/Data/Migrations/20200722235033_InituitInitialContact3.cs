using Microsoft.EntityFrameworkCore.Migrations;

namespace SmallPortal.Data.Migrations
{
    public partial class InituitInitialContact3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "box1",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "box10",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "box13",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "box14",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "box15a",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "box15b",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "box16",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "box17",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "box2",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "box3",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "box4",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "box5",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "box6",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "box7",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "box8",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "box9",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "box1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box10",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box13",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box14",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box15a",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box15b",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box16",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box17",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box2",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box3",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box4",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box5",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box6",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box7",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box8",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "box9",
                table: "AspNetUsers");
        }
    }
}
