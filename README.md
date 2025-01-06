# FunctionCrudSample

Sample functions application.

This sample is using:
- .NET 8  
- Azure Functions V4 isolated  
- CosmosDb Function input and output bindings  

This does not have AspNetCore integration enabled.

# Prerequisites
- .NET 8  
- Cosmos Db emulator or CosmosDb instance  
- Azurite Storage Emulator  

# To start local

## local.settings.json

You should create a file named `local.settings.json` with the following configuration to specify the worker runtime of "dotnet-isolate" as well as using the Azureite storage emulator for the functions host to work.

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
  }
}
```

## CosmosDb ConnectionString
This project is using `dotnet user-secrets`, so you may set the connection string with the expected key of `CosmosDBConnection` using the following:  

```sh
dotnet user-secrets init
# This connection string is a well-known AccountKey for CosmosDB emulator
dotnet user-secrets set "CosmosDBConnection" "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
```

Or you may set your connection string in `local.settings.json`  

```js
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "CosmosDBConnection": "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
  }
}
```

## Run
In Visual Studio, use F5 or run to start the application with debugging.  

Once the function host starts you will see the index of the functions in the terminal

```sh
Functions:
        CreateCamelCase: [POST] http://localhost:7147/api/CreateCamelCase
        CreatePascalCase: [POST] http://localhost:7147/api/CreatePascalCase
```

## `api/CreateCamelCase` endpoint

In the `Tests` folder I have created a `.http` file with a sample POST to create a 'MyDocumentCamelCase' which is working with camelCase property names.

## `api/CreatePascalCase` endpoint

In the `Tests` folder I have created a `.http` file with a sample POST to create a 'MyDocumentCamelCase' which is NOT working with PascalCase property names.