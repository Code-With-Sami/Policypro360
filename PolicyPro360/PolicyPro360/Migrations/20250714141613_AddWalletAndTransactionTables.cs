using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolicyPro360.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletAndTransactionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_AdminWallet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_AdminWallet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_AdminWallet_Tbl_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tbl_Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tbl_AdminWallet_Tbl_Policy_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Tbl_Policy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tbl_AdminWallet_Tbl_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Tbl_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_CompanyWallet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_CompanyWallet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_CompanyWallet_Tbl_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tbl_Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tbl_CompanyWallet_Tbl_Policy_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Tbl_Policy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tbl_CompanyWallet_Tbl_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Tbl_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_TransactionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromId = table.Column<int>(type: "int", nullable: false),
                    ToType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    PolicyId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_TransactionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_TransactionHistory_Tbl_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tbl_Company",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tbl_TransactionHistory_Tbl_Policy_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Tbl_Policy",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_AdminWallet_CompanyId",
                table: "Tbl_AdminWallet",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_AdminWallet_PolicyId",
                table: "Tbl_AdminWallet",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_AdminWallet_UserId",
                table: "Tbl_AdminWallet",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_CompanyWallet_CompanyId",
                table: "Tbl_CompanyWallet",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_CompanyWallet_PolicyId",
                table: "Tbl_CompanyWallet",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_CompanyWallet_UserId",
                table: "Tbl_CompanyWallet",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_TransactionHistory_CompanyId",
                table: "Tbl_TransactionHistory",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_TransactionHistory_PolicyId",
                table: "Tbl_TransactionHistory",
                column: "PolicyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_AdminWallet");

            migrationBuilder.DropTable(
                name: "Tbl_CompanyWallet");

            migrationBuilder.DropTable(
                name: "Tbl_TransactionHistory");
        }
    }
}
