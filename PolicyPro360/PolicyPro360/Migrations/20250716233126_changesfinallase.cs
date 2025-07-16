using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolicyPro360.Migrations
{
    /// <inheritdoc />
    public partial class changesfinallase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Policy_Tbl_Category_PolicyTypeId",
                table: "Tbl_Policy");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Policy_Tbl_Company_CompanyId",
                table: "Tbl_Policy");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Category_PolicyCategoryId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Policy_PolicyId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Users_UserId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserPolicy_Tbl_Policy_PolicyId",
                table: "Tbl_UserPolicy");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserPolicy_Tbl_Users_UserId",
                table: "Tbl_UserPolicy");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Policy_Tbl_Category_PolicyTypeId",
                table: "Tbl_Policy",
                column: "PolicyTypeId",
                principalTable: "Tbl_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Policy_Tbl_Company_CompanyId",
                table: "Tbl_Policy",
                column: "CompanyId",
                principalTable: "Tbl_Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Category_PolicyCategoryId",
                table: "Tbl_UserClaims",
                column: "PolicyCategoryId",
                principalTable: "Tbl_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Policy_PolicyId",
                table: "Tbl_UserClaims",
                column: "PolicyId",
                principalTable: "Tbl_Policy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Users_UserId",
                table: "Tbl_UserClaims",
                column: "UserId",
                principalTable: "Tbl_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserPolicy_Tbl_Policy_PolicyId",
                table: "Tbl_UserPolicy",
                column: "PolicyId",
                principalTable: "Tbl_Policy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserPolicy_Tbl_Users_UserId",
                table: "Tbl_UserPolicy",
                column: "UserId",
                principalTable: "Tbl_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Policy_Tbl_Category_PolicyTypeId",
                table: "Tbl_Policy");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_Policy_Tbl_Company_CompanyId",
                table: "Tbl_Policy");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Category_PolicyCategoryId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Policy_PolicyId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Users_UserId",
                table: "Tbl_UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserPolicy_Tbl_Policy_PolicyId",
                table: "Tbl_UserPolicy");

            migrationBuilder.DropForeignKey(
                name: "FK_Tbl_UserPolicy_Tbl_Users_UserId",
                table: "Tbl_UserPolicy");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Policy_Tbl_Category_PolicyTypeId",
                table: "Tbl_Policy",
                column: "PolicyTypeId",
                principalTable: "Tbl_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_Policy_Tbl_Company_CompanyId",
                table: "Tbl_Policy",
                column: "CompanyId",
                principalTable: "Tbl_Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Category_PolicyCategoryId",
                table: "Tbl_UserClaims",
                column: "PolicyCategoryId",
                principalTable: "Tbl_Category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Policy_PolicyId",
                table: "Tbl_UserClaims",
                column: "PolicyId",
                principalTable: "Tbl_Policy",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserClaims_Tbl_Users_UserId",
                table: "Tbl_UserClaims",
                column: "UserId",
                principalTable: "Tbl_Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserPolicy_Tbl_Policy_PolicyId",
                table: "Tbl_UserPolicy",
                column: "PolicyId",
                principalTable: "Tbl_Policy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tbl_UserPolicy_Tbl_Users_UserId",
                table: "Tbl_UserPolicy",
                column: "UserId",
                principalTable: "Tbl_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
