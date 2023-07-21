# Create App Service and Web App

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


---
[Back to main instructions](/README.md)