using DiceRoller.DataAccess.Models;
using Xamarin.Forms.Internals;

namespace DiceRoller.DataAccess.Context
{
    public static class Seed
    {
        public static Game[] GetGames()
        {
            var games = new[]
            {
                new Game {Name = "Mice and Mystics", Id = 1}
            };

            return games;
        }

        public static Dice[] GetDice()
        {
            var dice = new[]
            {
                new Dice
                {
                    Id = 1,
                    GameId = 1,
                    Path = "DiceRoller.Images.MiceMystics.",
                    MiniImageSource = "mm_mini.jpg"
                }
            };

            return dice;
        }

        public static DiceWall[] GetWalls()
        {
            var walls = new[]
            {
                new DiceWall {DiceId = 1, ImageSource = "mm_bow.jpg"},
                new DiceWall {DiceId = 1, ImageSource = "mm_bow_surge.jpg"},
                new DiceWall {DiceId = 1, ImageSource = "mm_cheese.jpg"},
                new DiceWall {DiceId = 1, ImageSource = "mm_shield_sword.jpg"},
                new DiceWall {DiceId = 1, ImageSource = "mm_shield_sword_surge.jpg"},
                new DiceWall {DiceId = 1, ImageSource = "mm_sword_surge.jpg"}
            };

            var index = 1;
            walls.ForEach(w =>
            {
                w.Id = index++;
            });

            return walls;
        }
    }
}
