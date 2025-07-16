using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolicyPro360.Migrations
{
    /// <inheritdoc />
    public partial class updateuserclaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Tbl_UserClaims",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_UserClaims_CategoryId",
                table: "Tbl_UserClaims",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_UserClaims_PolicyId",
                table: "Tbl_UserClaims",
                column: "PolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Category_CategoryId",
                table: "Tbl_UserClaims",
                column: "CategoryId",
                principalTable: "Tbl_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Policy_PolicyId",
                table: "Tbl_UserClaims",
                column: "PolicyId",
                principalTable: "Tbl_Policy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Category_CategoryId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Policy_PolicyId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_UserClaims_CategoryId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_UserClaims_PolicyId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Tbl_UserClaims");
        }
    }
}
