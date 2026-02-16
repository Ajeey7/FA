using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace WeatherFunctionApp.SimpleWeather
{
    public class SimpleWeather
    {
        [Function("SimpleWeather")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string city = query["city"] ?? "Unknown";

            // Simulated weather data
            var weatherData = GetWeatherData(city);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            
            string jsonResponse = System.Text.Json.JsonSerializer.Serialize(weatherData);
            response.WriteString(jsonResponse);

            return response;
        }

        private object GetWeatherData(string city)
        {
            // Simple mock weather data
            var weatherData = new Dictionary<string, string>
            {
                { "Stockholm", "5°C,Molnigt,☁️" },
                { "Göteborg", "3°C,Regn,🌧️" },
                { "Malmö", "7°C,Soligt,☀️" },
                { "Uppsala", "4°C,Snö,❄️" },
                { "Västerås", "2°C,Dimma,🌫️" }
            };

            if (weatherData.ContainsKey(city))
            {
                var data = weatherData[city].Split(',');
                return new { city = city, temperature = data[0], condition = data[1], emoji = data[2] };
            }

            return new { city = city, temperature = "N/A", condition = "Okänd stad", emoji = "❓" };
        }
    }
}
