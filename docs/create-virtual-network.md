# Azure Virtual Network

## Create Virtual Network
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

---
[Back to main instructions](/README.md)