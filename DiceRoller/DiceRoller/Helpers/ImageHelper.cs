using System;
using System.Linq;
using DiceRoller.Controls;
using DiceRoller.DataAccess.Models;
using Xamarin.Forms;

namespace DiceRoller.Helpers
{
    public static class ImageHelper
    {
	    private static readonly Random Rand = new Random();

	    public static SwipeableImage DrawDice(Dice dice, bool randomWall = true)
	    {
			return new SwipeableImage
			{
				Source = BlobHelper.GetImgSource(dice.Walls.ElementAt(Rand.Next(0, dice.Walls.Count)).Image),
				BindingContext = dice,
				HeightRequest = 64d,
				WidthRequest = 64d
			};
		}

	    public static Image DrawMini(Dice dice)
	    {
		    return new Image
		    {
			    Source = BlobHelper.GetImgSource(dice.MiniImage),
			    BindingContext = dice,
			    HeightRequest = 36d,
			    WidthRequest = 36d
		    };
		}

	    public static SwipeableImage DrawDiceWall(DiceWall wall, double size = 36d)
	    {
		    return new SwipeableImage
		    {
			    Source = BlobHelper.GetImgSource(wall.Image),
			    WidthRequest = size,
			    HeightRequest = size
		    };
	    }
    }
}
