using LinqConsoleLab.PL.Data;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    /// <summary>
    /// Zadanie:
    /// Wyszukaj wszystkich studentów mieszkających w Warsaw.
    /// Zwróć numer indeksu, pełne imię i nazwisko oraz miasto.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko, Miasto
    /// FROM Studenci
    /// WHERE Miasto = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        return DaneUczelni.Studenci
            .Where(student => student.Miasto == "Warsaw")
            .Select(student => $"{student.NumerIndeksu}, {student.Imie}, {student.Nazwisko}, {student.Miasto}");    // Zauważyć - brak maila na liście SELECT.
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj listę adresów e-mail wszystkich studentów.
    /// Użyj projekcji, tak aby w wyniku nie zwracać całych obiektów.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Studenci;
    /// </summary>
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        return DaneUczelni.Studenci
            .Select(student => $"{student.Email}");
    }

    /// <summary>
    /// Zadanie:
    /// Posortuj studentów alfabetycznie po nazwisku, a następnie po imieniu.
    /// Zwróć numer indeksu i pełne imię i nazwisko.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko
    /// FROM Studenci
    /// ORDER BY Nazwisko, Imie;
    /// </summary>
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        return DaneUczelni.Studenci
            .OrderBy(student => student.Nazwisko)
            .ThenBy(student => student.Imie)
            .Select(student => $"{student.NumerIndeksu}, {student.Imie}, {student.Nazwisko}");
    }

    /// <summary>
    /// Zadanie:
    /// Znajdź pierwszy przedmiot z kategorii Analytics.
    /// Jeżeli taki przedmiot nie istnieje, zwróć komunikat tekstowy.
    ///
    /// SQL:
    /// SELECT TOP 1 Nazwa, DataStartu
    /// FROM Przedmioty
    /// WHERE Kategoria = 'Analytics';
    /// </summary>
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        // return DaneUczelni.Przedmioty
        //     .Where(przedmiot => przedmiot.Kategoria == "Analytics")
        //     .Take(1)
        //     .Select(przedmiot => $"{przedmiot.Nazwa}, {przedmiot.DataStartu}");
        var przedmiot = DaneUczelni.Przedmioty
            .FirstOrDefault(p => p.Kategoria == "Analytics");
        
        if (przedmiot is null)
        {
            return ["Brak przedmiotu z kategorii Analytics."];
        }
        
        return [$"{przedmiot.Nazwa}, {przedmiot.DataStartu}"];
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy w danych istnieje przynajmniej jeden nieaktywny zapis.
    /// Zwróć jedno zdanie z odpowiedzią True/False albo Tak/Nie.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Zapisy
    ///     WHERE CzyAktywny = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        return [DaneUczelni.Zapisy.Any(z => !z.CzyAktywny) ? "1" : "0"];
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy każdy prowadzący ma uzupełnioną nazwę katedry.
    /// Warto użyć metody, która weryfikuje warunek dla całej kolekcji.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Katedra)
    /// THEN 1 ELSE 0 END
    /// FROM Prowadzacy;
    /// </summary>
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        return [DaneUczelni.Prowadzacy.All(prowadzacy => !string.IsNullOrWhiteSpace(prowadzacy.Katedra)) ? "1" : "0"];
    }

    /// <summary>
    /// Zadanie:
    /// Policz, ile aktywnych zapisów znajduje się w systemie.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Zapisy
    /// WHERE CzyAktywny = 1;
    /// </summary>
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        return [$"{DaneUczelni.Zapisy.Count(z => z.CzyAktywny)}"];
    }

    /// <summary>
    /// Zadanie:
    /// Pobierz listę unikalnych miast studentów i posortuj ją rosnąco.
    ///
    /// SQL:
    /// SELECT DISTINCT Miasto
    /// FROM Studenci
    /// ORDER BY Miasto;
    /// </summary>
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        //return DaneUczelni.Studenci
        //    .OrderBy(s => s.Miasto)
        //    .DistinctBy(s => s.Miasto)
        //    .Select(s => $"{s.Miasto}");
        return DaneUczelni.Studenci
            .Select(s => s.Miasto)
            .Distinct()
            .Order();
    }

    /// <summary>
    /// Zadanie:
    /// Zwróć trzy najnowsze zapisy na przedmioty.
    /// W wyniku pokaż datę zapisu, identyfikator studenta i identyfikator przedmiotu.
    ///
    /// SQL:
    /// SELECT TOP 3 DataZapisu, StudentId, PrzedmiotId
    /// FROM Zapisy
    /// ORDER BY DataZapisu DESC;
    /// </summary>
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        return DaneUczelni.Zapisy
            .OrderByDescending(z => z.DataZapisu)
            .Take(3)
            .Select(z => $"{z.DataZapisu}, {z.StudentId}, {z.PrzedmiotId}");
    }

    /// <summary>
    /// Zadanie:
    /// Zaimplementuj prostą paginację dla listy przedmiotów.
    /// Załóż stronę o rozmiarze 2 i zwróć drugą stronę danych.
    ///
    /// SQL:
    /// SELECT Nazwa, Kategoria
    /// FROM Przedmioty
    /// ORDER BY Nazwa
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        return DaneUczelni.Przedmioty
            .OrderBy(p => p.Nazwa)
            .Skip(2)
            .Take(2)
            .Select(p => $"{p.Nazwa}, {p.Kategoria}");
    }

    /// <summary>
    /// Zadanie:
    /// Połącz studentów z zapisami po StudentId.
    /// Zwróć pełne imię i nazwisko studenta oraz datę zapisu.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, z.DataZapisu
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId;
    /// </summary>
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        //return DaneUczelni.Studenci
        //    .Join(DaneUczelni.Zapisy, s => s.Id, z => z.StudentId, (s, z) => new { s.Imie, s.Nazwisko, z.DataZapisu })
        //    .Select(v => $"{v.Imie}, {v.Nazwisko}, {v.DataZapisu}");
        return DaneUczelni.Studenci
            .Join(DaneUczelni.Zapisy, s => s.Id, z => z.StudentId,
                (s, z) => $"{s.Imie} {s.Nazwisko}, {z.DataZapisu}");
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj wszystkie pary student-przedmiot na podstawie zapisów.
    /// Użyj podejścia, które pozwoli spłaszczyć dane do jednej sekwencji wyników.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, p.Nazwa
    /// FROM Zapisy z
    /// JOIN Studenci s ON s.Id = z.StudentId
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId;
    /// </summary>
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        // return DaneUczelni.Zapisy
        //     .Join(DaneUczelni.Studenci,
        //         zapis => zapis.StudentId,
        //         student => student.Id,
        //         (zapis, student) => new { student.Imie, student.Nazwisko, zapis.PrzedmiotId })
        //     .Join(DaneUczelni.Przedmioty,
        //         v => v.PrzedmiotId,
        //         przedmiot => przedmiot.Id,
        //         (arg1, przedmiot) => $"{arg1.Imie}, {arg1.Nazwisko}, {przedmiot.Nazwa}");
        return DaneUczelni.Studenci
            .SelectMany(
                student => DaneUczelni.Zapisy.Where(zapis => zapis.StudentId == student.Id),
                (student, zapis) => new { student.Imie, student.Nazwisko, zapis.PrzedmiotId }
            )
            .SelectMany(
                tymczasowy => DaneUczelni.Przedmioty.Where(przedmiot => przedmiot.Id == tymczasowy.PrzedmiotId),    // Tutaj - aby wyciągnąć dane z 1-elementowej kolekcji.
                (tymczasowy, przedmiot) => $"{tymczasowy.Imie}, {tymczasowy.Nazwisko}, {przedmiot.Nazwa}"
            );
    }

    /// <summary>
    /// Zadanie:
    /// Pogrupuj zapisy według przedmiotu i zwróć nazwę przedmiotu oraz liczbę zapisów.
    ///
    /// SQL:
    /// SELECT p.Nazwa, COUNT(*)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        return DaneUczelni.Zapisy
            .Join(DaneUczelni.Przedmioty,
                zapis => zapis.PrzedmiotId,
                przedmiot => przedmiot.Id,
                (zapis, przedmiot) => przedmiot.Nazwa)
            .GroupBy(nazwa => nazwa)
            .Select(grupa => $"{grupa.Key}, {grupa.Count()}");
    }

    /// <summary>
    /// Zadanie:
    /// Oblicz średnią ocenę końcową dla każdego przedmiotu.
    /// Pomiń rekordy, w których ocena końcowa ma wartość null.
    ///
    /// SQL:
    /// SELECT p.Nazwa, AVG(z.OcenaKoncowa)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        return DaneUczelni.Zapisy
            .Join(DaneUczelni.Przedmioty,
                zapis => zapis.PrzedmiotId,
                przedmiot => przedmiot.Id,
                (zapis, przedmiot) => new { przedmiot.Nazwa, zapis.OcenaKoncowa })
            .Where(v => v.OcenaKoncowa is not null)
            .GroupBy(v => v.Nazwa)
            .Select(grupa => $"{grupa.Key}, {grupa.Average(v => v.OcenaKoncowa)}");
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego prowadzącego policz liczbę przypisanych przedmiotów.
    /// W wyniku zwróć pełne imię i nazwisko oraz liczbę przedmiotów.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, COUNT(p.Id)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        // return DaneUczelni.Prowadzacy
        //     .LeftJoin(DaneUczelni.Przedmioty,
        //         prowadzacy => prowadzacy.Id,
        //         przedmiot => przedmiot.ProwadzacyId,
        //         (prowadzacy, przedmiot) => new { prowadzacy.Imie, prowadzacy.Nazwisko, PrzedmiotId = przedmiot?.Id })
        //     .GroupBy(v => new { v.Imie, v.Nazwisko })
        //     .Select(grupa => $"{grupa.Key.Imie}, " +
        //                      $"{grupa.Key.Nazwisko}, " +
        //                      $"{grupa.Count(v => v.PrzedmiotId is not null)}"
        //                      );
        return DaneUczelni.Prowadzacy
            .Select(pr => new 
            {
                pr.Imie, 
                pr.Nazwisko,
                LiczbaPrzedmiotow = DaneUczelni.Przedmioty.Count(p => p.ProwadzacyId == pr.Id)
            })
            .Select(x => $"{x.Imie}, {x.Nazwisko}, {x.LiczbaPrzedmiotow}");
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego studenta znajdź jego najwyższą ocenę końcową.
    /// Pomiń studentów, którzy nie mają jeszcze żadnej oceny.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, MAX(z.OcenaKoncowa)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY s.Imie, s.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        return DaneUczelni.Studenci
            .Join(DaneUczelni.Zapisy,
                s => s.Id,
                z => z.StudentId,
                (s, z) => new { s.Imie, s.Nazwisko, z.OcenaKoncowa })
            .Where(v => v.OcenaKoncowa is not null)
            .GroupBy(v => new { v.Imie, v.Nazwisko })
            .Select(grupa => $"{grupa.Key.Imie}, {grupa.Key.Nazwisko}, {grupa.Max(v => v.OcenaKoncowa)}");
    }

    /// <summary>
    /// Wyzwanie:
    /// Znajdź studentów, którzy mają więcej niż jeden aktywny zapis.
    /// Zwróć pełne imię i nazwisko oraz liczbę aktywnych przedmiotów.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Imie, s.Nazwisko
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        return DaneUczelni.Studenci
            .Join(DaneUczelni.Zapisy,
                s => s.Id,
                z => z.StudentId,
                (s, z) => new { s.Imie, s.Nazwisko, z.CzyAktywny })
            .Where(v => v.CzyAktywny)
            .GroupBy(v => new { v.Imie, v.Nazwisko })
            .Where(grupa => grupa.Count() > 1)
            .Select(grupa => $"{grupa.Key.Imie}, {grupa.Key.Nazwisko}, {grupa.Count()}");
    }

    /// <summary>
    /// Wyzwanie:
    /// Wypisz przedmioty startujące w kwietniu 2026, dla których żaden zapis nie ma jeszcze oceny końcowej.
    ///
    /// SQL:
    /// SELECT p.Nazwa
    /// FROM Przedmioty p
    /// JOIN Zapisy z ON p.Id = z.PrzedmiotId
    /// WHERE MONTH(p.DataStartu) = 4 AND YEAR(p.DataStartu) = 2026
    /// GROUP BY p.Nazwa
    /// HAVING SUM(CASE WHEN z.OcenaKoncowa IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        return DaneUczelni.Przedmioty
            .Where(p => p.DataStartu.Month == 4 && p.DataStartu.Year == 2026)
            .Join(DaneUczelni.Zapisy,
                p => p.Id,
                z => z.PrzedmiotId,
                (p, z) => new { p.Nazwa, z.OcenaKoncowa })
            .GroupBy(v => v.Nazwa)
            .Where(grupa => grupa.All(x => x.OcenaKoncowa is null))
            .Select(grupa => grupa.Key);
    }

    /// <summary>
    /// Wyzwanie:
    /// Oblicz średnią ocen końcowych dla każdego prowadzącego na podstawie wszystkich jego przedmiotów.
    /// Pomiń brakujące oceny, ale pozostaw samych prowadzących w wyniku.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, AVG(z.OcenaKoncowa)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// LEFT JOIN Zapisy z ON z.PrzedmiotId = p.Id
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        // return DaneUczelni.Prowadzacy
        //     .LeftJoin(DaneUczelni.Przedmioty,
        //         pr => pr.Id,
        //         p => p.ProwadzacyId,
        //         (pr, p) => new { pr.Imie, pr.Nazwisko, PrzedmiotId = p?.Id })
        //     .LeftJoin(DaneUczelni.Zapisy,
        //         x => x.PrzedmiotId,
        //         z => z.PrzedmiotId,
        //         (x, z) => new { x.Imie, x.Nazwisko, z?.OcenaKoncowa })
        //     .GroupBy(x => new { x.Imie, x.Nazwisko })
        //     .Select(grupa =>
        //     {
        //         var oceny = grupa
        //             .Where(v => v.OcenaKoncowa is not null)
        //             .Select(v => v.OcenaKoncowa.Value).ToList();
        //         string sredniaTekst = oceny.Any() ? oceny.Average().ToString("F2") : "NULL";
        //         return $"{grupa.Key.Imie}, {grupa.Key.Nazwisko}, {sredniaTekst}";
        //     });
        return DaneUczelni.Prowadzacy
            .Select(pr =>
            {
                var oceny = DaneUczelni.Przedmioty
                    .Where(p => p.ProwadzacyId == pr.Id)    // Jakiś prowadzący może nie mieć przedmiotu...
                    .SelectMany(p => DaneUczelni.Zapisy.Where(z => z.PrzedmiotId == p.Id))  // Jakiś przedmiot może nie mieć zapisów
                    .Where(z => z.OcenaKoncowa is not null)
                    .Select(z => z.OcenaKoncowa.Value)
                    .ToList();
                string sredniaTekst = oceny.Any() ? oceny.Average().ToString("F2") : "NULL";
                return $"{pr.Imie}, {pr.Nazwisko}, {sredniaTekst}";
            });
    }

    /// <summary>
    /// Wyzwanie:
    /// Pokaż miasta studentów oraz liczbę aktywnych zapisów wykonanych przez studentów z danego miasta.
    /// Posortuj wynik malejąco po liczbie aktywnych zapisów.
    ///
    /// SQL:
    /// SELECT s.Miasto, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Miasto
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        return DaneUczelni.Studenci
            .Join(DaneUczelni.Zapisy.Where(z => z.CzyAktywny),
                s => s.Id,
                z => z.StudentId,
                (s, z) => s.Miasto)
            .GroupBy(miasto => miasto)
            .Select(grupa => new { Miasto = grupa.Key, LiczbaZapisow = grupa.Count() })
            .OrderByDescending(x => x.LiczbaZapisow)
            .Select(x => $"{x.Miasto}, {x.LiczbaZapisow}");
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}
