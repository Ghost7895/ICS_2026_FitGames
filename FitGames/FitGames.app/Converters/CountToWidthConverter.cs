using System.Globalization;

namespace FitGames.app.Converters;

public class CountToWidthConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int count)
        {
            // Scale the width based on count (max width 200)
            // Adjust the multiplier based on your max expected count
            return Math.Min(count * 20, 200);
        }

        return 0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
