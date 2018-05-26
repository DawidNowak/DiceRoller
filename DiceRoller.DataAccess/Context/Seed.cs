using System.Collections.Generic;
using DiceRoller.DataAccess.Helpers;
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
                new Game {Name = "Mice and Mystics"},
                new Game {Name = "Descent 2nd ed."}
            };

            GiveIds(games);

            return games;
        }

        public static Dice[] GetDice()
        {
            var dice = new[]
            {
                new Dice
                {
                    GameId = 1,
                    Path = "DiceRoller.Images.MiceMystics.",
                    MiniImageSource = "mm_mini.jpg"
                },
                new Dice
                {
                    GameId = 2,
                    Path = "DiceRoller.Images.Descent.yellow.",
                    MiniImageSource = "mini.jpg"
                },
                new Dice
                {
                    GameId = 2,
                    Path = "DiceRoller.Images.Descent.red.",
                    MiniImageSource = "mini.jpg"
                },
                new Dice
                {
                    GameId = 2,
                    Path = "DiceRoller.Images.Descent.blue.",
                    MiniImageSource = "mini.jpg"
                },
                new Dice
                {
                    GameId = 2,
                    Path = "DiceRoller.Images.Descent.brown.",
                    MiniImageSource = "mini.jpg"
                },
                new Dice
                {
                    GameId = 2,
                    Path = "DiceRoller.Images.Descent.gray.",
                    MiniImageSource = "mini.jpg"
                },
                new Dice
                {
                    GameId = 2,
                    Path = "DiceRoller.Images.Descent.black.",
                    MiniImageSource = "mini.jpg"
                }
            };

            GiveIds(dice);

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
                new DiceWall {DiceId = 1, ImageSource = "mm_sword_surge.jpg"},

                new DiceWall {DiceId = 2, ImageSource = "one_dist_one_heart.jpg"},
                new DiceWall {DiceId = 2, ImageSource = "one_dist_surge.jpg"},
                new DiceWall {DiceId = 2, ImageSource = "one_heart_surge.jpg"},
                new DiceWall {DiceId = 2, ImageSource = "two_dist_one_heart.jpg"},
                new DiceWall {DiceId = 2, ImageSource = "two_hearts.jpg"},
                new DiceWall {DiceId = 2, ImageSource = "two_hearts_surge.jpg"},

                new DiceWall {DiceId = 3, ImageSource = "one_heart.jpg"},
                new DiceWall {DiceId = 3, ImageSource = "two_hearts.jpg"},
                new DiceWall {DiceId = 3, ImageSource = "two_hearts.jpg"},
                new DiceWall {DiceId = 3, ImageSource = "two_hearts.jpg"},
                new DiceWall {DiceId = 3, ImageSource = "three_hearts.jpg"},
                new DiceWall {DiceId = 3, ImageSource = "three_hearts_surge.jpg"},

                new DiceWall {DiceId = 4, ImageSource = "fail.jpg"},
                new DiceWall {DiceId = 4, ImageSource = "two_dist_two_hearts_surge.jpg"},
                new DiceWall {DiceId = 4, ImageSource = "three_dist_two_hearts.jpg"},
                new DiceWall {DiceId = 4, ImageSource = "four_dist_two_hearts.jpg"},
                new DiceWall {DiceId = 4, ImageSource = "five_dist_one_heart.jpg"},
                new DiceWall {DiceId = 4, ImageSource = "six_dist_one_hearts_surge.jpg"},

                new DiceWall {DiceId = 5, ImageSource = "no_shield.jpg"},
                new DiceWall {DiceId = 5, ImageSource = "no_shield.jpg"},
                new DiceWall {DiceId = 5, ImageSource = "no_shield.jpg"},
                new DiceWall {DiceId = 5, ImageSource = "one_shield.jpg"},
                new DiceWall {DiceId = 5, ImageSource = "one_shield.jpg"},
                new DiceWall {DiceId = 5, ImageSource = "two_shields.jpg"},

                new DiceWall {DiceId = 6, ImageSource = "no_shield.jpg"},
                new DiceWall {DiceId = 6, ImageSource = "one_shield.jpg"},
                new DiceWall {DiceId = 6, ImageSource = "one_shield.jpg"},
                new DiceWall {DiceId = 6, ImageSource = "one_shield.jpg"},
                new DiceWall {DiceId = 6, ImageSource = "two_shields.jpg"},
                new DiceWall {DiceId = 6, ImageSource = "three_shields.jpg"},

                new DiceWall {DiceId = 7, ImageSource = "no_shield.jpg"},
                new DiceWall {DiceId = 7, ImageSource = "two_shields.jpg"},
                new DiceWall {DiceId = 7, ImageSource = "two_shields.jpg"},
                new DiceWall {DiceId = 7, ImageSource = "two_shields.jpg"},
                new DiceWall {DiceId = 7, ImageSource = "three_shields.jpg"},
                new DiceWall {DiceId = 7, ImageSource = "four_shields.jpg"}
            };

            GiveIds(walls);

            return walls;
        }

        public static Config[] GetConfigs()
        {
            var configs = new[]
            {
                new Config {Key = Consts.RollAnimationKey, Value = true.ToString()}
            };

            GiveIds(configs);

            return configs;
        }

        private static void GiveIds<T>(IEnumerable<T> list) where T : Entity
        {
            var index = 1;
            list.ForEach(w =>
            {
                w.Id = index++;
            });
        }
    }
}
