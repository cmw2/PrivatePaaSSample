# Azure Front Door

## Create Front Door
Starting from the resource group:

1. Click the Create button in the top left of the toolbar.
1. In the search box, type "front door" and press Enter.
1. Click Front Door and CDN profiles in the search results
1. Click the Create button
1. Keep the defaults of "Azure Front Door" and "Quick create" and click Continue to create a Front Door.
1. Again verify the Subscription and Resource Group
1. Enter a name for your front door
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

It will take a couple minutes for this deployment to finish.

## Approve Private Endpoint
Once the Front Door has finished creating, we need to approve our private networking request.

Starting again at the resource group:
1. Navigate to your App Service
1. Select the Networking blade in the left hand menu
1. In the Inbound Traffic section, click the Private endpoints link
1. There should a pending request.  Select that row by clicking in it.
1. Then click the Approve button in the toolbar
1. Confirm by clicking the Yes button
1. Wait while that finishes.  Click the Refresh button (in the toolbar, not the browser refresh) until the Connection state changes to Approved.

## Test the Web App
Finally let's test our site so far.  Head back to the resource group
1. Navigate to the Front Door
1. In the main view (Overview blade) you should see your Endpoint hostname listed.  (You may have to scroll down a little bit.)
1. Click the Copy button next to the hostname.
1. Open a new browser tab and paste in the url and navigate to it
1. You should see a site that has a  "Welcome" header and menu items for testing Azure SQL and Azure Redis.  We haven't set them up so those pages won't work yet but we should at least see the menu choices.  Setting those up comes next.
	- It does take some time for things to get working so it's possible it won't be available on your first try.  In that case try again after a minute or so.
    
---
[Back to main instructions](/README.md)