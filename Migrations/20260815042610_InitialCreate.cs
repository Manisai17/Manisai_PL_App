using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manisai_PL_App.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aadharmasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aadharnumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Middlename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fathername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<DateOnly>(type: "date", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aadharmasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BasicDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Emailid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pannumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aadharnumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pincode = table.Column<int>(type: "int", nullable: true),
                    Dob = table.Column<DateOnly>(type: "date", nullable: true),
                    Leadid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Appstatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Appstage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Statusremarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companymasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Companyname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companymasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctypemasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Doctype = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctypemasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Otpmasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OtpCode = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otpmasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Panmasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pancardnumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Middlename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fathername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Panmasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pincodemasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pincode = table.Column<int>(type: "int", nullable: true),
                    Circle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Village = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Servicable = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pincodemasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rulesmasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rulename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Minvalue = table.Column<int>(type: "int", nullable: true),
                    Maxvalue = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rulesmasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usermasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usermasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bankdetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Appid = table.Column<int>(type: "int", nullable: true),
                    Bankname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bankbranch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ifsccode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Acctype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accnumber = table.Column<int>(type: "int", nullable: true),
                    Accholdername = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bankdetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bankdetails_BasicDetail_Appid",
                        column: x => x.Appid,
                        principalTable: "BasicDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Companydetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppId = table.Column<int>(type: "int", nullable: true),
                    Companyname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Companymailid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grossincome = table.Column<int>(type: "int", nullable: true),
                    Obligations = table.Column<int>(type: "int", nullable: true),
                    Companyaddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companydetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companydetails_BasicDetail_AppId",
                        column: x => x.AppId,
                        principalTable: "BasicDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Docuploaddetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Appid = table.Column<int>(type: "int", nullable: true),
                    Doctype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Docpath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docuploaddetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Docuploaddetails_BasicDetail_Appid",
                        column: x => x.Appid,
                        principalTable: "BasicDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Loandetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Appid = table.Column<int>(type: "int", nullable: true),
                    Appliedamount = table.Column<int>(type: "int", nullable: true),
                    Appliedtenure = table.Column<int>(type: "int", nullable: true),
                    Roi = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Emi = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Approvedamount = table.Column<int>(type: "int", nullable: true),
                    Approvedtenure = table.Column<int>(type: "int", nullable: true),
                    Approvedroi = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Approvedemi = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Approvaldate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loandetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loandetails_BasicDetail_Appid",
                        column: x => x.Appid,
                        principalTable: "BasicDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Personaldetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Appid = table.Column<int>(type: "int", nullable: true),
                    Fathername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mothername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Permanentaddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currentaddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference1name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference1email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference1mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference1relation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference2name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference2email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reference2mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference2relation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personaldetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personaldetails_BasicDetail_Appid",
                        column: x => x.Appid,
                        principalTable: "BasicDetail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bankdetails_Appid",
                table: "Bankdetails",
                column: "Appid");

            migrationBuilder.CreateIndex(
                name: "IX_Companydetails_AppId",
                table: "Companydetails",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_Docuploaddetails_Appid",
                table: "Docuploaddetails",
                column: "Appid");

            migrationBuilder.CreateIndex(
                name: "IX_Loandetails_Appid",
                table: "Loandetails",
                column: "Appid");

            migrationBuilder.CreateIndex(
                name: "IX_Personaldetails_Appid",
                table: "Personaldetails",
                column: "Appid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aadharmasters");

            migrationBuilder.DropTable(
                name: "Bankdetails");

            migrationBuilder.DropTable(
                name: "Companydetails");

            migrationBuilder.DropTable(
                name: "Companymasters");

            migrationBuilder.DropTable(
                name: "Doctypemasters");

            migrationBuilder.DropTable(
                name: "Docuploaddetails");

            migrationBuilder.DropTable(
                name: "Loandetails");

            migrationBuilder.DropTable(
                name: "Otpmasters");

            migrationBuilder.DropTable(
                name: "Panmasters");

            migrationBuilder.DropTable(
                name: "Personaldetails");

            migrationBuilder.DropTable(
                name: "Pincodemasters");

            migrationBuilder.DropTable(
                name: "Rulesmasters");

            migrationBuilder.DropTable(
                name: "Usermasters");

            migrationBuilder.DropTable(
                name: "BasicDetail");
        }
    }
}
