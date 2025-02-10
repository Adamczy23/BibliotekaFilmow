using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotekaFilmow.Migrations
{
    /// <inheritdoc />
    public partial class DodajKolumneNickPoprawiona : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nick",
                table: "Komentarze",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nick",
                table: "Komentarze");
        }
    }

}
