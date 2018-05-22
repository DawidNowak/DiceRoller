using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiceRoller.Extensions
{
    [ContentProperty("ResourceId")]
    public class EmbeddedImage : IMarkupExtension
    {
        public string ResourceId { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return !string.IsNullOrWhiteSpace(ResourceId) ? ImageSource.FromResource(ResourceId) : null;
        }
    }
}
