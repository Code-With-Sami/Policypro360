using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolicyPro360.Migrations
{
    /// <inheritdoc />
    public partial class updateagainsuserclaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Category_CategoryId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_UserClaims_CategoryId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Tbl_UserClaims");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_UserClaims_PolicyCategoryId",
                table: "Tbl_UserClaims",
                column: "PolicyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_UserClaims_UserId",
                table: "Tbl_UserClaims",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Category_PolicyCategoryId",
                table: "Tbl_UserClaims",
                column: "PolicyCategoryId",
                principalTable: "Tbl_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Users_UserId",
                table: "Tbl_UserClaims",
                column: "UserId",
                principalTable: "Tbl_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Category_PolicyCategoryId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Users_UserId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_UserClaims_PolicyCategoryId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropIndex(
                name: "IX_Tbl_UserClaims_UserId",
                table: "Tbl_UserClaims");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Category_CategoryId",
                table: "Tbl_UserClaims",
                column: "CategoryId",
                principalTable: "Tbl_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
