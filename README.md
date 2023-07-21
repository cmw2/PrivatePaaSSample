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

[See Create Resource Group instructions](docs/create-resource-group.md)


## Create Virtual Network
A virtual network provides a private network for your resources to communicate with each other without going over public/shared networks.  You can connect virtual networks to each other and even to your on premise networks.

Azure PaaS services, like Azure SQL Database, are traditionally accessed via public networking.  We will be joining Azure PaaS services to the private virtual networks with Private Endpoints.  This will allow you to access these services from within your vnet without going over the public internet.

We will also join the Azure App Service (and it's Web App) to the virtual network with VNet Integration.  This will allow the Web App to access the private services on your vnet.

(Think of Private Endpoint as allowing things in the vnet to talk out to an Azure Service while VNet integration allows the Service to talk to things in the vnet.  So, opposite directions.)

* NOTE: What we're creating below are just simple sandbox areas for us to play around with and learn about Private Endpoints and VNet integration. Please see a real network specialist to know how to setup your actual production vnets and subnets.

[See Create Virtual Network instructions](docs/create-virtual-network.md)



## Create an App Service and a Web App
An App Service is a managed hosting environment for your web applications.  It can host web applications written in many different languages and frameworks.  It can also host APIs and Functions.  It can also host static websites.  It can also host containers.  It can also host many other things.  It's a very flexible service.

We're going to host the application in this repository.  Whatever application you're going to host will have certain requirements of how things should be configured.  So be prepared that you will need to alter the steps below to match those needs when you do this for a real application.

Also, we're going to use the easy to use Deployment Center capabilities of App Service.  This is a feature of App Service that will automatically connect to this repository, build the code, and deploy the latest version to your App Service.  For a real application you will probably want to consider GitHub Actions or Azure DevOps Pipelines to do deployments.

Final note, we're going to create two things, a Web App and an App Service. Think of the App Service as the underlying IIS web farm, and the Web App as the site being deployed on the web farm.

[See Create App Service and Web App instructions](docs/create-web-app.md)

## Create a Front Door to the App Service
Azure Front Door (AFD) is a modern Content Delivery Network (CDN) used to improve the performance, availability, and security of web applications and APIs.  It has multiple features including content caching, intelligent routing and load balancing, and a built-in web appliction firewall.

We're going to create an Azure Front Door instance and configure it to deliver our App Service's content.  The network flow will be from users to AFD over public networking and then from AFD to the App Service/Web App over private networking.

We're going to use all the default generated URLs for this exercise but in your usage you will likely follow-up by configuring custom domains (urls) for your users.

[See Create Front Door instructions](docs/create-front-door.md)

## Create SQL Database

## Create Redis Cache

## Create Key Vault

## Create Log Analytics Workspace

## Create Application Insights

## Delete Resources


