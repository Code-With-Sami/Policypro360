using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolicyPro360.Migrations
{
    /// <inheritdoc />
    public partial class updatePolicyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_Policy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PolicyTypeId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    SumInsured = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Premium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tenure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsConditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrochureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Policy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_Policy_Tbl_Category_PolicyTypeId",
                        column: x => x.PolicyTypeId,
                        principalTable: "Tbl_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tbl_Policy_Tbl_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tbl_Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_PolicyAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_PolicyAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_PolicyAttributes_Tbl_Policy_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Tbl_Policy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Policy_CompanyId",
                table: "Tbl_Policy",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Policy_PolicyTypeId",
                table: "Tbl_Policy",
                column: "PolicyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_PolicyAttributes_PolicyId",
                table: "Tbl_PolicyAttributes",
                column: "PolicyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_PolicyAttributes");

            migrationBuilder.DropTable(
                name: "Tbl_Policy");
        }
    }
}
