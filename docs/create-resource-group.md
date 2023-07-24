# Azure Resource Group

## Create Resource Group
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
To start each of the other sections you'll often want to go back to your resource group.  There are a couple ways to get there:

1. If you're looking at a particular resource (probably one you've just created) you can go to the Overview blade (top left menu option) and find the link to the Resource Group in the Essentials area in the main body of the page.
1. You can also click Home in the breadcrumb area.  From there you will likely see your resource group in the list of recently used Resources. 
1. Or, also from the Home page, you can click Resource Groups and find your group in the list of all groups.

---
[Back to main instructions](/README.md)