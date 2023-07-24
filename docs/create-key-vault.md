# Azure Key Vault

## Create Key Vault
Starting from the resource group:

1. Click the _Create_ button in the top left of the toolbar.
1. In the search box, type "key vault" and press Enter.
1. Click _Key Vault_ in the search results
1. Click the _Create_ button
1. Again verify the Subscription and Resource Group
1. Enter a name for your Key Vault.  This will need to be globally unique
	- I'm using "kv-private-paas".
1. Click _Next_
1. Click _Next_
1. Uncheck _Enable public access_
    - Actually, like for SQL, we're going to have to temporarily turn this on later, but it's good practice to go ahead and just turn it off now.
1. Click _Create a private endpoint_
1. Provide a name
    - I'm using "pe-kv-private-paas"
1. Pick your virutal network and subnet.
    - I'm picking "vnet-private-paas" and "app".
1. Click _OK_
1. Click _Review + create_
1. After validation, click _Create_

This could take a few minutes to finish.

## Set Premissions
At the moment, you have permission to manage the Key Vault itself, but not it's data.  Let's make you an admin:

1. Once the cache is created, navigate to it.
1. Select the _Access Control (IAM)_ blade
1. In the toolbar, click _Add_ and then _Add role assignment_
1. Scroll down the list until you find "Key Vault Administrator" and then click on that row to select it.
1. Click _Next_
1. Click _Select members_ link
1. Search for yourself and then click your name.  This should add you to the list of Selected members.
1. Click _Select_
1. Click _Review + assign_
1. Click _Review + assign_ again

We also need to give the App Service permission to read secrets from the Key Vault. 

1. In the toolbar, click _Add_ and then _Add role assignment_
1. Scroll down the list until you find "Key Vault Secrets User" and then click on that row to select it.
1. Click _Next_
1. Under Assign access to, select _Managed identity_
1. Click _Select members_ link
1. In the Managed identity drop down, select App Service
1. It should then show your App Services (along with a search box to filter the list if it is long)
1. Select your App Service.
1. Click _Select_
1. Click _Review + assign_
1. Click _Review + assign_ again

### Temporarly Enable Your IP
If you were on the same network as the Key Vault you could continue to store the secret, but since we're not, we're going to have to enable public access first.

1. Select the _Networking_ blade
1. Under Allow access from, select _Allow public access from specific virtual networks and IP addresses_
1. Scroll down to the Firewall section and click _Add your client IP address_
1. Paste in your public facing IP Address
    - This won't be the local value you get from ipconfig.
    - If you have it from the SQL Database setup you can use that.
    - Otherwise, in another tab, do a search for "whats my ip" and your browser should tell it to you.
1. Scroll down and click _Apply_

### Store the secret
Finally we can store our secret.  But first we need to go get it! This is probably best done with two separate browser tabs.  We'll use that second tab again in the Configure App Service task below.  

1. Open a new browser tab and navigate to your App Service.
1. Select the _Configuration_ blade
1. Click on the name of the Redis connection string application setting.
1. Copy the value.

Now we can put it in Key Vault
1. Back in the tab with Key Vault open, select the _Secrets_ blade.
1. Click _Generate/Import_ button in the toolbar
1. Enter a Name
    - I'm using ""
1. Enter the Secret Value (paste in what you copied from the App Service setting)
1. Click _Create_

### Cleanup the Public Access
Let's remove that temporary access from our IP Address


## Configure App Service to Use Key Vault
Next we need to configure the App Service to get the redis connection string from Key Vault instead of storing it directly.

Let's gather some details from the Key Vault:
- Key Vault Name: Get from the Overview blade.
- Secret Name: Just entered above.  Get from the Secrets blade.

1. Switch to the tab with the App Service open.  You should still have the App Setting open to the Redis Connection setting.
1. Change the Value to:
    - @Microsoft.KeyVault(kv,sec)
    - Mine is ...


### Change the Connection String Setting

## Test Redis Connection
Now we should be able to test our connection to the Azure Redis Cache. Again, we want to use the Front Door URL, not the App Service URL. 
1. Navigate to your Azure Front Door.
2. On the overview blade find the Endpoint hostname and click the _Copy_ icon to the right of it.
3. Open a new browser tab and navigate to the URL you just copied.
4. Click the _Azure Redis Test_ menu item
5. You should get a screen that shows you the Connection String it tried to use (with the password masked) and messages about trying to Connect as well as a count of the number of tables in the database.
    1. If you don't have success, you should see error messages instead that hopefuly can guide you to where things went wrong.

---
[Back to main instructions](/README.md)