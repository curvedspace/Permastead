

// ReSharper disable UnusedType.Global

namespace Models;

public interface IWeatherState
{
    public IEnumerable<string> Aliases { get; }
    public WeatherState State { get; }
}

public class Cloudy : IWeatherState
{
    public IEnumerable<string> Aliases
    {
        get
        {
            yield return "Cloudy";
            yield return "VeryCloudy";
        }
    }

    public WeatherState State => WeatherState.Cloudy;
}


public class Foggy : IWeatherState
{
    public IEnumerable<string> Aliases
    {
        get
        {
            yield return "Fog";
            yield return "Mist";
            yield return "FreezingFog";
        }
    }

    public WeatherState State => WeatherState.Foggy;
}

// [Export(typeof(IWeatherState))]
// [ExportMetadata(nameof(State), WeatherState.Overcast)]
// [ExportMetadata(nameof(Aliases), new[] { "Overcast" })]
public class Overcast : IWeatherState
{
    public IEnumerable<string> Aliases
    {
        get { yield return "Overcast"; }
    }

    public WeatherState State => WeatherState.Overcast;
}

// [Export(typeof(IWeatherState))]
// [ExportMetadata(nameof(State), WeatherState.PartlyCloudy)]
// [ExportMetadata(nameof(Aliases), new[] { "PartlyCloudy" })]
public class PartlyCloudy : IWeatherState
{
    public IEnumerable<string> Aliases
    {
        get { yield return "PartlyCloudy"; }
    }

    public WeatherState State => WeatherState.PartlyCloudy;
}

// [Export(typeof(IWeatherState))]
// [ExportMetadata(nameof(State), WeatherState.Raining)]
// [ExportMetadata(nameof(Aliases),
//     new[]
//     {
//         "LightShowers", "LightSleetShowers", "LightRain", "HeavyShowers", "HeavyRain", "LightRainShower",
//         "PatchyRainPossible", "PatchyFreezingDrizzlePossible", "PatchyLightDrizzle", "LightDrizzle", "PatchyLightRain",
//         "ModerateRainAtTimes", "ModerateRain", "HeavyRainAtTimes", "LightFreezingRain", "ModerateOrHeavyRainShower"
//     })]
public class Raining : IWeatherState
{
    public IEnumerable<string> Aliases
    {
        get
        {
            yield return "LightShowers";
            yield return "LightSleetShowers";
            yield return "LightRain";
            yield return "HeavyShowers";
            yield return "HeavyRain";
            yield return "LightRainShower";
            yield return "PatchyRainPossible";
            yield return "PatchyFreezingDrizzlePossible";
            yield return "PatchyLightDrizzle";
            yield return "LightDrizzle";
            yield return "PatchyLightRain";
            yield return "ModerateRainAtTimes";
            yield return "ModerateRain";
            yield return "HeavyRainAtTimes";
            yield return "LightFreezingRain";
            yield return "ModerateOrHeavyRainShower";
        }
    }

    public WeatherState State => WeatherState.Raining;
}

// [Export(typeof(IWeatherState))]
// [ExportMetadata(nameof(State), WeatherState.Snowy)]
// [ExportMetadata(nameof(Aliases),
//     new[]
//     {
//         "LightSleet", "LightSnow", "HeavySnow", "HeavySnowShowers", "PatchySnowPossible", "PatchySleetPossible",
//         "BlowingSnow", "Blizzard", "FreezingDrizzle", "HeavyFreezingDrizzle"
//     })]
public class Snowy : IWeatherState
{
    public IEnumerable<string> Aliases
    {
        get
        {
            yield return "LightSleet";
            yield return "LightSnow";
            yield return "HeavySnow";
            yield return "HeavySnowShowers";
            yield return "PatchySnowPossible";
            yield return "PatchySleetPossible";
            yield return "BlowingSnow";
            yield return "Blizzard";
            yield return "FreezingDrizzle";
            yield return "HeavyFreezingDrizzle";
        }
    }

    public WeatherState State => WeatherState.Snowy;
}

// [Export(typeof(IWeatherState))]
// [ExportMetadata(nameof(State), WeatherState.Sunshine)]
// [ExportMetadata(nameof(Aliases), new[] { "Sunny", "Clear" })]
public class Sunshine : IWeatherState
{
    public IEnumerable<string> Aliases
    {
        get
        {
            yield return "Sunny";
            yield return "Clear";
        }
    }

    public WeatherState State => WeatherState.Sunshine;
}

// [Export(typeof(IWeatherState))]
// [ExportMetadata(nameof(State), WeatherState.Thunder)]
// [ExportMetadata(nameof(Aliases),
//     new[]
//     {
//         "ThunderyShowers", "ThunderyHeavyRain", "ThunderySnowShowers", "ThunderyOutbreaksPossible",
//         "LightRainWithThunderstorm", "Thunderstorm", "RainAndHailWithThunderstorm", "RainWithThunderStorm"
//     })]
public class Thunder : IWeatherState
{
    public IEnumerable<string> Aliases
    {
        get
        {
            yield return "ThunderyShowers";
            yield return "ThunderyHeavyRain";
            yield return "ThunderySnowShowers";
            yield return "ThunderyOutbreaksPossible";
            yield return "LightRainWithThunderstorm";
            yield return "Thunderstorm";
            yield return "RainAndHailWithThunderstorm";
            yield return "RainWithThunderstorm";
        }
    }

    public WeatherState State => WeatherState.Thunder;
}