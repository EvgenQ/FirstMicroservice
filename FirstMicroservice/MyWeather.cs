namespace FirstMicroservice
{
    public class MyWeather
    {
        public int TemperatureC { get; set; }
        public DateTime Date { get; set; }
        
        public void Add(int temperatureC, int year, int mounth, int day)
        {
            TemperatureC = temperatureC;
            Date = new DateTime(year, mounth, day);
        }
    }
}
