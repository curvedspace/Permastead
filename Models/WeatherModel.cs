namespace Models;

public class WeatherModel
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AreaName
    {
        public string value;
    }

    public class Astronomy
    {
        public string moon_illumination;
        public string moon_phase;
        public string moonrise;
        public string moonset;
        public string sunrise;
        public string sunset;
    }

    public class Country
    {
        public string value;
    }

    public class CurrentCondition
    {
        public string FeelsLikeC;
        public string FeelsLikeF;
        public string cloudcover;
        public string humidity;
        public string localObsDateTime;
        public string observation_time;
        public string precipInches;
        public string precipMM;
        public string pressure;
        public string pressureInches;
        public string temp_C;
        public string temp_F;
        public string uvIndex;
        public string visibility;
        public string visibilityMiles;
        public string weatherCode;
        public List<WeatherDesc> weatherDesc;
        public List<WeatherIconUrl> weatherIconUrl;
        public string winddir16Point;
        public string winddirDegree;
        public string windspeedKmph;
        public string windspeedMiles;
    }

    public class Hourly
    {
        public string DewPointC;
        public string DewPointF;
        public string FeelsLikeC;
        public string FeelsLikeF;
        public string HeatIndexC;
        public string HeatIndexF;
        public string WindChillC;
        public string WindChillF;
        public string WindGustKmph;
        public string WindGustMiles;
        public string chanceoffog;
        public string chanceoffrost;
        public string chanceofhightemp;
        public string chanceofovercast;
        public string chanceofrain;
        public string chanceofremdry;
        public string chanceofsnow;
        public string chanceofsunshine;
        public string chanceofthunder;
        public string chanceofwindy;
        public string cloudcover;
        public string diffRad;
        public string humidity;
        public string precipInches;
        public string precipMM;
        public string pressure;
        public string pressureInches;
        public string shortRad;
        public string tempC;
        public string tempF;
        public string time;
        public string uvIndex;
        public string visibility;
        public string visibilityMiles;
        public string weatherCode;
        public List<WeatherDesc> weatherDesc;
        public List<WeatherIconUrl> weatherIconUrl;
        public string winddir16Point;
        public string winddirDegree;
        public string windspeedKmph;
        public string windspeedMiles;
    }

    public class NearestArea
    {
        public List<AreaName> areaName;
        public List<Country> country;
        public string latitude;
        public string longitude;
        public string population;
        public List<Region> region;
        public List<WeatherUrl> weatherUrl;
    }

    public class Region
    {
        public string value;
    }

    public class Request
    {
        public string query;
        public string type;
    }

    public class Root
    {
        public List<CurrentCondition> current_condition;
        public List<NearestArea> nearest_area;
        public List<Request> request;
        public List<Weather> weather;
    }

    public class Weather
    {
        public List<Astronomy> astronomy;
        public string avgtempC;
        public string avgtempF;
        public string date;
        public List<Hourly> hourly;
        public string maxtempC;
        public string maxtempF;
        public string mintempC;
        public string mintempF;
        public string sunHour;
        public string totalSnow_cm;
        public string uvIndex;
    }

    public class WeatherDesc
    {
        public string value;
    }

    public class WeatherIconUrl
    {
        public string value;
    }

    public class WeatherUrl
    {
        public string value;
    }


}