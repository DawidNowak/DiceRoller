using DiceRoller.DataAccess.Models;
using SkiaSharp;

namespace DiceRoller.Helpers
{
    public static class DrawHelper
    {
        private static readonly SKPaint WhiteColor = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.White
        };

        private static readonly SKPaint LightGrayStrokeColor = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 4,
            Color = SKColors.LightGray
        };

        private static readonly SKPaint BlackTextColor = new SKPaint
        {
            TextSize = 40.0f,
            IsAntialias = true,
            Color = SKColors.Black,
            IsStroke = false,
            TextAlign = SKTextAlign.Left
        };

        private static readonly SKPaint GrayTextColor = new SKPaint
        {
            TextSize = 20.0f,
            IsAntialias = true,
            Color = SKColors.Gray,
            IsStroke = false,
            TextAlign = SKTextAlign.Right
        };

        private static readonly SKSurface Surface = SKSurface.Create(100, 100, SKColorType.Rgba8888, SKAlphaType.Opaque);

        public static SKData DrawDice(int drawnResult, int minValue, int maxValue)
        {
            var canv = Surface.Canvas;
            canv.Clear(SKColors.White);

            canv.DrawRect(0f,0f,100f,100f, LightGrayStrokeColor);

            var textWidth = BlackTextColor.MeasureText(drawnResult.ToString());
            canv.DrawText(drawnResult.ToString(), 50f - textWidth/2, 65f, BlackTextColor);

            canv.DrawText(minValue.ToString(), 30f, 25f, GrayTextColor);
            canv.DrawText(maxValue.ToString(), 90f, 25f, GrayTextColor);

            return Surface.Snapshot().Encode(SKEncodedImageFormat.Jpeg, 100);
        }
    }
}
