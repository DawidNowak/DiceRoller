using DiceRoller.DataAccess.Model;
using Xamarin.Forms.Internals;

namespace DiceRoller.DataAccess.Context.Seed
{
    public static class GamesReader
    {
        public static Game[] GetAll()
        {
            var index = 1;
            var miceDice = new Dice(6, "DiceRoller.Core.Images.MiceMystics.") {Id = 1, GameId = 1};
            miceDice.Walls = new[]
            {
                new DiceWall {Dice = miceDice,ImageSource = "mm_bow.jpg"},
                new DiceWall {Dice = miceDice,ImageSource = "mm_bow_surge.jpg"},
                new DiceWall {Dice = miceDice,ImageSource = "mm_cheese.jpg"},
                new DiceWall {Dice = miceDice,ImageSource = "mm_shield_sword.jpg"},
                new DiceWall {Dice = miceDice,ImageSource = "mm_shield_sword_surge.jpg"},
                new DiceWall {Dice = miceDice,ImageSource = "mm_sword_surge.jpg"}
            };
            miceDice.Walls.ForEach(w =>
            {
                w.Id = index++;
                w.DiceId = 1;
            });

            var games = new[]
            {
                new Game {Dice = new[] {miceDice}, Name = "Mice and Mystics", Id = 1}
            };

            return games;
        }
    }
}
