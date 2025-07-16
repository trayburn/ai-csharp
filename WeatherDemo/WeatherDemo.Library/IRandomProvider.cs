namespace WeatherDemo.Library
{
    public interface IRandomProvider
    {
        int Next(int minValue, int maxValue);
    }
}
