using System.Globalization;

namespace Models;

public class City
{
    public string Name { get; init; }
    public string Country { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public string CountryIso { get; init; }

    // ReSharper disable once MemberCanBePrivate.Global
    public City(string name, string country, double latitude, double longitude, string countryIso)
    {
        Name = name;
        Country = country;
        Latitude = latitude;
        Longitude = longitude;
        CountryIso = countryIso;
    }

    public static City Parse(string line)
    {
        const int nameKey = 0;
        const int latKey = 2;
        const int longKey = 3;
        const int countryKey = 4;
        const int countryIsoKey = 5;

        var parts = line.Split(',');
        return new(parts[nameKey], parts[countryKey], double.Parse(parts[latKey], CultureInfo.InvariantCulture),
            double.Parse(parts[longKey], CultureInfo.InvariantCulture), parts[countryIsoKey]);
    }
}