using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        private static void Main(string[] args)
        {
            // AddSamurais("Shimada","Okamoto","Kikuchio","Hayashida");
            // AddSamurai();
            // AddVariousTypes();
            //GetSamurais("After Add:");
            //Console.Write("Press any key...");
            //Console.ReadKey();

            QueryFilters();
            QueryAggregates();
        }

        private static void QueryAggregates()
        {
            var name = "Shimada";
            var samurais = _context.Samurais.Where(s => s.Name == name).FirstOrDefault();
            //_context.Samurais.Find(2);
        }

        private static void QueryFilters()
        {
            var name = "Shimada";
            var samurais = _context.Samurais.FirstOrDefault(s => s.Name == name);

            //_context.Samurais.Where(s => EF.Functions.Like(s.Name, "J%")).ToList();

            //_context.Samurais.Where(s => s.Name.Contains("abc"));
        }

        private static void AddVariousTypes()
        {
            _context.Samurais.AddRange(
                new Samurai { Name = "Shimada" },
                new Samurai { Name = "Okamoto" });

            _context.Battles.AddRange(
                new Battle { Name = "Battle of Anegawa" },
                new Battle { Name = "Battle of Nagashino" });
            _context.SaveChanges();

            //OR
            //_context.AddRange(
            //    new Samurai { Name = "Shimada" },
            //    new Samurai { Name = "Okamoto" },
            //    new Battle { Name = "Battle of Anegawa" },
            //    new Battle { Name = "Battle of Nagashino" });
            //   _context.SaveChanges();
        }
        private static void AddSamurais(params string[] names)
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai {Name=name });
            }

            _context.SaveChanges();
        }
        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "Sampson" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais.ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }
    }
}
