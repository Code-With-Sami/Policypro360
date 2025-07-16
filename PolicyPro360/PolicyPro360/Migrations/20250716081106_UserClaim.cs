using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolicyPro360.Migrations
{
    /// <inheritdoc />
    public partial class UserClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PolicyCategoryId = table.Column<int>(type: "int", nullable: false),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    DateOfIncident = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IncidentDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ClaimedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserRequest = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SupportingDocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_UserClaims", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_UserClaims");
        }
    }
}
