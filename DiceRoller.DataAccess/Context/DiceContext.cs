using System.Linq;
using Autofac.Core;
using DiceRoller.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace DiceRoller.DataAccess.Context
{
    public class DiceContext : DbContext
    {
        public DiceContext(IDbPathHelper pathHelper)
        {
            DbPath = pathHelper.GetLocalFilePath("diceRollerDb.sqlite");
        }

        public string DbPath { get; }

        public DbSet<DiceWall> Walls { get; set; }
        public DbSet<Dice> Dice { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureGame()
                .ConfugureDice()
                .ConfugureDiceWall();

            base.OnModelCreating(modelBuilder);
        }

        public void RollBack()
        {
            var changedEntries = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged);

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
    }
}
