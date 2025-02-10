using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotekaFilmow.Migrations
{
    /// <inheritdoc />
    public partial class DodajKomentarze : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "Uzytkownicy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: true),
                    CzyAdministrator = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownicy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Komentarze",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PozycjaId = table.Column<int>(type: "INTEGER", nullable: false),
                    UzytkownikId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tresc = table.Column<string>(type: "TEXT", nullable: true),
                    DataDodania = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentarze", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Komentarze_Pozycje_PozycjaId",
                        column: x => x.PozycjaId,
                        principalTable: "Pozycje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Komentarze_Uzytkownicy_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "Uzytkownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Komentarze_PozycjaId",
                table: "Komentarze",
                column: "PozycjaId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentarze_UzytkownikId",
                table: "Komentarze",
                column: "UzytkownikId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Komentarze");

            migrationBuilder.DropTable(
                name: "Pozycje");

            migrationBuilder.DropTable(
                name: "Uzytkownicy");
        }
    }
}
