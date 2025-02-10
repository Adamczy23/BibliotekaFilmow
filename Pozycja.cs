using System.Collections.Generic;

namespace BibliotekaFilmow
{
    public class Pozycja
    {
        public int Id { get; set; }
        public string? Tytul { get; set; }
        public string? Rezyser { get; set; }
        public int RokWydania { get; set; }
        public string? Kategoria { get; set; }
        public string? Gatunek { get; set; }
        public bool Obejrzane { get; set; }

        public virtual ICollection<Komentarz> Komentarze { get; set; } = new List<Komentarz>();
    }


}
