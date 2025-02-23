using System.Globalization;

namespace MoreCoffee.Converters;

public class EnumToListConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Type enumType && enumType.IsEnum)
        {
            return Enum.GetValues(enumType).Cast<object>().ToList();
        }
        return new List<object>();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}