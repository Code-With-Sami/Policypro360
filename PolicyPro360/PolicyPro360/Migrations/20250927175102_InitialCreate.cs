using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolicyPro360.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_Admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Admin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Blog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FeaturedImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Blog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndustryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyLogoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerDOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerNationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerPhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerCNIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Contact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Contact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_FAQ",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_FAQ", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_PrivacyPolicy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_PrivacyPolicy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Quiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Quiz", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_TermsCondition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_TermsCondition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_UserPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_UserPayment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Users", x => x.Id);
                });

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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tbl_Policy_Tbl_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Tbl_Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_QuizQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    QuestionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_QuizQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_QuizQuestion_Tbl_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Tbl_Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_QuizResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    ScoresJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuggestedPolicyIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AiResponseJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_QuizResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_QuizResult_Tbl_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Tbl_Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Testimonial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Testimonial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_Testimonial_Tbl_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Tbl_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_UserSupport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_UserSupport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_UserSupport_Tbl_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Tbl_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "Tbl_LoanRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    LoanAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LoanType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationInMonths = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisbursedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_LoanRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_LoanRequests_Tbl_Policy_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Tbl_Policy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tbl_LoanRequests_Tbl_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Tbl_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    table.ForeignKey(
                        name: "FK_Tbl_UserClaims_Tbl_Category_PolicyCategoryId",
                        column: x => x.PolicyCategoryId,
                        principalTable: "Tbl_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tbl_UserClaims_Tbl_Policy_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Tbl_Policy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tbl_UserClaims_Tbl_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Tbl_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_UserPolicy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CalculatedPremium = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CoverageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_UserPolicy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_UserPolicy_Tbl_Policy_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Tbl_Policy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tbl_UserPolicy_Tbl_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Tbl_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_UserWallet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PolicyId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_UserWallet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_UserWallet_Tbl_Policy_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Tbl_Policy",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tbl_UserWallet_Tbl_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Tbl_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_QuizOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CategoryWeightsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_QuizOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_QuizOption_Tbl_QuizQuestion_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Tbl_QuizQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_QuizAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResultId = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    OptionIdsCsv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RawAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_QuizAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_QuizAnswer_Tbl_QuizQuestion_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Tbl_QuizQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tbl_QuizAnswer_Tbl_QuizResult_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Tbl_QuizResult",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_LoanInstallments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanRequestId = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_LoanInstallments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_LoanInstallments_Tbl_LoanRequests_LoanRequestId",
                        column: x => x.LoanRequestId,
                        principalTable: "Tbl_LoanRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_LoanPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoanInstallmentId = table.Column<int>(type: "int", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_LoanPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tbl_LoanPayments_Tbl_LoanInstallments_LoanInstallmentId",
                        column: x => x.LoanInstallmentId,
                        principalTable: "Tbl_LoanInstallments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tbl_LoanPayments_Tbl_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Tbl_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Tbl_LoanInstallments_LoanRequestId",
                table: "Tbl_LoanInstallments",
                column: "LoanRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_LoanPayments_LoanInstallmentId",
                table: "Tbl_LoanPayments",
                column: "LoanInstallmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_LoanPayments_UserId",
                table: "Tbl_LoanPayments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_LoanRequests_PolicyId",
                table: "Tbl_LoanRequests",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_LoanRequests_UserId",
                table: "Tbl_LoanRequests",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_QuizAnswer_QuestionId",
                table: "Tbl_QuizAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_QuizAnswer_ResultId",
                table: "Tbl_QuizAnswer",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_QuizOption_QuestionId",
                table: "Tbl_QuizOption",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_QuizQuestion_QuizId",
                table: "Tbl_QuizQuestion",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_QuizResult_QuizId",
                table: "Tbl_QuizResult",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Testimonial_UserId",
                table: "Tbl_Testimonial",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_TransactionHistory_CompanyId",
                table: "Tbl_TransactionHistory",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_TransactionHistory_PolicyId",
                table: "Tbl_TransactionHistory",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_UserClaims_PolicyCategoryId",
                table: "Tbl_UserClaims",
                column: "PolicyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_UserClaims_PolicyId",
                table: "Tbl_UserClaims",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_UserClaims_UserId",
                table: "Tbl_UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_UserPolicy_PolicyId",
                table: "Tbl_UserPolicy",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_UserPolicy_UserId",
                table: "Tbl_UserPolicy",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_UserSupport_UserId",
                table: "Tbl_UserSupport",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_UserWallet_PolicyId",
                table: "Tbl_UserWallet",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_UserWallet_UserId",
                table: "Tbl_UserWallet",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_Admin");

            migrationBuilder.DropTable(
                name: "Tbl_AdminWallet");

            migrationBuilder.DropTable(
                name: "Tbl_Blog");

            migrationBuilder.DropTable(
                name: "Tbl_CompanyWallet");

            migrationBuilder.DropTable(
                name: "Tbl_Contact");

            migrationBuilder.DropTable(
                name: "Tbl_FAQ");

            migrationBuilder.DropTable(
                name: "Tbl_LoanPayments");

            migrationBuilder.DropTable(
                name: "Tbl_PolicyAttributes");

            migrationBuilder.DropTable(
                name: "Tbl_PrivacyPolicy");

            migrationBuilder.DropTable(
                name: "Tbl_QuizAnswer");

            migrationBuilder.DropTable(
                name: "Tbl_QuizOption");

            migrationBuilder.DropTable(
                name: "Tbl_TermsCondition");

            migrationBuilder.DropTable(
                name: "Tbl_Testimonial");

            migrationBuilder.DropTable(
                name: "Tbl_TransactionHistory");

            migrationBuilder.DropTable(
                name: "Tbl_UserClaims");

            migrationBuilder.DropTable(
                name: "Tbl_UserPayment");

            migrationBuilder.DropTable(
                name: "Tbl_UserPolicy");

            migrationBuilder.DropTable(
                name: "Tbl_UserSupport");

            migrationBuilder.DropTable(
                name: "Tbl_UserWallet");

            migrationBuilder.DropTable(
                name: "Tbl_LoanInstallments");

            migrationBuilder.DropTable(
                name: "Tbl_QuizResult");

            migrationBuilder.DropTable(
                name: "Tbl_QuizQuestion");

            migrationBuilder.DropTable(
                name: "Tbl_LoanRequests");

            migrationBuilder.DropTable(
                name: "Tbl_Quiz");

            migrationBuilder.DropTable(
                name: "Tbl_Policy");

            migrationBuilder.DropTable(
                name: "Tbl_Users");

            migrationBuilder.DropTable(
                name: "Tbl_Category");

            migrationBuilder.DropTable(
                name: "Tbl_Company");
        }
    }
}
