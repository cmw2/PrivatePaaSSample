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

1. Start by logging into your Azure Portal.  (https://portal.azure.com or https://portal.azure.us or other relevant URL.)
1. From the Home page, click on Resource groups.  
	1. You might have to scroll down to see this option.
1. Click the Create button in the top left of the toolbar.
1. Select the correct subscription.
	1. Probably you'll want to do this in a sandbox or dev subscription or in your Visual Studio based subscription.
1. Enter a Resource group name.
	1. This can be anything you want.  I'm using "rg-private-paas" for this sample.
	1. This can be a good time to find the Azure Resource Naming Standards (if they exist) for your organization and start using them.
		1. See https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming to get started creating a standard if you don't have one already.
1. Enter a Region.
	1. A Region is a set of datacenters that are close together.  Azure has many regions around the world.  You can find the list of regions at https://azure.microsoft.com/en-us/global-infrastructure/regions/
	1. This should be the same region you use for all of the resources in this sample.  Pick something relatively close to you.  
	1. I'm using East US.
1. Click Next: Tags
1. Tags are optional however may be required based on your organization's policies.
	1. Tags are a simple way to provide some extra information about the resources that will be managed in this resource group.
	1. Tags consist of a Name and a Value.
	1. It can be a good idea to at least put something like Name: Owner and Value: Your Name for future reference.
1. Click Next: Review + Create
1. On this page you validation will run to make sure all settings are valid.
1. If validation passes, click Create.
	1. Creation should happen rather quickly.  
1. You can click Go To Resource in the pop dialog or pick your new group in the list of resource groups.
1. You should now be on a page that shows your new resource group and that it contains no resources.

## Getting back to the Resource Group
To start each of the below sections you'll want to go back to your resource group.  There are a couple ways to get there:

1. If you're looking at a particular resource (probably one you've just created) you can go to the Overview blade (top left menu option) and find the link to the Resource Group in the Essentials area in the main body of the page.
1. You can also click Home in the breadcrumb area.  From there you will likely see your resource group in the list of recently used Resources. 
1. Or, also from the Home page, you can click Resource Groups and find your group in the list of all groups.


## Create Virtual Network
A virtual network provides a private network for your resources to communicate with each other without going over public/shared networks.  You can connect virtual networks to each other and even to your on premise networks.

Azure PaaS services, like Azure SQL Database, are traditionally accessed via public networking.  We will be joining Azure PaaS services to the private virtual networks with Private Endpoints.  This will allow you to access these services from within your vnet without going over the public internet.

We will also join the Azure App Service (and it's Web App) to the virtual network with VNet Integration.  This will allow the Web App to access the private services on your vnet.

(Think of Private Endpoint as allowing things in the vnet to talk out to an Azure Service while VNet integration allows the Service to talk to things in the vnet.  So, opposite directions.)

* NOTE: What we're creating below are just simple sandbox areas for us to play around with and learn about Private Endpoints and VNet integration. Please see a real network specialist to know how to setup your actual production vnets and subnets.

We're starting within your resource group where we left off in the previous section.

1. Click the Create button in the top left of the toolbar.
1. In the search box, type "Virtual Network" and press Enter.
	1. Note this is NOT the top most search box that will search all of Azure.  This is referring to the box with the text "Search the Marketplace"
1. Select Virtual Network from the list of results.
1. Click Create.
1. Again ensure the correct subscription is selected.
1. Ensure the correct resource group is selected (the one we just created above).
1. Enter a name for the virtual network.
	1. I'm using "vnet-private-paas" for this sample.
1. Ensure the Region is the same as you used before.
1. Click Next and then click Next again to get to the IP Addresses page.
1. Add a new subnet
	1. Click the Add a subnet button
	1. Change the Name to "app"
	1. Click Add
1. Repeat above to add another subnet called "app-in"
1. Click Next and then Next again to get to the Review + Create page (or just click the Review + Create button to get there).
1. Once validation has passed, click Create.
1. This should create the virtual network rather quickly.
1. When done you can click Go to resource to explore your new vnet.

## Create an App Service
An App Service is a managed hosting environment for your web applications.  It can host web applications written in many different languages and frameworks.  It can also host APIs and Functions.  It can also host static websites.  It can also host containers.  It can also host many other things.  It's a very flexible service.

We're going to host the application in this repository.  Whatever application you're going to host will have certain requirements of how things should be configured.  So be prepared that you will need to alter the steps below to match those needs when you do this for a real application.

Also, we're going to use the easy to use Deployment Center capabilities of App Service.  This is a feature of App Service that will automatically connect to this repository, build the code, and deploy the latest version to your App Service.  For a real application you will probably want to consider GitHub Actions or Azure DevOps Pipelines to do deployments.

Below we're going to create two things, a Web App and an App Service. Think of the App Service as the underlying IIS web farm, and the Web App as the site being deployed on the web farm.

Starting from the resource group:

1. Click the Create button in the top left of the toolbar.
1. In the search box, type "web app" and press Enter.
1. Click Web App in the search results
1. Click the Create button
1. Again verify the Subscription and Resource Group
1. Enter a name for your web app
	1. I'm using "app-private-paas" for this sample.
	1. However, this has to be a globally unique name so you will need to customize that name.  Perhaps add your initials at the beginning or end.
1. For the Runtime stack, select .NET 6 (LTS)
1. Scroll down to the Pricing plans section and click Create new
1. Give the App Service a name.  I'm using "plan-private-paas" for this sample.
1. You can keep the rest of the fields as the defaults on this page.
1. Click Next: Deployment
1. Click Next: Networking
1. Turn OFF Enable public access
1. Turn ON Enable network injection
1. Select your vnet you created above from the list of Virtual Networks
1. In Inbound access, turn ON Enable private endpoints
	1. Give your private endpoint a name.  I'm using "pe-app-private-paas" for this sample.
	1. Select the inbound subnet as "app"
1. In Outbound access, turn ON Enable VNet integration
	1. Select the outbound subnet as "app-in". The naming is a bit wierd.  From the point of view of the App Service this is the outbound subnet.  From the point of view of the network, this is the inbound subnet.
1. Click Next: Monitoring
1. For now, turn OFF Enable Application Insights.  We'll turn this back on later.
1. Click Review + Create
1. Click Create
1. Creation of this takes a bit longer than the previous things we've created.
1. You may see an error about a conflict.  If so:
	1. Wait until the deployment finishes.
	1. Click Redeploy.
	1. Pick your resource group from the list.
	1. Click Review + Create
	1. Click Create
1. We next want to get to the Web App.  Depending on whether you hit the error above you can either click Go to Resource or Go to Resource Group.
	1. If you went to the Resource Group, click on the Web App in the list of resources.

Now that the Web App is created we're going to tell it to get the application from this GitHub repository.

1. While on the Web App page, click on the Deployment Center blade in the left hand menu.
1. In the Source list select External Git
1. If you get a red error box about Basic authentication
	1. Click the red box
	1. Select the General settings tab
	1. Scroll down to the Basic Auth Publishing Credentials section
	1. Toggle it On
	1. Click Save in the toolbar
	1. Confirm the save by clicking Continue
	1. Close the notification dialog (if it hasn't closed itself)
	1. Click the X in the top right hand (but not the browser X, the one in the Azure portal).
	1. That should bring you back to the Deployment Center blade.
1. Enter "https://github.com/cmw2/PrivatePaaSSample" in the Repository URL field
1. Enter "main" in the Branch field.  (Type it in even though it looks like it's already there.)
1. Click Save in the toolbar.
1. You can switch to the Logs tab and watch the deployment happen.  It can take a few minutes.  You can use the Refresh icon to monitor the status, though the page will automatically update itself as well.

Once this creates you actually won't be able to browse to it.  We have it isolated to the vnet.  You could add a VM and a Bastion host to the vnet so you could sign into a computer within the VNet and then browse to it.  But we're going to use an Azure Front Door in the next section to make the site availabe to public networks.  We do this because Front Door provides extra performance and security benefits over just making the App Service itself publicly accessible.  So for now, you can see the site create but you won't be able to test it yet.

## Create a Front Door to the App Service
Azure Front Door (AFD) is a modern Content Delivery Network (CDN) used to improve the performance, availability, and security of web applications and APIs.  It has multiple features including content caching, intelligent routing and load balancing, and a built-in web appliction firewall.

We're going to create an Azure Front Door instance and configure it to deliver our App Service's content.  The network flow will be from users to AFD over public networking and then from AFD to the App Service/Web App over private networking.

We're going to use all the default generated URLs for this exercise but in your usage you will likely follow-up by configuring custom domains (urls) for your users.

Starting from the resource group:

1. Click the Create button in the top left of the toolbar.
1. In the search box, type "front door" and press Enter.
1. Click Front Door and CDN profiles in the search results
1. Click the Create button
1. Keep the defaults of "Azure Front Door" and "Quick create" and click Continue to create a Front Door.
1. Again verify the Subscription and Resource Group
1. Enter a name for your web app
	1. I'm using "afd-private-paas" for this sample.	
1. Change the Tier to Premium so we can connect to the App Service over private networking.
1. For endpoint name enter "app-private-pass".
   1. This combined with some random characters will be the URL to the site.
1. For Origin type select App Services
1. For Origin host name select your Web App which for me is "app-private-paas.azurewebsites.net"
1. Check the box for Enable private link service
	1. Ensure the Region is correct
	1. For Target sub resource select "sites"
	1. For Request message enter something like "Request to connect AFD".
		1. This will create a request that we need to approve later.
1. For our scenario leave Caching UNchecked.  
	1. To get the full benefit in a real applicaiton you'd want to turn this on, but check with the application owners to make sure they are configuring their caching needs correctly in their website code.
1. For WAF policy, click the Create new button.
	1. Name your WAF policy.  I'm using "wafprivatepaas".
	1. Since we're already in the Premium tier might as well turn on bot protection also. :)
	1. Click Create
1. Click Review + create
1. After validation passes, click Create button.

It will take a couple minutes for this deployment to finish.  Once it does we need to approve out private networking request.

Starting again at the resource group:
1. Navigate to your App Service
1. Select the Networking blade in the left hand menu
1. In the Inbound Traffic section, click the Private endpoints link
1. There should a pending request.  Select that row by clicking in it.
1. Then click the Approve button in the toolbar
1. Confirm by clicking the Yes button
1. Wait while that finishes.  Click the Refresh button (in the toolbar, not the browser refresh) until the Connection state changes to Approved.

Finally let's test our site so far.  Head back to the resource group
1. Navigate to the Front Door
1. In the main view (Overview blade) you should see your Endpoint hostname listed.  (You may have to scroll down a little bit.)
1. Click the Copy button next to the hostname.
1. Open a new browser tab and paste in the url and navigate to it
1. You should see a site that has a  "Welcome" header and menu items for testing Azure SQL and Azure Redis.  We haven't set them up so those pages won't work yet but we should at least see the menu choices.  Setting those up comes next.
	1. It does take some time for things to get working so it's possible it won't be available on your first try.  In that case try again after a minute or so.

## Create SQL Database

## Create Redis Cache

## Create Key Vault

## Create Log Analytics Workspace

## Create Application Insights

## Delete Resources


