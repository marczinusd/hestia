using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NJsonSchema;

namespace Hestia.ConsoleRunner.Configuration
{
    [ExcludeFromCodeCoverage]
    public class JsonConfigurationProvider : IJsonConfigurationProvider
    {
        public async Task<ConsoleRunnerConfig> LoadConfiguration(string jsonPath)
        {
            var schema = await JsonSchema.FromJsonAsync(File.ReadAllText("Repository.schema.json"));
            var config = File.ReadAllText(jsonPath);
            schema.Validate(config);

            return JsonConvert.DeserializeObject<ConsoleRunnerConfig>(config);
        }
    }
}
