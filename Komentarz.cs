using System;

namespace BibliotekaFilmow
{
    public class Komentarz
    {
        public int Id { get; set; }
        public int PozycjaId { get; set; }
        public string Nick { get; set; } = string.Empty;
        public string Tresc { get; set; } = string.Empty;
        public DateTime DataDodania { get; set; }

        public virtual Pozycja? Pozycja { get; set; }
    }

}
