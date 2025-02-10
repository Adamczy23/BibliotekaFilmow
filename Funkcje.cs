using System;
using System.Linq;
using System.Globalization;
using System.Text;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace BibliotekaFilmow
{
    public static class Funkcje
    {
        public static void DodajPozycje()
        {
            Console.Clear();
            Console.WriteLine("=== DODAJ POZYCJĘ ===");
            Console.WriteLine("Wpisz '0' w dowolnym momencie, aby wrócić do menu.");

            string ? tytul;
            do
            {
                Console.Write("Tytuł: ");
                tytul = Console.ReadLine();
                if (tytul == "0") return;

                if (string.IsNullOrWhiteSpace(tytul))
                {
                    Console.WriteLine("Tytuł nie może być pusty.");
                }
            } while (string.IsNullOrWhiteSpace(tytul));

            string? rezyser;
            do
            {
                Console.Write("Reżyser: ");
                rezyser = Console.ReadLine();
                if (rezyser == "0") return;
                if (string.IsNullOrWhiteSpace(rezyser))
                {
                    Console.WriteLine("Reżyser nie może być pusty.");
                }
            } while (string.IsNullOrWhiteSpace(rezyser));

            int rokWydania;
            while (true)
            {
                Console.Write("Rok wydania: ");
                string? input = Console.ReadLine();
                if (input == "0") return;

                if (int.TryParse(input, out rokWydania))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy format roku. Podaj rok jako liczbę całkowitą.");
                }
            }

            string? kategoria;
            do
            {
                Console.Write("Kategoria (Film/Serial): ");
                kategoria = Console.ReadLine();
                if (kategoria == "0") return;

                if (string.IsNullOrWhiteSpace(kategoria))
                {
                    Console.WriteLine("Kategoria nie może być pusty.");
                }
            } while (string.IsNullOrWhiteSpace(kategoria));

            string? gatunek;
            do
            {
                Console.Write("Gatunek: ");
                gatunek = Console.ReadLine();
                if (gatunek == "0") return;

                if (string.IsNullOrWhiteSpace(gatunek))
                {
                    Console.WriteLine("Gatunek nie może być pusty.");
                }
            } while (string.IsNullOrWhiteSpace(gatunek));

            var pozycja = new Pozycja
            {
                Tytul = tytul,
                Rezyser = rezyser,
                RokWydania = rokWydania,
                Kategoria = kategoria,
                Gatunek = gatunek,
                Obejrzane = false
            };

            using (var db = new BibliotekaContext())
            {
                db.Pozycje.Add(pozycja);
                db.SaveChanges();
            }

            Console.WriteLine("Pozycja dodana pomyślnie! Naciśnij dowolny klawisz, aby kontynuować.");
            Console.ReadKey();
        }

        public static void PrzegladajPozycje()
        {
            bool kontynuuj = true;
            while (kontynuuj)
            {
                Console.Clear();
                Console.WriteLine("=== LISTA POZYCJI ===");
                Console.WriteLine("1. Sortuj według tytułu (A-Z)");
                Console.WriteLine("2. Sortuj według tytułu (Z-A)");
                Console.WriteLine("3. Sortuj według reżysera (A-Z)");
                Console.WriteLine("4. Sortuj według reżysera (Z-A)");
                Console.WriteLine("5. Sortuj według roku produkcji (od najstarszej)");
                Console.WriteLine("6. Sortuj według roku produkcji (od najmłodszej)");
                Console.WriteLine("7. Przeglądaj według kategorii (film/serial)");
                Console.WriteLine("0. Powrót do menu");
                Console.Write("Wybierz opcję: ");

                string? wybor = Console.ReadLine();
                List<Pozycja> pozycje;

                using (var db = new BibliotekaContext())
                {
                    switch (wybor)
                    {
                        case "1":
                            pozycje = db.Pozycje.OrderBy(p => p.Tytul).ToList();
                            break;
                        case "2":
                            pozycje = db.Pozycje.OrderByDescending(p => p.Tytul).ToList();
                            break;
                        case "3":
                            pozycje = db.Pozycje.OrderBy(p => p.Rezyser).ToList();
                            break;
                        case "4":
                            pozycje = db.Pozycje.OrderByDescending(p => p.Rezyser).ToList();
                            break;
                        case "5":
                            pozycje = db.Pozycje.OrderBy(p => p.RokWydania).ToList();
                            break;
                        case "6":
                            pozycje = db.Pozycje.OrderByDescending(p => p.RokWydania).ToList();
                            break;
                        case "7":
                            Console.WriteLine("Wybierz kategorię:");
                            Console.WriteLine("1. Film");
                            Console.WriteLine("2. Serial");
                            Console.Write("Wybierz opcję: ");
                            string? kategoriaWybor = Console.ReadLine();
                            string? kategoria = kategoriaWybor switch
                            {
                                "1" => "Film",
                                "2" => "Serial",
                                _ => null
                            };

                            if (string.IsNullOrWhiteSpace(kategoria))
                            {
                                Console.WriteLine("Nieprawidłowy wybór.");
                                Console.ReadKey();
                                continue;
                            }

                            pozycje = db.Pozycje.Where(p => p.Kategoria != null && p.Kategoria.ToLower() == kategoria.ToLower()).ToList();
                            break;
                        case "0":
                            kontynuuj = false;
                            continue;
                        default:
                            Console.WriteLine("Nieprawidłowy wybór.");
                            Console.ReadKey();
                            continue;
                    }

                    if (pozycje.Count == 0)
                    {
                        Console.WriteLine("Brak pozycji w bibliotece.");
                    }
                    else
                    {
                        // Wyświetlanie listy pozycji z numerami porządkowymi
                        WyswietlListePozycjiZNumerami(pozycje);

                        // Pytanie, czy użytkownik chce oznaczyć którąś z pozycji jako obejrzaną
                        Console.WriteLine("\nCzy chcesz oznaczyć którąś z pozycji jako obejrzaną?");
                        Console.WriteLine("1. Tak");
                        Console.WriteLine("0. Nie");
                        Console.Write("Wybierz opcję: ");
                        string? wyborOznaczenia = Console.ReadLine();

                        if (wyborOznaczenia == "1")
                        {
                            OznaczPozycjeJakoObejrzane(pozycje, db);
                        }
                    }

                    Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                    Console.ReadKey();
                }
            }
        }




        private static void OznaczPozycjeJakoObejrzane(List<Pozycja> wszystkiePozycje, BibliotekaContext db)
        {
            Console.Write("Podaj część tytułu pozycji, którą chcesz oznaczyć jako obejrzaną/nieobejrzaną: ");
            string? czescTytulu = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(czescTytulu))
            {
                Console.WriteLine("Tytuł nie może być pusty.");
                Console.ReadKey();
                return;
            }

            var pasujacePozycje = wszystkiePozycje.Where(p => p.Tytul != null && p.Tytul.ToLower().Contains(czescTytulu.ToLower())).ToList();

            if (pasujacePozycje.Count == 0)
            {
                Console.WriteLine("Nie znaleziono pozycji pasujących do podanego tytułu.");
                Console.ReadKey();
                return;
            }
            else if (pasujacePozycje.Count == 1)
            {
                var wybranaPozycja = pasujacePozycje.First();
                ZmienStatusObejrzenia(wybranaPozycja, db);
            }
            else
            {
                Console.WriteLine("Znaleziono kilka pozycji:");
                for (int i = 0; i < pasujacePozycje.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {pasujacePozycje[i].Tytul}");
                }

                Console.Write("Wybierz numer pozycji z listy: ");
                int numerPozycji;
                while (!int.TryParse(Console.ReadLine(), out numerPozycji) || numerPozycji < 1 || numerPozycji > pasujacePozycje.Count)
                {
                    Console.Write("Nieprawidłowy wybór. Podaj numer z listy: ");
                }

                var wybranaPozycja = pasujacePozycje[numerPozycji - 1];
                ZmienStatusObejrzenia(wybranaPozycja, db);
            }
        }




        private static void ZmienStatusObejrzenia(Pozycja wybranaPozycja, BibliotekaContext db)
        {
            Console.WriteLine($"Czy chcesz oznaczyć pozycję '{wybranaPozycja.Tytul}' jako:");
            Console.WriteLine("1. Obejrzane");
            Console.WriteLine("2. Nieobejrzane");
            Console.Write("Wybierz opcję: ");
            string? obejrzaneWybor = Console.ReadLine();

            if (obejrzaneWybor == "1")
            {
                wybranaPozycja.Obejrzane = true;
                db.SaveChanges();
                Console.WriteLine("Pozycja została oznaczona jako obejrzana.");
            }
            else if (obejrzaneWybor == "2")
            {
                wybranaPozycja.Obejrzane = false;
                db.SaveChanges();
                Console.WriteLine("Pozycja została oznaczona jako nieobejrzana.");
            }
            else
            {
                Console.WriteLine("Nieprawidłowy wybór.");
            }
        }


        private static void WyswietlListePozycjiZNumerami(List<Pozycja> pozycje)
        {
            int padding = 5; // Zwiększenie odstępów między kolumnami
            int minWidth = 5;  // Minimalna szerokość kolumny dla numeru
            int maxNumerLength = Math.Max("Nr".Length + padding, minWidth);
            int maxTytulLength = Math.Max("Tytuł".Length + padding, minWidth);
            int maxRezyserLength = Math.Max("Reżyser".Length + padding, minWidth);
            int maxRokLength = Math.Max("Rok".Length + padding, minWidth);
            int maxKategoriaLength = Math.Max("Kategoria".Length + padding, minWidth);
            int maxGatunekLength = Math.Max("Gatunek".Length + padding, minWidth);
            int maxObejrzaneLength = Math.Max("Obejrzane".Length + padding, minWidth);

            foreach (var p in pozycje)
            {
                maxTytulLength = Math.Max(maxTytulLength, (p.Tytul ?? "Brak tytułu").Length + padding);
                maxRezyserLength = Math.Max(maxRezyserLength, (p.Rezyser ?? "Brak reżysera").Length + padding);
                maxRokLength = Math.Max(maxRokLength, p.RokWydania.ToString().Length + padding);
                maxKategoriaLength = Math.Max(maxKategoriaLength, (p.Kategoria ?? "Brak").Length + padding);
                maxGatunekLength = Math.Max(maxGatunekLength, (p.Gatunek ?? "Brak gatunku").Length + padding);
                maxObejrzaneLength = Math.Max(maxObejrzaneLength, (p.Obejrzane ? "Tak" : "Nie").Length + padding);
            }

            // Nagłówek tabeli z numerami porządkowymi
            Console.WriteLine(
                "{0,-" + maxNumerLength + "} {1,-" + maxTytulLength + "} {2,-" + maxRezyserLength + "} {3,-" + maxRokLength + "} {4,-" + maxKategoriaLength + "} {5,-" + maxGatunekLength + "} {6,-" + maxObejrzaneLength + "}",
                "Nr", "Tytuł", "Reżyser", "Rok", "Kategoria", "Gatunek", "Obejrzane");

            Console.WriteLine(new string('-', maxNumerLength + maxTytulLength + maxRezyserLength + maxRokLength + maxKategoriaLength + maxGatunekLength + maxObejrzaneLength + padding * 6));

            for (int i = 0; i < pozycje.Count; i++)
            {
                var p = pozycje[i];
                string numer = (i + 1).ToString();
                string tytul = WrapText(p.Tytul ?? "Brak tytułu", maxTytulLength);
                string rezyser = WrapText(p.Rezyser ?? "Brak reżysera", maxRezyserLength);
                string rok = p.RokWydania.ToString();
                string kategoria = WrapText(p.Kategoria ?? "Brak", maxKategoriaLength);
                string gatunek = WrapText(p.Gatunek ?? "Brak gatunku", maxGatunekLength);
                string obejrzane = p.Obejrzane ? "Tak" : "Nie";

                Console.WriteLine(
                    "{0,-" + maxNumerLength + "} {1,-" + maxTytulLength + "} {2,-" + maxRezyserLength + "} {3,-" + maxRokLength + "} {4,-" + maxKategoriaLength + "} {5,-" + maxGatunekLength + "} {6,-" + maxObejrzaneLength + "}",
                    numer, tytul, rezyser, rok, kategoria, gatunek, obejrzane);
            }
        }



        private static void WyswietlListePozycjiBezID(List<Pozycja> pozycje)
        {
            int padding = 5; // Zwiększenie odstępów między kolumnami
            int minWidth = 10; // Minimalna szerokość kolumny

            // Obliczenie maksymalnej szerokości każdej kolumny
            int maxTytulLength = Math.Max("Tytuł".Length + padding, minWidth);
            int maxRezyserLength = Math.Max("Reżyser".Length + padding, minWidth);
            int maxRokLength = Math.Max("Rok".Length + padding, minWidth);
            int maxKategoriaLength = Math.Max("Kategoria".Length + padding, minWidth);
            int maxGatunekLength = Math.Max("Gatunek".Length + padding, minWidth);
            int maxObejrzaneLength = Math.Max("Obejrzane".Length + padding, minWidth);

            foreach (var p in pozycje)
            {
                maxTytulLength = Math.Max(maxTytulLength, (p.Tytul ?? "Brak tytułu").Length + padding);
                maxRezyserLength = Math.Max(maxRezyserLength, (p.Rezyser ?? "Brak reżysera").Length + padding);
                maxRokLength = Math.Max(maxRokLength, p.RokWydania.ToString().Length + padding);
                maxKategoriaLength = Math.Max(maxKategoriaLength, (p.Kategoria ?? "Brak").Length + padding);
                maxGatunekLength = Math.Max(maxGatunekLength, (p.Gatunek ?? "Brak gatunku").Length + padding);
                maxObejrzaneLength = Math.Max(maxObejrzaneLength, (p.Obejrzane ? "Tak" : "Nie").Length + padding);
            }

            // Nagłówek tabeli bez kolumny ID
            Console.WriteLine(
                "{0,-" + maxTytulLength + "} {1,-" + maxRezyserLength + "} {2,-" + maxRokLength + "} {3,-" + maxKategoriaLength + "} {4,-" + maxGatunekLength + "} {5,-" + maxObejrzaneLength + "}",
                "Tytuł", "Reżyser", "Rok", "Kategoria", "Gatunek", "Obejrzane");

            Console.WriteLine(new string('-', maxTytulLength + maxRezyserLength + maxRokLength + maxKategoriaLength + maxGatunekLength + maxObejrzaneLength + padding * 5));

            foreach (var p in pozycje)
            {
                string tytul = WrapText(p.Tytul ?? "Brak tytułu", maxTytulLength);
                string rezyser = WrapText(p.Rezyser ?? "Brak reżysera", maxRezyserLength);
                string rok = p.RokWydania.ToString();
                string kategoria = WrapText(p.Kategoria ?? "Brak", maxKategoriaLength);
                string gatunek = WrapText(p.Gatunek ?? "Brak gatunku", maxGatunekLength);
                string obejrzane = p.Obejrzane ? "Tak" : "Nie";

                Console.WriteLine(
                    "{0,-" + maxTytulLength + "} {1,-" + maxRezyserLength + "} {2,-" + maxRokLength + "} {3,-" + maxKategoriaLength + "} {4,-" + maxGatunekLength + "} {5,-" + maxObejrzaneLength + "}",
                    tytul, rezyser, rok, kategoria, gatunek, obejrzane);
            }
        }

        







        // Jedna funkcja zawijająca tekst
        public static string WrapText(string text, int maxWidth)
        {
            if (text.Length <= maxWidth)
                return text;

            StringBuilder wrappedText = new StringBuilder();

            int currentWidth = 0;
            foreach (char c in text)
            {
                if (currentWidth >= maxWidth && c == ' ')
                {
                    wrappedText.Append("\n");
                    currentWidth = 0;
                }

                wrappedText.Append(c);
                currentWidth++;
            }

            return wrappedText.ToString();
        }



















        public static void WyszukajPozycje()
        {
            Console.Clear();
            Console.WriteLine("=== WYSZUKAJ POZYCJE ===");
            Console.WriteLine("Wpisz '0' w dowolnym momencie, aby wrócić do menu.");
            Console.WriteLine("Wyszukaj po:");
            Console.WriteLine("1. Tytule");
            Console.WriteLine("2. Reżyserze");
            Console.WriteLine("3. Gatunku");
            Console.WriteLine("4. Roku wydania");
            Console.WriteLine("5. Obejrzane/nieobejrzane");
            Console.Write("Wybierz opcję: ");

            string? wybor = Console.ReadLine();
            if (wybor == "0") return;

            string prompt = "Podaj wartość wyszukiwania: ";
            if (wybor == "4")
            {
                prompt = "Podaj rok wydania: ";
            }
            else if (wybor == "5")
            {
                prompt = "Obejrzane (Tak/Nie): ";
            }

            Console.Write(prompt);
            string? wartosc = Console.ReadLine();
            if (wartosc == "0") return;

            if (string.IsNullOrWhiteSpace(wartosc))
            {
                Console.WriteLine("Nie podano wartości do wyszukania.");
                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                Console.ReadKey();
                return;
            }

            wartosc = wartosc.ToLower();

            using (var db = new BibliotekaContext())
            {
                IQueryable<Pozycja> pozycje = db.Pozycje;

                switch (wybor)
                {
                    case "1":
                        pozycje = pozycje.Where(p => p.Tytul != null && p.Tytul.ToLower().Contains(wartosc));
                        break;
                    case "2":
                        pozycje = pozycje.Where(p => p.Rezyser != null && p.Rezyser.ToLower().Contains(wartosc));
                        break;
                    case "3":
                        pozycje = pozycje.Where(p => p.Gatunek != null && p.Gatunek.ToLower().Contains(wartosc));
                        break;
                    case "4":
                        if (int.TryParse(wartosc, out int rokWydania))
                        {
                            pozycje = pozycje.Where(p => p.RokWydania == rokWydania);
                        }
                        else
                        {
                            Console.WriteLine("Niepoprawny format roku.");
                            return;
                        }
                        break;
                    case "5":
                        bool obejrzane = wartosc == "tak";
                        pozycje = pozycje.Where(p => p.Obejrzane == obejrzane);
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór.");
                        return;
                }

                var lista = pozycje.ToList();

                if (lista.Count == 0)
                {
                    Console.WriteLine("Brak wyników.");
                }
                else
                {
                    int maxLpLength = "Lp.".Length;
                    int maxTytulLength = "Tytuł".Length;
                    int maxRezyserLength = "Reżyser".Length;
                    int maxRokLength = "Rok".Length;
                    int maxKategoriaLength = "Kategoria".Length;
                    int maxGatunekLength = "Gatunek".Length;
                    int maxObejrzaneLength = "Obejrzane".Length;

                    foreach (var p in lista)
                    {
                        maxTytulLength = Math.Max(maxTytulLength, (p.Tytul ?? "Brak tytułu").Length);
                        maxRezyserLength = Math.Max(maxRezyserLength, (p.Rezyser ?? "Brak reżysera").Length);
                        maxRokLength = Math.Max(maxRokLength, p.RokWydania.ToString().Length);
                        maxKategoriaLength = Math.Max(maxKategoriaLength, (p.Kategoria ?? "Brak").Length);
                        maxGatunekLength = Math.Max(maxGatunekLength, (p.Gatunek ?? "Brak gatunku").Length);
                        maxObejrzaneLength = Math.Max(maxObejrzaneLength, (p.Obejrzane ? "Tak" : "Nie").Length);
                    }

                    // Dodajmy dodatkowe odstępy
                    int padding = 4;

                    Console.WriteLine(
                        "{0,-" + (maxLpLength + padding) + "} {1,-" + (maxTytulLength + padding) + "} {2,-" + (maxRezyserLength + padding) + "} {3,-" + (maxRokLength + padding) + "} {4,-" + (maxKategoriaLength + padding) + "} {5,-" + (maxGatunekLength + padding) + "} {6,-" + (maxObejrzaneLength + padding) + "}",
                        "Lp.", "Tytuł", "Reżyser", "Rok", "Kategoria", "Gatunek", "Obejrzane");

                    Console.WriteLine(new string('-', maxLpLength + maxTytulLength + maxRezyserLength + maxRokLength + maxKategoriaLength + maxGatunekLength + maxObejrzaneLength + padding * 6));

                    int liczbaPorzadkowa = 1;
                    foreach (var p in lista)
                    {
                        string tytul = p.Tytul ?? "Brak tytułu";
                        string rezyser = p.Rezyser ?? "Brak reżysera";
                        string gatunek = p.Gatunek ?? "Brak gatunku";

                        Console.WriteLine(
                            "{0,-" + (maxLpLength + padding) + "} {1,-" + (maxTytulLength + padding) + "} {2,-" + (maxRezyserLength + padding) + "} {3,-" + (maxRokLength + padding) + "} {4,-" + (maxKategoriaLength + padding) + "} {5,-" + (maxGatunekLength + padding) + "} {6,-" + (maxObejrzaneLength + padding) + "}",
                            liczbaPorzadkowa++, tytul, rezyser, p.RokWydania, p.Kategoria ?? "Brak", gatunek, (p.Obejrzane ? "Tak" : "Nie"));
                    }
                }
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
            Console.ReadKey();
        }














        public static void UsunPozycje()
        {
            Console.Clear();
            Console.WriteLine("=== USUŃ POZYCJĘ ===");
            Console.WriteLine("Podaj fragment tytułu pozycji, którą chcesz usunąć:");
            Console.Write("Fragment tytułu: ");
            string? fragmentTytulu = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(fragmentTytulu))
            {
                Console.WriteLine("Nie podano fragmentu tytułu. Naciśnij dowolny klawisz, aby kontynuować.");
                Console.ReadKey();
                return;
            }

            using (var db = new BibliotekaContext())
            {
                var pozycje = db.Pozycje
                    .Where(p => p.Tytul != null && p.Tytul.ToLower().Contains(fragmentTytulu.ToLower()))
                    .ToList();

                if (pozycje.Count == 0)
                {
                    Console.WriteLine($"Pozycja zawierająca fragment tytułu '{fragmentTytulu}' nie została znaleziona.");
                }
                else if (pozycje.Count == 1)
                {
                    var pozycja = pozycje.First();
                    db.Pozycje.Remove(pozycja);
                    db.SaveChanges();
                    Console.WriteLine($"Pozycja o tytule '{pozycja.Tytul}' została pomyślnie usunięta.");
                }
                else
                {
                    Console.WriteLine("Znaleziono więcej niż jedną pozycję. Wybierz dokładny tytuł do usunięcia:");
                    for (int i = 0; i < pozycje.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {pozycje[i].Tytul}");
                    }

                    Console.Write("Wybierz numer pozycji do usunięcia: ");
                    if (int.TryParse(Console.ReadLine(), out int wybor) && wybor > 0 && wybor <= pozycje.Count)
                    {
                        var pozycja = pozycje[wybor - 1];
                        db.Pozycje.Remove(pozycja);
                        db.SaveChanges();
                        Console.WriteLine($"Pozycja o tytule '{pozycja.Tytul}' została pomyślnie usunięta.");
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy wybór. Naciśnij dowolny klawisz, aby kontynuować.");
                        Console.ReadKey();
                        return;
                    }
                }
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
            Console.ReadKey();
        }




        public static void OznaczJakoObejrzane()
        {
            Console.Clear();
            Console.WriteLine("=== OZNACZ JAKO OBEJRZANE ===");

            Console.Write("Podaj część tytułu pozycji, którą chcesz oznaczyć jako obejrzaną: ");
            string? czescTytulu = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(czescTytulu))
            {
                Console.WriteLine("Nie podano tytułu.");
                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                Console.ReadKey();
                return;
            }

            using (var db = new BibliotekaContext())
            {
                var pasujacePozycje = db.Pozycje
                    .Where(p => !p.Obejrzane && p.Tytul != null && p.Tytul.ToLower().Contains(czescTytulu.ToLower()))
                    .ToList();

                if (pasujacePozycje.Count == 0)
                {
                    Console.WriteLine("Nie znaleziono nieobejrzanych pozycji pasujących do podanego tytułu.");
                }
                else if (pasujacePozycje.Count == 1)
                {
                    var wybranaPozycja = pasujacePozycje.First();
                    OznaczWybranaPozycjeJakoObejrzana(wybranaPozycja, db);
                }
                else
                {
                    Console.WriteLine("Znaleziono kilka nieobejrzanych pozycji:");
                    for (int i = 0; i < pasujacePozycje.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {pasujacePozycje[i].Tytul}");
                    }

                    Console.Write("Wybierz numer pozycji do oznaczenia jako obejrzaną: ");
                    int numerWybieranejPozycji;
                    while (!int.TryParse(Console.ReadLine(), out numerWybieranejPozycji) || numerWybieranejPozycji < 1 || numerWybieranejPozycji > pasujacePozycje.Count)
                    {
                        Console.Write("Nieprawidłowy wybór. Podaj poprawny numer: ");
                    }

                    var wybranaPozycja = pasujacePozycje[numerWybieranejPozycji - 1];
                    OznaczWybranaPozycjeJakoObejrzana(wybranaPozycja, db);
                }
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
            Console.ReadKey();
        }




        private static void OznaczWybranaPozycjeJakoObejrzana(Pozycja wybranaPozycja, BibliotekaContext db)
        {
            wybranaPozycja.Obejrzane = true;
            db.SaveChanges();
            Console.WriteLine($"Pozycja '{wybranaPozycja.Tytul}' została oznaczona jako obejrzana.");
        }



        public static void SortujPozycje()
        {
            Console.Clear();
            Console.WriteLine("=== SORTUJ POZYCJE ===");
            Console.WriteLine("1. Sortuj według tytułu");
            Console.WriteLine("2. Sortuj według reżysera");
            Console.WriteLine("3. Sortuj według roku wydania");
            Console.WriteLine("0. Powrót do menu");
            Console.Write("Wybierz opcję: ");

            string? wybor = Console.ReadLine();
            if (wybor == "0") return;

            using (var db = new BibliotekaContext())
            {
                IQueryable<Pozycja> pozycje = db.Pozycje;

                switch (wybor)
                {
                    case "1":
                        pozycje = pozycje.OrderBy(p => p.Tytul);
                        break;
                    case "2":
                        pozycje = pozycje.OrderBy(p => p.Rezyser);
                        break;
                    case "3":
                        pozycje = pozycje.OrderBy(p => p.RokWydania);
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór.");
                        return;
                }

                var lista = pozycje.ToList();

                if (lista.Count == 0)
                {
                    Console.WriteLine("Brak wyników.");
                }
                else
                {
                    // Obliczenie maksymalnej szerokości każdej kolumny z dodanym odstępem
                    int maxIdLength = "ID".Length;
                    int maxTytulLength = "Tytuł".Length;
                    int maxRezyserLength = "Reżyser".Length;
                    int maxRokLength = "Rok".Length;
                    int maxKategoriaLength = "Kategoria".Length;
                    int maxGatunekLength = "Gatunek".Length;
                    int maxObejrzaneLength = "Obejrzane".Length;
                    int padding = 2;

                    foreach (var p in lista)
                    {
                        maxIdLength = Math.Max(maxIdLength, p.Id.ToString().Length);
                        maxTytulLength = Math.Max(maxTytulLength, (p.Tytul ?? "Brak tytułu").Length);
                        maxRezyserLength = Math.Max(maxRezyserLength, (p.Rezyser ?? "Brak reżysera").Length);
                        maxRokLength = Math.Max(maxRokLength, p.RokWydania.ToString().Length);
                        maxKategoriaLength = Math.Max(maxKategoriaLength, (p.Kategoria ?? "Brak").Length);
                        maxGatunekLength = Math.Max(maxGatunekLength, (p.Gatunek ?? "Brak gatunku").Length);
                        maxObejrzaneLength = Math.Max(maxObejrzaneLength, (p.Obejrzane ? "Tak" : "Nie").Length);
                    }

                    // Nagłówek tabeli z formatowaniem
                    Console.WriteLine(
                        "{0,-" + (maxIdLength + padding) + "} {1,-" + (maxTytulLength + padding) + "} {2,-" + (maxRezyserLength + padding) + "} {3,-" + (maxRokLength + padding) + "} {4,-" + (maxKategoriaLength + padding) + "} {5,-" + (maxGatunekLength + padding) + "} {6,-" + (maxObejrzaneLength + padding) + "}",
                        "ID", "Tytuł", "Reżyser", "Rok", "Kategoria", "Gatunek", "Obejrzane");

                    Console.WriteLine(new string('-', maxIdLength + maxTytulLength + maxRezyserLength + maxRokLength + maxKategoriaLength + maxGatunekLength + maxObejrzaneLength + (padding * 7) + 6));

                    foreach (var p in lista)
                    {
                        string tytul = p.Tytul ?? "Brak tytułu";
                        string rezyser = p.Rezyser ?? "Brak reżysera";
                        string gatunek = p.Gatunek ?? "Brak gatunku";

                        Console.WriteLine(
                            "{0,-" + (maxIdLength + padding) + "} {1,-" + (maxTytulLength + padding) + "} {2,-" + (maxRezyserLength + padding) + "} {3,-" + (maxRokLength + padding) + "} {4,-" + (maxKategoriaLength + padding) + "} {5,-" + (maxGatunekLength + padding) + "} {6,-" + (maxObejrzaneLength + padding) + "}",
                            p.Id, tytul, rezyser, p.RokWydania, p.Kategoria ?? "Brak", gatunek, (p.Obejrzane ? "Tak" : "Nie"));
                    }
                }
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
            Console.ReadKey();
        }






        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new System.Text.StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }


        public static void ZarzadzajKomentarzami()
        {
            Console.Clear();
            Console.WriteLine("=== ZARZĄDZAJ KOMENTARZAMI ===");
            Console.WriteLine("1. Dodaj komentarz");
            Console.WriteLine("2. Wyświetl komentarze");
            Console.WriteLine("3. Usuń komentarz (administrator)");
            Console.WriteLine("4. Edytuj swój komentarz");
            Console.WriteLine("0. Powrót do menu");
            Console.Write("Wybierz opcję: ");

            string? wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    DodajKomentarz();
                    break;
                case "2":
                    WyswietlKomentarze();
                    break;
                case "3":
                    UsunKomentarz();
                    break;
                case "4":
                    EdytujKomentarz();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Nieprawidłowy wybór.");
                    Console.ReadKey();
                    break;
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
            Console.ReadKey();
        }




        private static void WyswietlKomentarze()
        {
            Console.Clear();
            Console.WriteLine("=== WYŚWIETL KOMENTARZE ===");
            Console.Write("Podaj część tytułu pozycji, której komentarze chcesz wyświetlić: ");
            string? czescTytulu = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(czescTytulu))
            {
                Console.WriteLine("Tytuł nie może być pusty.");
                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                Console.ReadKey();
                return;
            }

            using (var db = new BibliotekaContext())
            {
                var pasujacePozycje = db.Pozycje.Include(p => p.Komentarze)
                                          .Where(p => p.Tytul != null && p.Tytul.ToLower().Contains(czescTytulu.ToLower()))
                                          .ToList();

                if (pasujacePozycje.Count == 0)
                {
                    Console.WriteLine("Nie znaleziono żadnych pozycji pasujących do podanego tytułu.");
                    Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Znaleziono następujące pozycje:");
                for (int i = 0; i < pasujacePozycje.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {pasujacePozycje[i].Tytul}");
                }

                Console.Write("Wybierz numer pozycji, której komentarze chcesz wyświetlić: ");
                int wybor;
                while (!int.TryParse(Console.ReadLine(), out wybor) || wybor < 1 || wybor > pasujacePozycje.Count)
                {
                    Console.Write("Nieprawidłowy wybór. Wybierz numer z listy: ");
                }

                var wybranaPozycja = pasujacePozycje[wybor - 1];

                var komentarze = db.Komentarze
                    .Where(k => k.PozycjaId == wybranaPozycja.Id)
                    .ToList();

                if (komentarze.Count == 0)
                {
                    Console.WriteLine("Brak komentarzy dla tej pozycji.");
                }
                else
                {
                    foreach (var k in komentarze)
                    {
                        string uzytkownikNazwa = k.Nick ?? "Nieznany użytkownik";
                        Console.WriteLine($"[{k.DataDodania}] {uzytkownikNazwa}: {k.Tresc}");
                    }
                }
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
            Console.ReadKey();
        }

    

        



        public static void DodajKomentarz()
        {
            Console.Clear();
            Console.WriteLine("=== DODAJ KOMENTARZ ===");
            Console.Write("Podaj część tytułu pozycji, do której chcesz dodać komentarz: ");
            string? czescTytulu = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(czescTytulu))
            {
                Console.WriteLine("Tytuł nie może być pusty.");
                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                Console.ReadKey();
                return;
            }

            using (var db = new BibliotekaContext())
            {
                var pasujacePozycje = db.Pozycje.Include(p => p.Komentarze)
                                          .Where(p => p.Tytul != null && p.Tytul.ToLower().Contains(czescTytulu.ToLower()))
                                          .ToList();

                if (pasujacePozycje.Count == 0)
                {
                    Console.WriteLine("Nie znaleziono żadnych pozycji pasujących do podanego tytułu.");
                    Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Znaleziono następujące pozycje:");
                for (int i = 0; i < pasujacePozycje.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {pasujacePozycje[i].Tytul}");
                }

                Console.Write("Wybierz numer pozycji, do której chcesz dodać komentarz: ");
                int wybor;
                while (!int.TryParse(Console.ReadLine(), out wybor) || wybor < 1 || wybor > pasujacePozycje.Count)
                {
                    Console.Write("Nieprawidłowy wybór. Wybierz numer z listy: ");
                }

                var wybranaPozycja = pasujacePozycje[wybor - 1];

                Console.Write("Podaj swój nick: ");
                string? nick = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nick))
                {
                    Console.WriteLine("Nick nie może być pusty.");
                    Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Podaj treść komentarza: ");
                string? tresc = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(tresc))
                {
                    Console.WriteLine("Treść komentarza nie może być pusta.");
                    Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                    Console.ReadKey();
                    return;
                }

                var komentarz = new Komentarz
                {
                    PozycjaId = wybranaPozycja.Id,
                    Nick = nick,
                    Tresc = tresc,
                    DataDodania = DateTime.Now
                };

                db.Komentarze.Add(komentarz);
                db.SaveChanges();

                Console.WriteLine("Komentarz został pomyślnie dodany.");
                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                Console.ReadKey();
            }
        }





        private static void UsunKomentarz()
        {
            Console.Clear();
            Console.WriteLine("=== USUŃ KOMENTARZ ===");
            Console.Write("Podaj swój nick: ");
            string? nick = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nick))
            {
                Console.WriteLine("Nick nie może być pusty.");
                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                Console.ReadKey();
                return;
            }

            using (var db = new BibliotekaContext())
            {
                var komentarze = db.Komentarze.Include(k => k.Pozycja)
                                              .Where(k => k.Nick != null && k.Nick.ToLower() == nick.ToLower())
                                              .ToList();

                if (komentarze.Count == 0)
                {
                    Console.WriteLine("Brak komentarzy dla podanego nicku.");
                    Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Twoje komentarze:");
                for (int i = 0; i < komentarze.Count; i++)
                {
                    var pozycja = komentarze[i].Pozycja;
                    var tytul = pozycja?.Tytul ?? "Nieznany tytuł";
                    var rezyser = pozycja?.Rezyser ?? "Nieznany reżyser";
                    var gatunek = pozycja?.Gatunek ?? "Nieznany gatunek";
                    var rokWydania = pozycja?.RokWydania.ToString() ?? "Nieznany rok";
                    var kategoria = pozycja?.Kategoria ?? "Nieznana kategoria";

                    Console.WriteLine($"{i + 1}. {tytul} (Reżyser: {rezyser}, Gatunek: {gatunek}, Rok: {rokWydania}, Kategoria: {kategoria}) - {komentarze[i].Tresc} (ID: {komentarze[i].Id})");
                }

                Console.Write("Podaj numer komentarza do usunięcia: ");
                int komentarzWybor;
                while (!int.TryParse(Console.ReadLine(), out komentarzWybor) || komentarzWybor < 1 || komentarzWybor > komentarze.Count)
                {
                    Console.Write("Nieprawidłowy wybór. Wybierz numer z listy: ");
                }

                var wybranyKomentarz = komentarze[komentarzWybor - 1];

                db.Komentarze.Remove(wybranyKomentarz);
                db.SaveChanges();
                Console.WriteLine("Komentarz usunięty.");
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
            Console.ReadKey();
        }





        private static void EdytujKomentarz()
        {
            Console.Clear();
            Console.WriteLine("=== EDYTUJ KOMENTARZ ===");
            Console.Write("Podaj swój nick: ");
            string? nick = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nick))
            {
                Console.WriteLine("Nick nie może być pusty.");
                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                Console.ReadKey();
                return;
            }

            using (var db = new BibliotekaContext())
            {
                var komentarze = db.Komentarze.Include(k => k.Pozycja)
                                              .Where(k => k.Nick != null && k.Nick.ToLower() == nick.ToLower())
                                              .ToList();

                if (komentarze.Count == 0)
                {
                    Console.WriteLine("Brak komentarzy dla podanego nicku.");
                    Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Twoje komentarze:");
                for (int i = 0; i < komentarze.Count; i++)
                {
                    var pozycja = komentarze[i].Pozycja;
                    var tytul = pozycja != null ? pozycja.Tytul : "Nieznana pozycja";
                    Console.WriteLine($"{i + 1}. {tytul}: {komentarze[i].Tresc} (ID: {komentarze[i].Id})");
                }

                Console.Write("Podaj numer komentarza do edycji: ");
                int komentarzWybor;
                while (!int.TryParse(Console.ReadLine(), out komentarzWybor) || komentarzWybor < 1 || komentarzWybor > komentarze.Count)
                {
                    Console.Write("Nieprawidłowy wybór. Wybierz numer z listy: ");
                }

                var wybranyKomentarz = komentarze[komentarzWybor - 1];

                Console.Write("Nowa treść komentarza: ");
                string? nowaTresc = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(nowaTresc))
                {
                    Console.Write("Treść komentarza nie może być pusta. Wprowadź nową treść: ");
                    nowaTresc = Console.ReadLine();
                }

                wybranyKomentarz.Tresc = nowaTresc;
                db.SaveChanges();

                Console.WriteLine("Komentarz zaktualizowany.");
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
            Console.ReadKey();
        }




        public static void ImportujZCsv()
        {
            Console.Clear();
            Console.WriteLine("=== IMPORTUJ POZYCJE Z PLIKU CSV ===\n");
            Console.WriteLine("Wpisz '0' w dowolnym momencie, aby wrócić do menu.\n");

            // Wyświetl instrukcję importowania pliku CSV
            Console.WriteLine("Instrukcje dotyczące importowania pliku CSV:");
            Console.WriteLine("1. Upewnij się, że plik CSV jest poprawnie sformatowany.");
            Console.WriteLine("   - Plik powinien zawierać nagłówki kolumn w pierwszym wierszu.");
            Console.WriteLine("   - Wymagane nagłówki to: Tytul, Rezyser, RokWydania, Kategoria, Gatunek, Obejrzane.");
            Console.WriteLine("2. Upewnij się, że plik jest zapisany w kodowaniu UTF-8, aby obsługiwać polskie znaki.");
            Console.WriteLine("3. Umieść plik w łatwo dostępnym miejscu, np. w folderze projektu.");
            Console.WriteLine("4. Podaj pełną ścieżkę do pliku lub jego nazwę, jeśli znajduje się w folderze projektu.");
            Console.WriteLine("   Przykład: pozycje.csv\n");

            Console.Write("Podaj ścieżkę do pliku CSV: ");
            string? sciezka = Console.ReadLine();
            if (sciezka == "0") return;

            if (string.IsNullOrWhiteSpace(sciezka) || !File.Exists(sciezka))
            {
                Console.WriteLine("Plik nie został znaleziony. Upewnij się, że ścieżka jest poprawna.");
                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
                Console.ReadKey();
                return;
            }

            try
            {
                using (var reader = new StreamReader(sciezka))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<PozycjaMap>();

                    var pozycje = csv.GetRecords<Pozycja>().ToList();

                    using (var db = new BibliotekaContext())
                    {
                        db.Pozycje.AddRange(pozycje);
                        db.SaveChanges();
                    }

                    Console.WriteLine($"Pomyślnie zaimportowano {pozycje.Count} pozycji.");
                }

                // Usunięcie pliku CSV po zaimportowaniu
                File.Delete(sciezka);
                Console.WriteLine($"Plik {sciezka} został usunięty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas importowania: {ex.Message}");
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
            Console.ReadKey();
        }

        public static void UsunZaimportowanePozycje()
        {
            Console.Clear();
            Console.WriteLine("=== USUŃ ZAIMPORTOWANE POZYCJE Z TABELI ===\n");

            try
            {
                using (var db = new BibliotekaContext())
                {
                    // Usuń wszystkie pozycje z tabeli
                    db.Pozycje.RemoveRange(db.Pozycje);
                    db.SaveChanges();
                }

                Console.WriteLine("Pomyślnie usunięto wszystkie zaimportowane pozycje.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas usuwania: {ex.Message}");
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
            Console.ReadKey();
        }

        public static void UsunWszystkiePozycje()
        {
            Console.Clear();
            Console.WriteLine("=== USUŃ WSZYSTKIE POZYCJE Z APLIKACJI ===\n");

            try
            {
                using (var db = new BibliotekaContext())
                {
                    // Usuń wszystkie pozycje z tabeli
                    db.Pozycje.RemoveRange(db.Pozycje);
                    db.SaveChanges();
                }

                Console.WriteLine("Pomyślnie usunięto wszystkie pozycje.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas usuwania: {ex.Message}");
            }

            Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować.");
            Console.ReadKey();
        }
    }




    public class PozycjaMap : ClassMap<Pozycja>
    {
        public PozycjaMap()
        {
            Map(m => m.Id).Ignore();

            Map(m => m.Tytul).Name("Tytul", "Tytuł", "Title");
            Map(m => m.Rezyser).Name("Rezyser", "Reżyser", "Director");
            Map(m => m.RokWydania).Name("RokWydania", "Rok wydania", "Year");
            Map(m => m.Kategoria).Name("Kategoria", "Category");
            Map(m => m.Gatunek).Name("Gatunek", "Genre");
            Map(m => m.Obejrzane).Name("Obejrzane").TypeConverter<CustomBooleanConverter>();
        }
    }


}
