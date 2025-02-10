using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

public class CustomBooleanConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null; // Zwróć null dla pustych lub nullowych wartości tekstowych
        }

        if (text.ToLower() == "tak")
        {
            return true;
        }
        if (text.ToLower() == "nie")
        {
            return false;
        }

        return base.ConvertFromString(text, row, memberMapData);
    }

    public override string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value is bool booleanValue)
        {
            return booleanValue ? "Tak" : "Nie";
        }

        return base.ConvertToString(value, row, memberMapData);
    }
}
