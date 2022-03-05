using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
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

            //QueryFilters();
            //QueryAggregates();
            //RetriveeAndUpdateSamurai();
            //RetriveeAndUpdateMultipleSamurais();
            //MultipleDatabaseOperations();

            //RetriveeAndDeleteSamurai();

            //QueryAndUpdateBattles_Disconnect();

            // InsertNewSamuraiWithQuote();
            // InsertNewSamuraiWithManyQuote();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiNotTracked(2);

            //EagarLoadSamuraiWithQuotes();
            //ProjectSomeProperties();
            //ProjectSamuraisWithQuotes();

            //ExplicitLoadQuotes();
            // FilteringWithRelatedData();

            //ModifyingRelatedDataWhenTracked();
            //ModifyingRelatedDataWhenNotTracked();

            //AddingNewSamuraiToExistingBattle();
            //ReturnBattleWithSamurais();
            //ReturnAllBattleWithSamurais();
            //AddAllSamuraisToAllBattles();

            // RemoveSamuraiFromABattle();


            QuerySamuraiBattleStats();
        }

        private static void QuerySamuraiBattleStats()
        {
            //var stats = _context.SamuraiBattleStats.ToList();
            var firststats = _context.SamuraiBattleStats.FirstOrDefault();
            var sampsonState = _context.SamuraiBattleStats
                .FirstOrDefault(b => b.Name ==  "SampsonSan");

        }

        private static void RemoveSamuraiFromABattle()
        {
            var battleWithSamurai = _context.Battles
                .Include(b => b.Samurais.Where(s => s.Id == 12))
                .Single(s => s.BattleId == 1);
            var samurai = battleWithSamurai.Samurais[0];
            battleWithSamurai.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void AddAllSamuraisToAllBattles()
        {
            var allbattles = _context.Battles.Include(b => b.Samurais).ToList();
            var allSamurais = _context.Samurais.ToList();
            foreach (var battle in allbattles)
            {
                battle.Samurais.AddRange(allSamurais);
            }
            _context.SaveChanges();
        }

        private static void ReturnAllBattleWithSamurais()
        {
            var battles = _context.Battles.Include(b => b.Samurais).ToList();
        }

        private static void ReturnBattleWithSamurais()
        {
            var battle = _context.Battles.Include(b => b.Samurais).FirstOrDefault();
        }

        private static void AddingNewSamuraiToExistingBattle()
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.Samurais.Add(new Samurai { Name = "Takeda Shingen" });
            _context.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais
                .Include(s => s.Quotes).FirstOrDefault(s => s.Id == 2);

            //update
            var quote = samurai.Quotes[0];
             quote.Text += "Did you here that again?";

            using var newContext = new SamuraiContext();
            newContext.Entry(quote).State = EntityState.Modified;
            newContext.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais
                .Include(s => s.Quotes).FirstOrDefault(s => s.Id == 2);

            //update
            samurai.Quotes[0].Text = "Did you here that?";

            //Delete
            _context.Quotes.Remove(samurai.Quotes[2]);
            _context.SaveChanges();
        }

        private static void FilteringWithRelatedData()
        {
            var samurais = _context.Samurais
                .Where(s => s.Quotes.Any(q => q.Text.Contains("happy"))).ToList();
        }

        private static void ExplicitLoadQuotes()
        {
            _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr. Ed" });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            var samuria = _context.Samurais.Find(1);
            _context.Entry(samuria).Collection(s => s.Quotes).Load();
            _context.Entry(samuria).Reference(s => s.Horse).Load();
        }

        private static void ProjectSamuraisWithQuotes()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name,s.Quotes }).ToList();
            var somePropertiesWithQuotes = _context.Samurais.Select(s => new
            {
                s.Id,
                s.Name,
                HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
            }).ToList();

            var samuraisAndQuotes = _context.Samurais.Select(s => new { Samurai = s, HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy")) }).ToList();
            
        }

        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new {s.Id,s.Name}).ToList();
            var idAndName = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name )).ToList();
        }
        public struct IdAndName
        {
            public int id;
            public string Name;
            public IdAndName(int id,string name) : this()
            {
                this.id = id;
                Name = name;
            }

        }

        private static void EagarLoadSamuraiWithQuotes()
        {
            var samuraiWithQuotes = _context.Samurais
                .Include(s => s.Quotes.Where(q => q.Text.Contains("Thanks"))).ToList();

            //var filterPrimaryEntityWithInclude = _context.Samurais.Where(s => s.Name.Contains("Sampson"))
            //    .Include(s => s.Quotes).FirstOrDefault();
            //_context.Samurais.Include(s => s.Quotes);
            //_context.Samurais.Include(s => s.Quotes).ThenInclude(q => q.Ttanslations);
            //_context.Samurais.Include(s => s.Quotes.Translations);
            //_context.Samurais.Include(s => s.Quotes).Include(s => s.Clan);


        }

        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that I saved you,will you feed me dinner?"
            });
            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Update(samurai);
                newContext.SaveChanges();
            }
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "I be you're happy that I've saved you"
            });
            _context.SaveChanges();
        }

        private static void InsertNewSamuraiWithManyQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kyuzo",
                Quotes = new List<Quote>
                {
                    new Quote{Text="Watch out for my sharp sword!"},
                    new Quote{Text="I told you to watch out for the sharp sword! oh well!"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewSamuraiWithQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kamberi Shimada",
                Quotes = new List<Quote>
                {
                    new Quote{Text="I'we come to save you"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattles_Disconnect()
        {
            List<Battle> disconnectedBattles;
            using (var context1 = new SamuraiContext())
            {
                disconnectedBattles = _context.Battles.ToList();
            }

            disconnectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1570, 01, 01);
                b.EndDate = new DateTime(1570, 12, 01);
            });

            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }
        }

        private static void RetriveeAndDeleteSamurai()
        {
            var samurai = _context.Samurais.Find(18);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void MultipleDatabaseOperations()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.Samurais.Add(new Samurai { Name = "Shino" });
            _context.SaveChanges();
        }

        private static void RetriveeAndUpdateMultipleSamurais()
        {
            var samurai = _context.Samurais.Skip(1).Take(4).ToList();
            samurai.ForEach(s => s.Name += "San");
            _context.SaveChanges();
        }

        private static void RetriveeAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.SaveChanges();
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
