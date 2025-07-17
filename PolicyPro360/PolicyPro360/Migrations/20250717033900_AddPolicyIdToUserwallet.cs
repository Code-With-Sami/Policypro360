using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolicyPro360.Migrations
{
    /// <inheritdoc />
    public partial class AddPolicyIdToUserwallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tbl_UserWallet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PolicyId",
                table: "Tbl_UserWallet",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_UserWallet_PolicyId",
                table: "Tbl_UserWallet",
                column: "PolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserWallet_Tbl_Policy_PolicyId",
                table: "Tbl_UserWallet",
                column: "PolicyId",
                principalTable: "Tbl_Policy",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserWallet_Tbl_Policy_PolicyId",
                table: "Tbl_UserWallet");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_UserWallet_PolicyId",
                table: "Tbl_UserWallet");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tbl_UserWallet");

            migrationBuilder.DropColumn(
                name: "PolicyId",
                table: "Tbl_UserWallet");
        }
    }
}
