using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations.SqliteMigrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email1 = table.Column<string>(nullable: true),
                    Email2 = table.Column<string>(nullable: true),
                    F_name = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    L_name = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Phone2 = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CandedatesCvTransactions",
                columns: table => new
                {
                    TransId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Candedateid = table.Column<int>(nullable: false),
                    Candedatename = table.Column<string>(nullable: true),
                    AddedFileText = table.Column<string>(nullable: true),
                    Transdate = table.Column<string>(nullable: true),
                    AddedFilePath = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastModifiedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandedatesCvTransactions", x => x.TransId);
                    table.ForeignKey(
                        name: "FK_CandedatesCvTransactions_Candidates_Candedateid",
                        column: x => x.Candedateid,
                        principalTable: "Candidates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandedatesCvTransactions_Candedateid",
                table: "CandedatesCvTransactions",
                column: "Candedateid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandedatesCvTransactions");

            migrationBuilder.DropTable(
                name: "Candidates");
        }
    }
}
