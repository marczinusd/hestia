using System.Threading.Tasks;

namespace Hestia.ConsoleRunner.Configuration
{
    public interface IJsonConfigurationProvider
    {
        Task<ConsoleRunnerConfig> LoadConfiguration(string jsonPath);
    }
}
