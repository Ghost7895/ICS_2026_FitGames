using System.Globalization;
using FitGames.DAL.Enums;

namespace FitGames.app.Converters;

public class PegiToImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            Pegi.Pegi3 => "pegi3.png",
            Pegi.Pegi7 => "pegi7.png",
            Pegi.Pegi12 => "pegi12.png",
            Pegi.Pegi16 => "pegi16.png",
            Pegi.Pegi18 => "pegi18.png",
            _ => null
        };
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}