using System.Linq;
using DiceRoller.DataAccess.Context.Seed;
using DiceRoller.DataAccess.Model;
using Xamarin.Forms.Internals;

namespace DiceRoller.DataAccess.Context
{
    public static class DbContextExtension
    {
        public static void EnsureSeeded(this DiceContext ctx)
        {
            var games = GamesReader.GetAll();
            ctx.AddOrUpdate(games);
            var dice = games.SelectMany(g => g.Dice).ToArray();
            ctx.AddOrUpdate(dice);
            ctx.AddOrUpdate(dice.SelectMany(d => d.Walls).ToArray());
            ctx.SaveChanges();
        }

        public static void AddOrUpdate<T>(this DiceContext ctx, T[] set) where T : Entity
        {
            var dbSet = ctx.Set<T>();
            set.ForEach(e =>
            {
                if (dbSet.All(x => x.Id != e.Id)) dbSet.Add(e);
                else dbSet.Update(e);
            });
        }
    }
}
