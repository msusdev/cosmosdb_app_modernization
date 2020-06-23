# The Azure Cosmos DB Workshop

## Overview

Welcome to the Azure Cosmos DB hands-on workshop! Throughout this workshop, you will go through tasks to take a reasonably modern web application, migrate it to [Azure Cosmos DB](https://azure.microsoft.com/services/cosmos-db/), and implement features to make the application better than ever.

After you are done with the workshop, you can continue to use the content available in our [GitHub repository](https://github.com/msusdev/cosmosdb_app_modernization) to learn about Azure Cosmos DB at your own pace.

> [Schedule](.\schedule\readme.md)

## Goals

In this workshop, you will be asked to:

- Migrate a small dataset from Azure SQL Database to Azure Cosmos DB.
- Write code in Azure Functions to query Azure Cosmos DB.
- Globally distribute your Azure Cosmos DB account to multiple Azure regions.
- Extend the SQL query grammar using a user-defined function (UDF).
- Use Jupyter Notebooks to create a simple dashboard for your database and containers.

## Challenge 1: Deploy the existing application stack

For your first challenge, you will take the existing application stack and deploy it to your Azure subscription.

❗ It may be tempting to rush through this challenge, but taking your time here to learn the environment will help make any future challenges easier to complete.

### Step A: Deploy with an ARM template

The application stack consists of the following components:

- Azure Web App hosting the application website
  - The application code is deployed using the [msusdev/contoso.spaces.web](https://hub.docker.com/r/msusdev/contoso.spaces.web) container.
- Azure Functions to host the back-end APIs that power the application
  - ❗ Any code you modify in this workshop will only happen here.
  - This function app has the [msusdev/contoso.spaces.api.sql](https://hub.docker.com/r/msusdev/contoso.spaces.api.sql) container deployed by default.
- Azure SQL Database and Server for the application data.
- Azure Storage account to host the images used on the website and Azure Functions metadata
- Azure Container Instance to populate the Azure SQL Database and Azure Storage accounts with sample data
  - The [msusdev/contoso.spaces.populate.sql](https://hub.docker.com/r/msusdev/contoso.spaces.populate.sql) container is deployed to the container group.

![Existing application architecture](./media/01-arch.png)

Fortunately, you can deploy the application to your Azure subscription using an ARM template:

[![Deploy to Azure](https://docs.microsoft.com/en-us/azure/templates/media/deploy-to-azure.svg)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FMSUSDEV%2Fcosmosdb_app_modernization%2Fmaster%2Farmdeploy.json)

The deployment should take about **10 minutes**.

While the template is deploying, you should check out some of the deployment details from the Azure Resource Manager (ARM) template:

- The username and password for the SQL database are stored as variables in the template: [armdeploy.json#13](https://github.com/MSUSDEV/cosmosdb_app_modernization/blob/fb8685af8a93301801d9f612b35c9dd791de2d79/armdeploy.json#L13)
- The template also contains variables that points to the Docker container images on [Docker Hub](https://hub.docker.com/r/msusdev): [armdeploy.json#15](https://github.com/MSUSDEV/cosmosdb_app_modernization/blob/fb8685af8a93301801d9f612b35c9dd791de2d79/armdeploy.json#L15)
- The template deploys Azure Container Instances to seed the Azure Storage and Azure Cosmos DB accounts with data: [armdeploy.json#91](https://github.com/MSUSDEV/cosmosdb_app_modernization/blob/fb8685af8a93301801d9f612b35c9dd791de2d79/armdeploy.json#L91)
- The Azure Function created by the template has a connection string to the SQL Database: [armdeploy.json#188](https://github.com/MSUSDEV/cosmosdb_app_modernization/blob/fb8685af8a93301801d9f612b35c9dd791de2d79/armdeploy.json#L188)
- The Azure Web App created by the template has multiple configuration settings to point to HTTP APIs hosted on Azure Functions: [armdeploy.json#252](https://github.com/MSUSDEV/cosmosdb_app_modernization/blob/fb8685af8a93301801d9f612b35c9dd791de2d79/armdeploy.json#L252)

### Validation: Review the deployment

1. Browse to the URL for the Azure Web App created by the ARM template deployment. If you have deployed the website successfully, you should see the following homepage:

![Contoso Spaces homepage](./media/01-validation.png)

> [Hints](./hints/01-deploy/)
