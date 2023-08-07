
namespace Models;

public enum WeatherState
{
    Cloudy,
    Foggy,
    Overcast,
    PartlyCloudy,
    Raining,
    Snowy,
    Sunshine,
    Thunder
}

// ReSharper disable once ClassNeverInstantiated.Global
public class WeatherStateMetadata
{
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public WeatherState State { get; set; }

    // ReSharper disable once UnusedMember.Global
    public IEnumerable<string> Aliases { get; set; } = Enumerable.Empty<string>();
}

public static class WeatherStateFactory
{
    private class ImportInfo
    {
        //[ImportMany]
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        public IEnumerable<Lazy<IWeatherState, WeatherStateMetadata>> WeatherStates { get; set; } =
            Enumerable.Empty<Lazy<IWeatherState, WeatherStateMetadata>>();
    }

    private static readonly ImportInfo _info = new();

    static WeatherStateFactory()
    {
        // var assemblies = new[] { typeof(IWeatherState).Assembly };
        // var configuration = new ContainerConfiguration();
        // try
        // {
        //     configuration = configuration.WithAssemblies(assemblies);
        // }
        // catch (Exception)
        // {
        //     //ContextManager.Context.Logger.Error("Failed to load weather state assemblies");
        //     throw;
        // }
        //
        // var container = configuration.CreateContainer();
        // container.SatisfyImports(_info);
    }

    public static IWeatherState? GetWeatherStateByAlias(string alias)
    {
        alias = alias.Replace(" ", "").Split(',').First();
        var weatherState =
            _info.WeatherStates.FirstOrDefault(weatherState =>
                weatherState.Metadata.Aliases.Contains(alias.Replace(" ", ""),
                    StringComparer.InvariantCultureIgnoreCase));

        if (weatherState is not null) return weatherState.Value;

        //ContextManager.Context.Logger.Error($"Weather state by alias \"{alias}\" doesnt exist");
        return null;
    }
}