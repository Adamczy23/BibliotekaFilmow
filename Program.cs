using System;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BibliotekaFilmow
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BibliotekaContext())
            {
                db.Database.EnsureCreated();
            }

            MenuGlowne();
        }

        static void MenuGlowne()
        {
            bool kontynuuj = true;
            while (kontynuuj)
            {
                Console.Clear();
                Console.WriteLine("=== BIBLIOTEKA FILMÓW I SERIALI ===");
                Console.WriteLine("1. Dodaj pozycję");
                Console.WriteLine("2. Przeglądaj pozycje");
                Console.WriteLine("3. Wyszukaj pozycje");
                Console.WriteLine("4. Usuń Pozycję (podaj tytuł)");
                Console.WriteLine("5. Oznacz jako obejrzane");
                Console.WriteLine("6. Zarządzaj komentarzami");
                Console.WriteLine("7. Importuj pozycje z pliku CSV");
                Console.WriteLine("8. Usuń zaimportowane pozycje z tabeli");
                Console.WriteLine("9. Usuń wszystkie pozycje z aplikacji");
                Console.WriteLine("10. Wyjdź");
                Console.Write("Wybierz opcję: ");

                string? wybor = Console.ReadLine();

                switch (wybor)
                {
                    case "1":
                        Funkcje.DodajPozycje();
                        break;
                    case "2":
                        Funkcje.PrzegladajPozycje();
                        break;
                    case "3":
                        Funkcje.WyszukajPozycje();
                        break;
                    case "4":
                        Funkcje.UsunPozycje();
                        break;
                    case "5":
                        Funkcje.OznaczJakoObejrzane();
                        break;
                    case "6":
                        Funkcje.ZarzadzajKomentarzami();
                        break;
                    case "7":
                        Funkcje.ImportujZCsv();
                        break;
                    case "8":
                        Funkcje.UsunZaimportowanePozycje(); // Wywołaj funkcję do usuwania zaimportowanych pozycji
                        break;
                    case "9":
                        Funkcje.UsunWszystkiePozycje(); // Dodaj wywołanie nowej funkcji do usuwania wszystkich pozycji
                        break;
                    case "10":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór. Naciśnij dowolny klawisz, aby kontynuować.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
