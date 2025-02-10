using Microsoft.EntityFrameworkCore;

namespace BibliotekaFilmow
{
    public class BibliotekaContext : DbContext
    {
        public DbSet<Pozycja> Pozycje { get; set; }
        public DbSet<Komentarz> Komentarze { get; set; }

        // Konfiguracja połączenia z bazą danych
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=biblioteka.db");
        }

        // Konfiguracja relacji
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Komentarz>()
                .HasOne(k => k.Pozycja)
                .WithMany(p => p.Komentarze)
                .HasForeignKey(k => k.PozycjaId);
        }
    }


}
