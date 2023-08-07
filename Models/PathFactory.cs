namespace Models;

public static class PathFactory
{
    public static (string BackgroundPath, string ForegroundPath) GetImagePath(WeatherState state) =>
        state switch
        {
            WeatherState.Cloudy => (@"/Assets/weather-backgrounds/cloudyBackground.png",
                @"/Assets/weather-states/cloudy.svg"),
            WeatherState.Foggy => (@"/Assets/weather-backgrounds/foggyBackground.png",
                @"/Assets/weather-states/foggy.svg"),
            WeatherState.Raining => (@"/Assets/weather-backgrounds/rainingBackground.png",
                @"/Assets/weather-states/raining.svg"),
            WeatherState.Snowy => (@"/Assets/weather-backgrounds/snowyBackground.png",
                @"/Assets/weather-states/snowy.svg"),
            WeatherState.Sunshine => (@"/Assets/weather-backgrounds/sunshineBackground.png",
                @"/Assets/weather-states/sunshine.svg"),
            WeatherState.Thunder => (@"/Assets/weather-backgrounds/thunderBackground.png",
                @"/Assets/weather-states/thunder.svg"),
            WeatherState.PartlyCloudy => (@"/Assets/weather-backgrounds/cloudyBackground.png",
                @"/Assets/weather-states/partlyCloudy.svg"),
            WeatherState.Overcast => (@"/Assets/weather-backgrounds/sunshineBackground.png",
                @"/Assets/weather-states/overcast.svg"),
            _ => throw new ArgumentOutOfRangeException(nameof(state), "Unknown weather state")
        };
}