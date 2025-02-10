using System.Collections.Generic;

namespace BibliotekaFilmow
{
    public class Uzytkownik
    {
        public int Id { get; set; }
        public string? Nazwa { get; set; }
        public bool CzyAdministrator { get; set; } = false;

        public List<Komentarz> Komentarze { get; set; } = new List<Komentarz>();
    }
}
