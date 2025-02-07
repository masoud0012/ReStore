using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class WeatherForecast
    {
         public DateOnly Date { get; set; }
         public double TemperatureC { get; set; }
         public string? Summary { get; set; }
         public int TemperatureF=>32+(int)(TemperatureC/0.5556);
    }
}