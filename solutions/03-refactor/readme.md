# Challenge 3: Solution

## Walkthrough

There are many different ways to implement this solution, but we will walk thorugh an example that uses Azure Functions.

### Prep

To get started, delete the existing Function App and replace it with a new Function App in the same resource group. Ideally, you can also reuse the App Service Plan.

Since we are using in-portal code, you should choose to deploy an Application Insights resource to capture telemetry from the Function App for debugging purposes.

### Authoring "All & Featured" Functions

If you are willing to use the Cosmos DB extension in Azure Functions, writing the code for these two functions is very simple and similar.

First, you will need to configure integrations using the **function.json** file. For both functions, your **function.json** would include a HTTP binding for the **trigger**:

```json
{
    "name": "req",
    "authLevel": "anonymous",
    "methods": [
        "get"
    ],
    "direction": "in",
    "type": "httpTrigger",
    "route": "getalllocations"
}
```

The only difference between the triggers in each file is the route. The "all" function would have a route similar to ``getalllocations`` while the "featured" function would have a route similar to ``getfeaturedlocations``.

The output is pretty simple and simply uses the return value of the entry point method:

```json
{
    "name": "$return",
    "type": "http",
    "direction": "out"
}
```

Next, you want to create an **input** binding that uses the **cosmosDB** extension. For the "all" function, this binding would run the ``SELECT * FROM locations`` query:

```json
{
    "type": "cosmosDB",
    "name": "docs",
    "databaseName": "ContosoSpaces",
    "collectionName": "Locations",
    "connectionStringSetting": "CosmosConnectionString",
    "direction": "in",
    "sqlQuery": "SELECT * FROM locations"
}
```

The "featured" function would simply change the SQL query in the binding configuration to ``SELECT TOP 4 * FROM locations l ORDER BY l.lastRenovationDate DESC``:

```json
{
    "type": "cosmosDB",
    "name": "docs",
    "databaseName": "ContosoSpaces",
    "collectionName": "Locations",
    "connectionStringSetting": "CosmosConnectionString",
    "direction": "in",
    "sqlQuery": "SELECT TOP 4 * FROM locations l ORDER BY l.lastRenovationDate DESC"
}
```

Both functions can use the same code to bind ``IEnumerable<dynamic>`` to the result of the query and return it as a **HTTP 200** response with the results set as the response body:

```cs
using Microsoft.AspNetCore.Mvc;
using System.Net;

public static IActionResult Run(HttpRequest req, IEnumerable<dynamic> docs) => new OkObjectResult(docs);
```

### Authoring "Specific" Proxy & Function

The "specific" function requires a little more finesse. The [Cosmos DB binding](https://docs.microsoft.com/azure/azure-functions/functions-bindings-cosmosdb) does not support using query strings with the binding.

First, we need to create a proxy that will take a request to ``api/getspecificlocation?id={id}`` and redirect it to a function using a route parameter (such as ``api/specific/{id}``). This proxy file will create the redirect rule:

```json
{
    "$schema": "http://json.schemastore.org/proxies",
    "proxies": {
        "specificProxy": {
            "matchCondition": {
                "methods": [ "GET" ],
                "route": "/api/getspecificlocation"
            },
            "backendUri": "https://localhost/api/specific/{request.querystring.id}"
        }
    }
}
```

When you configure the "specific" function, you want to take special care to define a route that has ``id`` as a parameter:

```json
{
    "name": "req",
    "authLevel": "anonymous",
    "methods": [
        "get"
    ],
    "direction": "in",
    "type": "httpTrigger",
    "route": "specific/{id}"
}
```

You can then use that route parameter as part of the **cosmosDB** binding to get a specific record. In this example, all of my data is in the same logical partition (to keep things simple):

```json
{
    "type": "cosmosDB",
    "name": "docs",
    "databaseName": "ContosoSpaces",
    "collectionName": "Locations",
    "connectionStringSetting": "CosmosConnectionString",
    "direction": "in",
    "sqlQuery": "SELECT * FROM locations l WHERE l.id = {id}"
}
```

The code for this function is very trivial, it will get the resulting single document. If it's null, return a **404 Not Found**. If it's not null, return a **200** with the document as the body of the response:

```cs
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

public static IActionResult Run(HttpRequest req, IEnumerable<dynamic> docs)
    => docs.SingleOrDefault() is null ? new NotFoundResult() : new OkObjectResult(docs.Single()) as IActionResult;
```

### Source Code

You can [view the Gist](https://gist.github.com/seesharprun/95844afd94209b36a0503af57e6c6992#file-all-function-json) for the Function App .

### Final Configuration

To wrap things up, make sure the application settings in your Web App point to the correct URLs for the functions and proxies on your Function App.

You should check the following three settings:

- ``ConnectionData__GetFeaturedLocationsApiUrl``
- ``ConnectionData__GetAllLocationsApiUrl``
- ``ConnectionData__GetSpecificLocationApiUrl``

## Deployment Template

Use this template to deploy a pre-baked solution to your Azure subscription:

[![Deploy to Azure](https://docs.microsoft.com/en-us/azure/templates/media/deploy-to-azure.svg)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fgithub.com%2FMSUSDEV%2Fcosmosdb_app_modernization%2Fblob%2F03-solution%2Farmdeploy.json)
