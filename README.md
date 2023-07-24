# Private Networking with Azure PaaS Services

This sample shows how to use various common Azure PaaS services configured with private networking.  
The application code has simple test pages to check connections to Azure SQL Database and Azure Redis Cache.
The instructions below are for creating Azure PaaS resources and checking the setup with the sample application.
This isn't meant to teach networking concepts, but rather to show how to configure Azure PaaS services with private networking using Private Endpoints.

We'll be using the Azure Portal for most of the steps below, but you can also use the Azure CLI or PowerShell or Infrastructure as Code such as Bicep or Terraform.  I may add some of those examples in the future.  I'd recommend not using Azure Portal for enterprise level production management.

Azure Services used in this sample:
- Azure Virtual Network
- Azure App Service
- Azure SQL Database
- Azure Redis Cache
- Azure Front Door
- Azure Key Vault
- Azure Log Analytics
- Azure Application Insights

## Prerequisites

- Azure subscription - [create one for free](https://azure.microsoft.com/free/)

## Create Resource Group
Resource Groups are a mechanism to group together multiple related Azure resources to simplify maintenance of them. 
We're going to create everything in this walkthrough in a single resource group.  This probably isn't how you would structure it in production but this will give us a good place to find all of our resources and also make cleanup easy.

Instructions:
- [Create Resource Group](docs/create-resource-group.md#create-resource-group)
- [Navigate back to Resource Group](docs/create-resource-group.md#getting-back-to-the-resource-group)

## Create Virtual Network
A virtual network provides a private network for your resources to communicate with each other without going over public/shared networks.  You can connect virtual networks to each other and even to your on premise networks.

Azure PaaS services, like Azure SQL Database, are traditionally accessed via public networking.  We will be joining Azure PaaS services to the private virtual networks with Private Endpoints.  This will allow you to access these services from within your vnet without going over the public internet.

We will also join the Azure App Service (and it's Web App) to the virtual network with VNet Integration.  This will allow the Web App to access the private services on your vnet.

(Think of Private Endpoint as allowing things in the vnet to talk out to an Azure Service while VNet integration allows the Service to talk to things in the vnet.  So, opposite directions.)

* NOTE: What we're creating below are just simple sandbox areas for us to play around with and learn about Private Endpoints and VNet integration. Please see a real network specialist to know how to setup your actual production vnets and subnets.

Instructions:
- [Create Virtual Network](docs/create-virtual-network.md)



## Create an App Service
An App Service is a managed hosting environment for your web applications.  It can host web applications written in many different languages and frameworks (like .NET, Java, Node, etc).  It can also host APIs and Functions.  It can also host static websites.  It can also host containers.  It's a very flexible service.

We're going to host the application that is stored in this GitHub repository.  Whatever application you're going to host will have certain requirements of how things should be configured.  So be prepared that you will need to alter the steps below to match those needs when you do this for a real application.

Also, we're going to use the easy to use Deployment Center capabilities of App Service.  This is a feature of App Service that will automatically connect to this repository, build the code, and deploy the latest version to your App Service.  For a real application you will probably want to consider GitHub Actions or Azure DevOps Pipelines to do deployments.

Final note, we're going to create two things here: a Web App style App Service and an App Service Plan. Think of the App Service Plan as the underlying IIS web farm, and the Web App/App Service as the site being deployed on the web farm.  Web App and App Service can be used interchangeably for the most part.

Instructions:
- [Create App Service](docs/create-web-app.md#create-app-service)
- [Deploy Web Site Code](docs/create-web-app.md#deploy-application)

## Create a Front Door to the App Service
Azure Front Door (AFD) is a modern Content Delivery Network (CDN) used to improve the performance, availability, and security of web applications and APIs.  It has multiple features including content caching, intelligent routing and load balancing, and a built-in web appliction firewall.

We're going to create an Azure Front Door instance and configure it to deliver our App Service's content.  The network flow will be from users to AFD over public networking and then from AFD to the App Service/Web App over private networking.

We're going to use all the default generated URLs for this exercise but in your usage you will likely follow-up by configuring custom domains (urls) for your users.

Instructions:
- [Create Front Door](docs/create-front-door.md)
- [Approve Private Endpoint](docs/create-front-door.md#approve-private-endpoint)
- [Test Web Site](docs/create-front-door.md#test-the-web-app)

## Create SQL Database
At this point we have a working website but it's standalone, i.e. not using any other servies, which is unlikely for a real website.  Azure SQL Database is a common choice when websites need a relational database.

Azure SQL Database is a PaaS style relational database.  It handles most of the management functions such as upgrading, patching, etc.  It is a very flexible service which can scale from the smallest sporadically used development database to 100TB business critical always on databases.

As a side note, another common alternative is to use SQL Managed Instance.  This is still a managed service but provides functionality very similar to what DBA's are used to on premise.  

Instructions:
- [Create SQL Database](docs/create-sql-database.md)
- [Use SQL Authentication](docs/create-sql-database.md#use-sql-auth-connection-string)
- [Use Azure AD Authentication](docs/create-sql-database.md#use-azure-ad-connection-string)

## Create Redis Cache

## Create Key Vault

## Create Log Analytics Workspace

## Create Application Insights

## Delete Resources




[def]: docs/create-resource-group.md