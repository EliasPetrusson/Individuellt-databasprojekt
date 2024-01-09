using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELIASPETRUSSON.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Elever",
                columns: table => new
                {
                    ElevId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Namn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Personnummer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Klass = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elever", x => x.ElevId);
                });

            migrationBuilder.CreateTable(
                name: "Kurser",
                columns: table => new
                {
                    KursId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kursnamn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kurser", x => x.KursId);
                });

            migrationBuilder.CreateTable(
                name: "PersonalSet",
                columns: table => new
                {
                    PersonalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Namn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Befattning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Anställningsdatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Avdelning = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalSet", x => x.PersonalId);
                });

            migrationBuilder.CreateTable(
                name: "BetygSet",
                columns: table => new
                {
                    BetygId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElevId = table.Column<int>(type: "int", nullable: false),
                    KursId = table.Column<int>(type: "int", nullable: false),
                    Betygsgrad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BetygDatum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetygSet", x => x.BetygId);
                    table.ForeignKey(
                        name: "FK_BetygSet_Elever_ElevId",
                        column: x => x.ElevId,
                        principalTable: "Elever",
                        principalColumn: "ElevId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BetygSet_Kurser_KursId",
                        column: x => x.KursId,
                        principalTable: "Kurser",
                        principalColumn: "KursId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BetygSet_ElevId",
                table: "BetygSet",
                column: "ElevId");

            migrationBuilder.CreateIndex(
                name: "IX_BetygSet_KursId",
                table: "BetygSet",
                column: "KursId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BetygSet");

            migrationBuilder.DropTable(
                name: "PersonalSet");

            migrationBuilder.DropTable(
                name: "Elever");

            migrationBuilder.DropTable(
                name: "Kurser");
        }
    }
}
