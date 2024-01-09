using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static School.SkolaDbContext;

namespace School
{
    public class SkolaDbContext : DbContext
    {
        public DbSet<Elev> Elever { get; set; }
        public DbSet<Personal> PersonalSet { get; set; }
        public DbSet<Kurs> Kurser { get; set; }
        public DbSet<Betyg> BetygSet { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=School;Trusted_Connection=True;");
        }

        public class Elev
        {
            public int ElevId { get; set; }
            public string? Namn { get; set; }
            public string? Personnummer { get; set; }
            public string? Klass { get; set; }
        }

        public class Personal
        {
            public int PersonalId { get; set; }
            public string? Namn { get; set; }
            public string? Befattning { get; set; }
            public DateTime Anställningsdatum { get; set; } 
            public string? Avdelning { get; set; } 
        }

        public class Kurs
        {
            public int KursId { get; set; }
            public string? Kursnamn { get; set; }
        }

        public class Betyg
        {
            public int BetygId { get; set; }
            public int ElevId { get; set; }
            public int KursId { get; set; }
            public int Betygsgrad { get; set; }
            public DateTime BetygDatum { get; set; }

            public Elev? BetygElev { get; set; }
            public Kurs? BetygKurs { get; set; }
        }

        public int GetNumberOfTeachersOnDepartment(string department)
        {
            return PersonalSet.Count(p => p.Befattning == "Lärare" && p.Avdelning == department);
        }

        public List<Elev> GetAllStudents()
        {
            return Elever.ToList();
        }

        public List<Kurs> GetAllCourses()
        {
            return Kurser.ToList();
        }

        public static int ConvertBetygsgradToInt(string betygsgrad)
        {
            switch (betygsgrad.ToUpper())
            {
                case "A":
                    return 6;
                case "B":
                    return 5;
                case "C":
                    return 4;
                case "D":
                    return 3;
                case "E":
                    return 2;
                case "F":
                    return 1;
                default:
                    return 0;
            }
        }

        static void Main()
        {
            using (var dbContext = new SkolaDbContext())
            {
                bool exit = false;

                while (!exit)
                {
                    Console.WriteLine("Vad önskar du se?:");
                    Console.WriteLine("1. Hämta personal");
                    Console.WriteLine("2. Hämta alla elever");
                    Console.WriteLine("3. Hämta alla elever i en viss klass");
                    Console.WriteLine("4. Hämta alla betyg som satts den senaste månaden");
                    Console.WriteLine("5. Hämta en lista med alla kurser och dess statistik");
                    Console.WriteLine("6. Lägg till nya elever och betyg");
                    Console.WriteLine("7. Lägg till ny personal");
                    Console.WriteLine("8. Översikt över all personal med befattningar och arbetstid");
                    Console.WriteLine("9. Antal lärare på varje avdelning");
                    Console.WriteLine("10. Visa information om alla elever");
                    Console.WriteLine("11. Visa en lista på alla kurser");
                    Console.WriteLine("12. Avsluta");

                    Console.Write("Ange ditt val: ");
                    string val = Console.ReadLine();

                    switch (val)
                    {
                        case "1":
                            Console.WriteLine("Vill du se alla anställda eller någon specifik?");
                            Console.WriteLine("1. Alla anställda");
                            Console.WriteLine("2. Lärare");
                            Console.WriteLine("3. Administratörer");
                            Console.WriteLine("4. Rektorer");
                            Console.Write("Ange ditt val: ");
                            string personalVal = Console.ReadLine();

                            switch (personalVal)
                            {
                                case "1":
                                    var allaAnstallda = dbContext.PersonalSet.ToList();
                                    Console.WriteLine("Alla anställda:");
                                    foreach (var anställd in allaAnstallda)
                                    {
                                        Console.WriteLine($"{anställd.Namn} - {anställd.Befattning}");
                                    }
                                    break;
                                case "2":
                                    var lärare = dbContext.PersonalSet.Where(p => p.Befattning == "Lärare").ToList();
                                    Console.WriteLine("Alla lärare:");
                                    foreach (var lärarePerson in lärare)
                                    {
                                        Console.WriteLine($"{lärarePerson.Namn} - {lärarePerson.Befattning}");
                                    }
                                    break;
                                case "3":
                                    var admins = dbContext.PersonalSet.Where(p => p.Befattning == "Administratör").ToList();
                                    Console.WriteLine("Alla administratörer:");
                                    foreach (var admin in admins)
                                    {
                                        Console.WriteLine($"{admin.Namn} - {admin.Befattning}");
                                    }
                                    break;
                                case "4":
                                    var rektorer = dbContext.PersonalSet.Where(p => p.Befattning == "Rektor").ToList();
                                    Console.WriteLine("Alla rektorer:");
                                    foreach (var rektor in rektorer)
                                    {
                                        Console.WriteLine($"{rektor.Namn} - {rektor.Befattning}");
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Fel!. Försök igen.");
                                    break;
                            }
                            break;

                        case "2":
                            var elever = dbContext.Elever.OrderBy(e => e.Namn).ToList();
                            Console.WriteLine("Alla elever:");
                            foreach (var elev in elever)
                            {
                                Console.WriteLine($"{elev.Namn} - {elev.Personnummer} - {elev.Klass}");
                            }
                            break;

                        case "3":
                            var klasser = dbContext.Elever.Select(e => e.Klass).Distinct().ToList();

                            Console.WriteLine("Välj en klass:");
                            for (int i = 0; i < klasser.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {klasser[i]}");
                            }

                            Console.Write("Ange klassnummer: ");
                            int valdKlassIndex;
                            if (int.TryParse(Console.ReadLine(), out valdKlassIndex) && valdKlassIndex > 0 && valdKlassIndex <= klasser.Count)
                            {
                                var valdKlass = klasser[valdKlassIndex - 1];
                                var eleverIKlass = dbContext.Elever
                                    .Where(e => e.Klass == valdKlass)
                                    .OrderBy(e => e.Namn)
                                    .ToList();

                                Console.WriteLine($"Elever i klass {valdKlass}:");
                                foreach (var elev in eleverIKlass)
                                {
                                    Console.WriteLine($"{elev.Namn} - {elev.Personnummer}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("NÄÄÄE! Försök igen.");
                            }
                            break;

                        case "4":
                            var senasteManadensBetyg = dbContext.BetygSet
                                .Where(b => b.BetygDatum >= DateTime.Now.AddMonths(-1))
                                .ToList();

                            Console.WriteLine("Betyg som satts den senaste månaden:");
                            foreach (var betyg in senasteManadensBetyg)
                            {
                                Console.WriteLine($"{betyg.BetygElev.Namn} - {betyg.BetygKurs.Kursnamn} - {betyg.Betygsgrad} - {betyg.BetygDatum}");
                            }
                            break;

                        case "5":
                            var kursStatistik = dbContext.Kurser
                                .Select(k => new
                                {
                                    Kursnamn = k.Kursnamn,
                                    Snittbetyg = dbContext.BetygSet
                                        .Where(b => b.KursId == k.KursId && b.Betygsgrad != null)
                                        .Average(b => (double?)Convert.ToInt32(b.Betygsgrad)),
                                    HögstaBetyg = dbContext.BetygSet
                                        .Where(b => b.KursId == k.KursId && b.Betygsgrad != null)
                                        .Max(b => (double?)Convert.ToInt32(b.Betygsgrad)),
                                    LägstaBetyg = dbContext.BetygSet
                                        .Where(b => b.KursId == k.KursId && b.Betygsgrad != null)
                                        .Min(b => (double?)Convert.ToInt32(b.Betygsgrad))
                                })
                                .ToList();
                            break;

                        case "6":
                            Console.Write("Ange elevens namn: ");
                            string nyElevNamn2 = Console.ReadLine();
                            Console.Write("Ange elevens personnummer: ");
                            string nyElevPersonnummer2 = Console.ReadLine();
                            Console.Write("Ange elevens klass: ");
                            string nyElevKlass2 = Console.ReadLine();

                            var nyElev2 = new Elev { Namn = nyElevNamn2, Personnummer = nyElevPersonnummer2, Klass = nyElevKlass2 };
                            dbContext.Elever.Add(nyElev2);
                            dbContext.SaveChanges();

                            Console.WriteLine("Elev har lagts till. Ange betyg för eleven.");

                            var kurser = dbContext.Kurser.ToList();
                            Console.WriteLine("Välj kurs att betygsätta:");
                            for (int i = 0; i < kurser.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {kurser[i].Kursnamn}");
                            }

                            Console.Write("Ange kursnummer: ");
                            int valdKursIndex2;
                            if (int.TryParse(Console.ReadLine(), out valdKursIndex2) && valdKursIndex2 > 0 && valdKursIndex2 <= kurser.Count)
                            {
                                var kurs = kurser[valdKursIndex2 - 1];

                                Console.Write("Ange betygsgrad (A-F): ");
                                int betygsgrad;
                                if (int.TryParse(Console.ReadLine(), out betygsgrad) && betygsgrad >= 1 && betygsgrad <= 6)

                                    Console.Write("Ange lärarens namn: ");
                                string lärareNamn = Console.ReadLine();

                                var lärare = dbContext.PersonalSet.FirstOrDefault(p => p.Namn == lärareNamn && p.Befattning == "Lärare");

                                if (lärare != null)
                                {
                                    var betyg = new Betyg
                                    {
                                        ElevId = nyElev2.ElevId,
                                        KursId = kurs.KursId,
                                        Betygsgrad = betygsgrad,
                                        BetygDatum = DateTime.Now,
                                        BetygElev = nyElev2,
                                        BetygKurs = kurs
                                    };
                                    dbContext.BetygSet.Add(betyg);
                                    dbContext.SaveChanges();

                                    Console.WriteLine($"Betyg {betygsgrad} satt för {nyElevNamn2} i kursen {kurs.Kursnamn} av {lärareNamn}.");
                                }
                                else
                                {
                                    Console.WriteLine($"Läraren {lärareNamn} hittades inte eller är inte av befattning 'Lärare'. Betyget har inte lagts till.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Felaktigt antal kurser. Försök igen.");
                            }
                            break;

                        case "7":
                            Console.Write("Ange personalens namn: ");
                            string nyPersonalNamn2 = Console.ReadLine();
                            Console.Write("Ange personalens befattning: ");
                            string nyPersonalBefattning2 = Console.ReadLine();
                            Console.Write("Ange personalens avdelning: ");
                            string nyPersonalAvdelning = Console.ReadLine();
                            Console.Write("Ange anställningsdatum (yyyy-mm-dd): ");
                            DateTime nyPersonalAnställningsdatum;
                            if (DateTime.TryParse(Console.ReadLine(), out nyPersonalAnställningsdatum))
                            {
                                var nyPersonal2 = new Personal { Namn = nyPersonalNamn2, Befattning = nyPersonalBefattning2, Anställningsdatum = nyPersonalAnställningsdatum, Avdelning = nyPersonalAvdelning };
                                dbContext.PersonalSet.Add(nyPersonal2);
                                dbContext.SaveChanges();

                                Console.WriteLine("Nu finns personen i systemet.");
                            }
                            else
                            {
                                Console.WriteLine("Felaktigt datumformat. Försök igen.");
                            }
                            break;


                        case "8":
                            var allPersonal = dbContext.PersonalSet.ToList();
                            Console.WriteLine("Översikt över all personal med befattningar och arbetstid:");
                            foreach (var person in allPersonal)
                            {
                                Console.WriteLine($"{person.Namn} - {person.Befattning} - Anställd sedan: {person.Anställningsdatum}");
                            }
                            break;                         

                        

                        case "9":
                            // Antal lärare på varje avdelning
                            var departments = dbContext.PersonalSet.Where(p => p.Befattning == "Lärare").Select(p => p.Avdelning).Distinct().ToList();

                            Console.WriteLine("Antal lärare på varje avdelning:");
                            foreach (var department in departments)
                            {
                                var numberOfTeachers = dbContext.GetNumberOfTeachersOnDepartment(department);
                                Console.WriteLine($"{department}: {numberOfTeachers} lärare");
                            }
                            break;

                        case "10":
                            // Visa information om alla elever
                            var allStudents = dbContext.GetAllStudents();

                            Console.WriteLine("Information om alla elever:");
                            foreach (var student in allStudents)
                            {
                                Console.WriteLine($"{student.Namn} - {student.Personnummer} - {student.Klass}");
                            }
                            break;

                        case "11":
                            // Visa en lista på alla kurser
                            var allCourses = dbContext.GetAllCourses();

                            Console.WriteLine("Lista på alla (aktiva) kurser:");
                            foreach (var course in allCourses)
                            {
                                Console.WriteLine($"{course.Kursnamn}");
                            }
                            break;

                        case "12":
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Fel! Försök igen.");
                            break;
                    }
                }
            }
        }
    }
}
