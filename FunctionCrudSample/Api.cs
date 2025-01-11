using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace FunctionCrudSample;

public class Api
{
    private const string cosmosDatabaseName = "example";
    private const string cosmosContainerName = "documents";
    private const string cosmosConnectionString = "CosmosDBConnection";

    [Function(nameof(Create))]
    public async Task<CreateResponse> Create(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
        [FromBody] MyDocument document,
        FunctionContext executionContext
    )
    {
        var logger = executionContext.GetLogger(nameof(Api));
        logger.LogInformation("C# HTTP trigger function processed a request.");
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(document);
        return new() { Response = response, Document = document };
    }

    public class CreateResponse
    {
        [HttpResult]
        public required HttpResponseData Response { get; set; }

        [CosmosDBOutput(cosmosDatabaseName, cosmosContainerName, Connection = cosmosConnectionString, CreateIfNotExists = true, PartitionKey = "/id")]
        public MyDocument? Document { get; set; }
    }

    public class MyDocument
    {
        public string? Id { get; set; }
        public string? Message { get; set; }
    }
}
