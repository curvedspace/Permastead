using Avalonia.Media.Imaging;
using Models;
using Newtonsoft.Json;

namespace Services;

public interface IService
{
}

public interface ICityService : IService
{
    public Task<IReadOnlyDictionary<(string, string), City>> CreateAndFillCitiesDictionary();
}

public interface IWeatherService : IService
{
    public Task<WeatherDescriptor> UpdateWeather(City city);
}

public interface IImageService : IService
{
    public Task<Bitmap?> SaveImage(string? path, City city);
}

public class CityService : ICityService
{
    private const string FilePath = "Assets/csv/worldcities.csv";

    public async Task<IReadOnlyDictionary<(string, string), City>> CreateAndFillCitiesDictionary()
    {
        const int cityNameKey = 0;
        const int idKey = 10;

        if (!File.Exists(FilePath))
        {
            throw new FileNotFoundException($"File with path \"{FilePath}\" doesnt exist");
        }

        var dictionary = new Dictionary<(string, string), City>();

        using var sr = new StreamReader(FilePath);

        while (!sr.EndOfStream)
        {
            var line = await sr.ReadLineAsync();

            if (line is null)
            {
                throw new ArgumentNullException(nameof(line), "Error with reading line from file");
            }

            var lines = line.Split(',');

            dictionary[(lines[cityNameKey], lines[idKey])] = City.Parse(line);
        }

        //ContextManager.Context.Logger.Info("The dictionary with the cities filled up");
        return dictionary;
    }
}

public class WeatherService : IWeatherService, IDisposable
{
    private const string Url = @"https://wttr.in/";
    private readonly HttpClient _client = new();

    public async Task<WeatherDescriptor> UpdateWeather(City city)
    {
        var url = Path.Combine(Url, city.Name + "," + city.Country + "?format=j1");

        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        //ContextManager.Context.Logger.Info("Command update image is executed");
        Console.WriteLine(json);
        
        return JsonConvert.DeserializeObject<WeatherDescriptor>(json) ??
               throw new JsonSerializationException("Bad deserialization weather description");
    }

    public void Dispose() => _client.Dispose();
}

public class ImageService : IImageService, IDisposable
{
    private const string Url = @"https://wttr.in/";
    private const string ImageFormat = ".png";

    private readonly HttpClient _client = new();

    public async Task<Bitmap?> SaveImage(string? path, City city)
    {
        var imageUrl = Path.Combine(Url, city.Name + ImageFormat);

        var response = await _client.GetAsync(imageUrl);
        response.EnsureSuccessStatusCode();

        await using var imageStream = await response.Content.ReadAsStreamAsync();

        if (path is null) return new(imageStream);
        await using (var fileStream = File.Create(path))
        {
            await imageStream.CopyToAsync(fileStream);
        }

        //ContextManager.Context.Logger.Info($"Image saved to path \"{path}\"");
        return null;
    }

    public void Dispose() => _client.Dispose();

}