using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyWeatherController : ControllerBase
    {
        private static List<MyWeather> _weatherList = new List<MyWeather>();
        private readonly MyWeather _myWeather;
        public MyWeatherController(MyWeather myWeather)
        {
            _myWeather = myWeather;
        }
        [HttpGet("Read")]
        public string Read()
        {
            string list=string.Empty;
            if (_weatherList.Count == 0)
            {
                return "Нет данных о погоде.";
            }
            else
            {
                foreach (var item in _weatherList)
                {
                    list+= $"Температура в цельсиях: {item.TemperatureC}\n" +
                           $"Дата: {item.Date.ToShortDateString()}\n";
                }
            }
            return list;
        }
        [HttpGet("GetFilterList/{fromYear}/{fromMounth}/{fromDay}/To/{toYear}/{toMounth}/{toDay}")]
        public IActionResult GetDataWeather([FromRoute] int fromYear, [FromRoute] int fromMounth, [FromRoute] int fromDay,
                                                  [FromRoute] int toYear, [FromRoute] int toMounth, [FromRoute] int toDay)
        {
            DateTime fromDate = new DateTime(fromYear, fromMounth, fromDay);
            DateTime toDate = new DateTime(toYear, toMounth, toDay);
            List<MyWeather> list = new List<MyWeather>();
            int day = _weatherList.Count;
            string listX = string.Empty;
            for (int i = 0; i < day; i++)
            {
                if (_weatherList[i].Date >= fromDate && _weatherList[i].Date <= toDate)
                {
                    list.Add(_weatherList[i]);

                }
            }
            foreach (var item in list)
            {
                listX += $"Температура в цельсиях: {item.TemperatureC}\n" +
                       $"Дата: {item.Date.ToShortDateString()}\n";
            }
            return Ok(listX);
        }
        [HttpPost("Create/{temperatureC}/{year}/{mounth}/{day}")]
        public IActionResult CreateFromRoute([FromRoute] int temperatureC,[FromRoute] int year, [FromRoute] int mounth, [FromRoute] int day)
        {
            string xxx;
            _myWeather.Add(temperatureC, year, mounth, day);
            _weatherList.Add(_myWeather);
            xxx = $"Новые данные добавленны\n" +
                $"Температура в цельсиях: {temperatureC}\n" +
                $"Дата: {new DateTime(year,mounth,day).ToShortDateString()}\n";
            return Ok(xxx);
        }
        [HttpPost("CreatFromQuerye")]
        public IActionResult CreateFromQuery([FromQuery] int temperatureC, [FromQuery] int year, [FromQuery] int mounth, [FromQuery] int day)
        {
            string xxx;
            _myWeather.Add(temperatureC, year, mounth, day);
            _weatherList.Add(_myWeather);
            xxx = $"Новые данные добавленны\n" +
                $"Температура в цельсиях: {temperatureC}\n" +
                $"Дата: {new DateTime(year, mounth, day).ToShortDateString()}\n";
            return Ok(xxx);
        }
        [HttpPost("CreateFromBody")]
        public IActionResult CreateFromBody([FromBody] MyWeather weather)
        {
            string xxx;
            _weatherList.Add(weather);
            xxx = $"Новые данные добавленны\n" +
                $"Температура в цельсиях: {weather.TemperatureC}\n" +
                $"Дата: {weather.Date.ToShortDateString()}\n";
            return Ok(xxx);
        }
        [HttpPost("FillRandom")]
        public string FillRandom()
        {
            int year = 2021;
            int mounth;
            int day;

            Random random = new Random();
            for (int j = 1; j <= 12; j++)
            {
                mounth = j;
                for (int k = 1; k <= 31; k++)
                {
                    day = k;
                    // Костыль
                    try
                    {
                        _weatherList.Add(new MyWeather()
                        {
                            Date = new DateTime(year, mounth, day),
                            TemperatureC = random.Next(-30, 31)
                        });
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
            }
            return $"Новые данные добавленны";
        }
        [HttpDelete("Delete/{year}/{mounth}/{day}")]
        public IActionResult Delete([FromRoute] int year, [FromRoute] int mounth, [FromRoute] int day)
        {
            DateTime deleteDate = new DateTime(year, mounth, day);
            MyWeather my = _weatherList.FirstOrDefault(x => x.Date == deleteDate);
            if (my != null)
            {
                _weatherList.Remove(my);
                return Ok("Данные удалены");
            }
            return Ok("Данные не найдены");
        }
        [HttpDelete("DeleteFromDateToDate/{fromYear}/{fromMounth}/{fromDay}/To/{toYear}/{toMounth}/{toDay}")]
        public IActionResult DeleteStartDateEndDate([FromRoute] int fromYear, [FromRoute] int fromMounth, [FromRoute] int fromDay,
                                                  [FromRoute] int toYear, [FromRoute] int toMounth, [FromRoute] int toDay)
        {
            DateTime fromDate = new DateTime(fromYear, fromMounth, fromDay);
            DateTime toDate = new DateTime(toYear, toMounth, toDay);
            List<MyWeather> list = new List<MyWeather>(); 
           int day = _weatherList.Count;
            for (int i = 0; i < day; i++)
            {
                if (!(_weatherList[i].Date >= fromDate && _weatherList[i].Date <= toDate))
                {
                    list.Add(_weatherList[i]);

                }
            }
            _weatherList = list;
            return Ok("Данные удалены");
        }
        [HttpPut("UpdateDataWeather")]
        public IActionResult UpdateDataWeather([FromBody] MyWeather oldData,[FromQuery] int newData)
        {
            for (int i = 0; i < _weatherList.Count; i++)
            {
                if(_weatherList[i].Date == oldData.Date)
                {
                    _weatherList[i].TemperatureC = newData;
                }
            }
            return Ok("Данные изменены");
        }
    }
}
