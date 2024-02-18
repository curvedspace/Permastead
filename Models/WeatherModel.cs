using System.Reflection;

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
        public string DewPointC { get; set; }
        public string DewPointF { get; set; }
        public string FeelsLikeC { get; set; }
        public string FeelsLikeF { get; set; }
        public string HeatIndexC { get; set; }
        public string HeatIndexF { get; set; }
        public string WindChillC { get; set; }
        public string WindChillF { get; set; }
        public string WindGustKmph { get; set; }
        public string WindGustMiles { get; set; }
        public string chanceoffog { get; set; }
        public string chanceoffrost { get; set; }
        public string chanceofhightemp { get; set; }
        public string chanceofovercast { get; set; }
        public string chanceofrain { get; set; }
        public string chanceofremdry { get; set; }
        public string chanceofsnow { get; set; }
        public string chanceofsunshine { get; set; }
        public string chanceofthunder { get; set; }
        public string chanceofwindy { get; set; }
        public string cloudcover { get; set; }
        public string diffRad { get; set; }
        public string humidity { get; set; }
        public string precipInches { get; set; }
        public string precipMM { get; set; }
        public string pressure { get; set; }
        public string pressureInches { get; set; }
        public string shortRad { get; set; }
        public string tempC { get; set; }
        public string tempF { get; set; }
        public string time { get; set; }
        public string uvIndex { get; set; }
        public string visibility { get; set; }
        public string visibilityMiles { get; set; }
        public string weatherCode { get; set; }
        public List<WeatherDesc> weatherDesc { get; set; }
        public List<WeatherIconUrl> weatherIconUrl { get; set; }
        public string winddir16Point { get; set; }
        public string winddirDegree { get; set; }
        public string windspeedKmph { get; set; }
        public string windspeedMiles { get; set; }

        public string DisplayTime
        {
            get
            {
                switch (time.Length)
                {
                    case 0:
                        return "";
                        break;
                    case 1:
                        return "000" + time;
                        break;
                    case 3:
                        return "0" + time;
                        break;
                    case 4:
                        return time;
                        break;
                    default:
                        return "";
                        break;
                }
            }
        }
        
        public string Conditions
        {
            get
            {
                if (weatherDesc.Count > 0)
                {
                    return weatherDesc[0].value;
                }
                else
                {
                    return "";
                }
            }
        }
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
        public List<Astronomy> astronomy { get; set; }
        public string avgtempC { get; set; } 
        public string avgtempF { get; set; }
        public string date { get; set; }
        public List<Hourly> hourly { get; set; }
        public string maxtempC { get; set; }
        public string maxtempF { get; set; }
        public string mintempC { get; set; }
        public string mintempF { get; set; }
        public string sunHour { get; set; }
        public string totalSnow_cm { get; set; }
        public string uvIndex { get; set; }
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