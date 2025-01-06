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

    // -------------------------------------------------------- WORKS ------------------------------------------------- //
    // See Tests/createCamelCase.http for example POST request
    [Function(nameof(CreateCamelCase))]
    public async Task<CreateResponseCamelCase> CreateCamelCase(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
        [FromBody] MyDocumentCamelCase myDocument,
        FunctionContext executionContext
    )
    {
        var logger = executionContext.GetLogger(nameof(Api));
        logger.LogInformation("C# HTTP trigger function processed a request.");
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(myDocument);
        return new() { Response = response, Document = myDocument };
    }

    public class CreateResponseCamelCase
    {
        [HttpResult]
        public required HttpResponseData Response { get; set; }

        [CosmosDBOutput(cosmosDbName, cosmosContainerName, Connection = connectionString, CreateIfNotExists = true, PartitionKey = "/id")]
        public MyDocumentCamelCase? Document { get; set; }
    }

    public class MyDocumentCamelCase
    {
        public string? id { get; set; }
        public string? message { get; set; }
    }

    // -------------------------------------------------------- DOES NOT WORK ------------------------------------------------- //
    // See Tests/createPascalCase.http for example POST request
    [Function(nameof(CreatePascalCase))]
    public async Task<CreateResponsePascalCase> CreatePascalCase(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
        [FromBody] MyDocumentPascalCase myDocument,
        FunctionContext executionContext
    )
    {
        var logger = executionContext.GetLogger(nameof(Api));
        logger.LogInformation("C# HTTP trigger function processed a request.");
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(myDocument);
        return new() { Response = response, Document = myDocument };
    }

    public class CreateResponsePascalCase
    {
        [HttpResult]
        public required HttpResponseData Response { get; set; }

        [CosmosDBOutput(cosmosDbName, cosmosContainerName, Connection = connectionString, CreateIfNotExists = true, PartitionKey = "/id")]
        public MyDocumentPascalCase? Document { get; set; }
    }

    public class MyDocumentPascalCase
    {
        public string? Id { get; set; }
        public string? Message { get; set; }
    }
}
