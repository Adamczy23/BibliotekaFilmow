using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotekaFilmow.Migrations
{
    /// <inheritdoc />
    public partial class UsunKolumneNick : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "Nick",
            table: "Komentarze");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "Nick",
            table: "Komentarze",
            type: "TEXT",
            nullable: true);

        }
    }
}
