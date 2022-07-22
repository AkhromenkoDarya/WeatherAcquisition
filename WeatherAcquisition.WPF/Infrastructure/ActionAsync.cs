using System.Threading.Tasks;

namespace WeatherAcquisition.WPF.Infrastructure
{
    internal delegate Task ActionAsync();

    internal delegate Task ActionAsync<in T>(T parameter);
}
