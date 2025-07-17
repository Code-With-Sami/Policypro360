using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolicyPro360.Migrations
{
    /// <inheritdoc />
    public partial class AddPolicyIdToLoanRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PolicyId",
                table: "Tbl_LoanRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_LoanRequests_PolicyId",
                table: "Tbl_LoanRequests",
                column: "PolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_LoanRequests_Tbl_Policy_PolicyId",
                table: "Tbl_LoanRequests",
                column: "PolicyId",
                principalTable: "Tbl_Policy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_LoanRequests_Tbl_Policy_PolicyId",
                table: "Tbl_LoanRequests");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_LoanRequests_PolicyId",
                table: "Tbl_LoanRequests");

            migrationBuilder.DropColumn(
                name: "PolicyId",
                table: "Tbl_LoanRequests");
        }
    }
}
