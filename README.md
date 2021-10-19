# FeatureApp
A simple feature toggle api that provides feature toggle based on creator of the feature.

# Introduction
Feature toggle api is developed using .NET Core 3.1 with Web API design and hosted in Microsoft Azure. It stores/updates/fetches feature data on Azure CosmosDB for data persistance. 

# Pre-requisite
1. [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1) 
2. Visual Studio 2019/Code for Code editor

# Tech stack
1. `Microsoft Azure` - To host application in the cloud
2. `Azure App Service` - To host .NET Web API that made publically accessible in Windows environment
3. `Azure CosmosDB` - To stores/updates/fetches feature data on Azure CosmosDB for data persistance with minimum 400 RU
4. `.NET Core 3.1` - Web host to initialize .NET Web API

# Documentation
Feature app comes with Swagger based UI where you are able to use it as playground to test manually.
https://featureapp.azurewebsites.net/swagger/index.html

# Testing
You can run `Acceptance.Tests` that are available in the code repository which show cases the coverage to the feature app api.

# Deployment
I've used Visual Studio published profiles which you can directly deploy to Azure App Service instance locally using SSO. In real-world environment, we can configure CD to deploy the ARM template designed in `ServiceDependancies/FeatureApp - Web Deploy/profile.arm.json`.

# Important Note
1. You will not be able to run `Api.Tests` because I've excluded Azure Cosmos DB secret key(`CosmosDb.Key`) to be checked into repository. When CI to be configured, we can make changes to read from environment variables.
2. `CosmosRepository.cs` best to be tested with actual Azure CosmosDB instance because its a simple DAL which get's real confidence on the testing.
