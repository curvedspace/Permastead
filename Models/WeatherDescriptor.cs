using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Models;

public class WeatherDescriptorJsonConverter : Newtonsoft.Json.JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer) =>
        throw new NotImplementedException();

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        Newtonsoft.Json.JsonSerializer serializer)
    {
        var mainToken = JObject.Load(reader);

        var humidity = mainToken.SelectToken("current_condition[0].humidity")?.Value<int>() ??
                       throw new JsonSerializationException("No humidity property");
        var temperature = mainToken.SelectToken("current_condition[0].temp_C")?.Value<int>() ??
                          throw new JsonSerializationException("No temperature property");
        var weatherState = mainToken.SelectToken("current_condition[0].weatherDesc[0].value")?.Value<string>() ??
                           throw new JsonSerializationException("No weather state property");
        var feelTemperuate = mainToken.SelectToken("current_condition[0].FeelsLikeC")?.Value<int>() ??
                             throw new JsonSerializationException("No FeelsLikeC property");
        var pressure = mainToken.SelectToken("current_condition[0].pressureInches")?.Value<int>() ??
                       throw new JsonSerializationException("No pressureInches property");
        var visibilityMiles = mainToken.SelectToken("current_condition[0].visibilityMiles")?.Value<int>() ??
                              throw new JsonSerializationException("No visibilityMiles property");
        var wind = mainToken.SelectToken("current_condition[0].windspeedKmph")?.Value<int>() ??
                   throw new JsonSerializationException("No windspeedKmph property");
        var uvIndex = mainToken.SelectToken("current_condition[0].uvIndex")?.Value<int>() ??
                      throw new JsonSerializationException("No uvIndex property");
        
        var obsTime = mainToken.SelectToken("current_condition[0].localObsDateTime")?.Value<DateTime>() ??
                      throw new JsonSerializationException("No observation time property");

        //ContextManager.Context.Logger.Info("Recieve weather state: " + weatherState);

        return new WeatherDescriptor
        {
            Humidity = humidity,
            Temperature = temperature,
            FeelTemperature = feelTemperuate,
            Pressure = pressure,
            Visibility = visibilityMiles,
            Wind = wind,
            UvIndex = uvIndex,
            WeatherStateAlias = weatherState,
            ObservationTime = obsTime
        };
    }

    public override bool CanConvert(Type objectType)
        => objectType == typeof(WeatherDescriptor);
}

[Newtonsoft.Json.JsonConverter(typeof(WeatherDescriptorJsonConverter))]
public class WeatherDescriptor
{
    public int Temperature { get; init; }
    public int FeelTemperature { get; init; }
    public int Humidity { get; init; }
    public int Pressure { get; init; }
    public int Visibility { get; init; }
    public int Wind { get; init; }
    public int UvIndex { get; init; }
    public string WeatherStateAlias { get; init; }
    
    public DateTime ObservationTime { get; init; }
}