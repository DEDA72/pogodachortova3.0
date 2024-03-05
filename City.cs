using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pogodachortova3_0
{
    public class City
    {
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string Latitude { get; set; }                //сначала широта
        public string Longitude { get; set; }             //потом долгота
        public string TemperatureCelsius { get; set; }
        public string TemperatureFahrenheit { get; set; }
        public string WindSpeed { get; set; }
        public string WindDirectionDegrees { get; set; }
        public string WindDirectionCardinal { get; set; }
        public string Pressure { get; set; }
        public string Humidity { get; set; }
        public string WeatherState { get; set; }
        public string LastUpdate { get; set; }

        public City(string name)
        {
            CityName = name;
        }
    }
}
