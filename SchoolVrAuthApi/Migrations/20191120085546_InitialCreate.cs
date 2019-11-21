using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolVrAuthApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "LicenseKeys",
                columns: table => new
                {
                    LicenseKeyId = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    NumOfMacAddressesAllowed = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseKeys", x => x.LicenseKeyId);
                    table.ForeignKey(
                        name: "FK_LicenseKeys_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MacAddresses",
                columns: table => new
                {
                    MacAddressId = table.Column<string>(nullable: false),
                    AssignedDt = table.Column<DateTime>(nullable: false),
                    LicenseKeyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacAddresses", x => x.MacAddressId);
                    table.ForeignKey(
                        name: "FK_MacAddresses_LicenseKeys_LicenseKeyId",
                        column: x => x.LicenseKeyId,
                        principalTable: "LicenseKeys",
                        principalColumn: "LicenseKeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseKeys_UserId",
                table: "LicenseKeys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MacAddresses_LicenseKeyId",
                table: "MacAddresses",
                column: "LicenseKeyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MacAddresses");

            migrationBuilder.DropTable(
                name: "LicenseKeys");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
