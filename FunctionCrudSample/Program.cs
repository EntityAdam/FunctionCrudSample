using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var host = new HostBuilder().ConfigureFunctionsWorkerDefaults(options =>
{
    options.Services.AddOptions<CosmosDBExtensionOptions>().Configure<IOptions<WorkerOptions>>((cosmos, worker) =>
    {
        cosmos.ClientOptions.SerializerOptions.PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase;
    });

    options.ConfigureCosmosDBExtensionOptions(options =>
    {
        options.ClientOptions.SerializerOptions.PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase;
    });

}).Build();

await host.RunAsync();