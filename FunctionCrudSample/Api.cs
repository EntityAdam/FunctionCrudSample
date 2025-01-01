using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace FunctionCrudSample;

public class Api
{
    private const string cosmosDbName = "example";
    private const string cosmosContainerName = "documents";
    private const string connectionString = "CosmosDBConnection";

    [Function(nameof(Create))]
    public async Task<CreateResponse> Create(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
        [FromBody] MyDocument myDocument,
        FunctionContext executionContext
        )
    {
        var logger = executionContext.GetLogger(nameof(Api));
        logger.LogInformation("C# HTTP trigger function processed a request.");
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(myDocument);
        return new() { Response = response, Document = myDocument };
    }

    public class CreateResponse
    {
        [HttpResult]
        public required HttpResponseData Response { get; set; }

        [CosmosDBOutput(cosmosDbName, cosmosContainerName, Connection = connectionString, CreateIfNotExists = true, PartitionKey = "/id")]
        public MyDocument? Document { get; set; }
    }

    public class MyDocument
    {
        public string? id { get; set; }
        public string? message { get; set; }
    }
}
