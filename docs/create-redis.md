# Azure Cache for Redis

## Create Cache
Starting from the resource group:

1. Click the _Create_ button in the top left of the toolbar.
1. In the search box, type "redis cache" and press Enter.
1. Click _Azure Cache for Redis_ in the search results
    - Note it may not be the first option.
1. Click the _Create_ button
1. Again verify the Subscription and Resource Group
1. Enter a name for your cache.  This will need to be globally unique
	- I'm using "redis-private-paas".
1. Click _Next: Networking_
1. Click _Private Endpoint_
1. Click _Add_
1. Provide a name
    - I'm using "pe-redis-private-paas"
1. Pick your virutal network and subnet.
    - I'm picking "vnet-private-paas" and "app".
1. Click _OK_
1. Click _Review + create_
1. After validation, click _Create_

This could take a while to finish.

## Use Connection String
Like SQL, we can use connection strings that have passwords (access keys in this case) in them or ones that use Managed Identity.  However for Redis the second option is currently in preview so we're not going to explore that.  We're going to use the traditional type of connection string.  For now we're going to put this in the App Service's configuration.  In Key Vault instructions we'll move this into Key Vault and access from there.

1. Once the cache is created, navigate to it.
1. On the _Overview_ blade, in the _Essentials_ area look for and click on _Show access keys_ link.
1. Find the _Primary connection string_ box and click the _Copy to clipboard_ icon in the right hand side.
1. Navigate to the App Service.
1. Select the _Configuration_ blade
1. Click the _New application setting_ link
1. Paste your copied connection string into the Value field
1. Enter "CacheConnection" into the _Name_ field
1. Click _OK_ button
1. Click _Save_ in the toolbar
1. Confirm by clicking _Continue_ button


## Test Redis Connection
Now we should be able to test our connection to the Azure Redis Cache. Again, we want to use the Front Door URL, not the App Service URL. 
1. Navigate to your Azure Front Door.
2. On the overview blade find the Endpoint hostname and click the _Copy_ icon to the right of it.
3. Open a new browser tab and navigate to the URL you just copied.
4. Click the _Azure Redis Test_ menu item
5. You should get a screen that shows you the Connection String it tried to use (with the password masked) and messages about trying to Connect as well reading and writing to the cache.
    1. If you don't have success, you should see error messages instead that hopefuly can guide you to where things went wrong.

---
[Back to main instructions](/README.md)