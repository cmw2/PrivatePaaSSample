# Azure SQL Database

## Create SQL Database
Starting from the resource group:

1. Click the _Create_ button in the top left of the toolbar.
1. In the search box, type "sql database" and press Enter.
1. Click _SQL Database_ in the search results
1. Click the _Create_ button
1. Again verify the Subscription and Resource Group
1. Enter a name for your database.
	1. I'm using "SampleDB".	
1. For Server, click the _Create new_ button
    1. Enter a name for your logical database server.  This will need to be globally unique.
        1. I'm using "dbs-private-paas"
    1. Verify the location
    1. For Authentication method select _Use both SQL and Azure AD Authentication_
    1. Under Set Azure AD Admin click _Set Admin_
    1. Use the search box to find and then select yourself with the checkbox.
    1. Click _Select_ button
    1. For Server admin login you're creating the equivalent of the SA account for this logical server.
        1. I'm using "sqladmin
    1. Enter and then re-enter a strong password you'll remember.
        1. You can reset the password later but you won't see it displayed anywhere.
    1. Click _OK_ button
1. Keep defualt of No for Elastic Pool
1. Under Compute + storage click _Configure Database_.  This is where you select how much hardware to allocate for this database.  We'll be picking low end options for this scenario.
    1. For Service Tier leave the default of General Purpose.
    1. For Compute Tier pick Serverless
    1. For Max vCore adjust to "1"
    1. Can leave eveything else at defaults.
        1. Note the auto-pause setting is at 1 hour.  This is a cost savings tool that will automatically scale the compute nodes down to 0 for an unused database and then wake-up again on demand.  However just be aware it could give a temporary error while it wakes up again so best for your non-production workloads.  If you encounter that just wait a minute and try again.
    1. Click _Apply_ button.
1. For Backup storage redundancy, select _Locally-redundant_ for our purposes but obviously for more important applications you'll want to explore the other options.
1. Click _Next: Networking_ button.
1. For Connectivity method Click _Private Endpoint_
    1. Private endpoint will make this database usable by resources on your private network.
1. Click _Add private endpoint_
    1. Enter a name for the private endpoint.
        1. I'm using "pe-db-private-paas"
    1. Select your virtual network
        1. I selected "vnet-private-paas"
    1. Select your subnet
        1. I selected "app"
    1. Leave rest at defaults.
    1. Click _OK_ button
1. Click _Next: Security_ button
    1. Leave defaults
1. Click _Next: Additional Settings_ button
1. For Use existing data, select _Sample_
1. Click _OK_ at the prompt about changing settings.
1. Click _Review + create_
1. Review the configured information and then click _Create_ button

It will take a couple minutes for this deployment to finish.  Once it does we need to configure the web app with the connection string and then test things out.

## Use SQL Auth Connection String
Applications can connect to the database using either SQL Auth (User and Password in connection string) or Azure AD Auth (using Managed Identity, no password in connection string).  We'll explore both but we'll go the simple route first.  

We're going to store this connection string in the App Service configuration.  That's ok, but even better will be to put it in Key Vault which we'll explore later

1. Once the create process finishes click _Go to resource_ 
1. In the Essential area of the main page body, click _Show database connection strings_
1. Find the box labelled _ADO.NET (SQL Authentication)_ and click the little _Copy_ icon in the lower right of that box.
    1. Note this connection string is using the Admin user we created earlier.  This isn't typically what you'd want the application to run with but we're keeping it simple here.  See the docs for more info on [authorizing database access](https://learn.microsoft.com/en-us/azure/azure-sql/database/logins-create-manage?view=azuresql-mi).
1. The connection string you just copied doesn't have the password entered.  I find it easiest to put this in using Notepad.
    1. Open Notepad.
    1. Paste in the connection string you just copied.
    1. Find and replace the *{your_password}* part with the correct password from back when you were creating the SQL Database.
        1. Note you will also replace the {} so the result will be something like ... ;Password=mystrongpwd; ...
    1. Now copy the entire connection string again from Notepad.
1. Now we need to head back to the Web App.
    1. You can head back to your resource group and find it in the list.
    1. If you recall the name a quick way can be to type it in the top most search bar and select it from the results.
1. Once in the web app, select the _Configuration_ blade in the left hand menu.
1. Click the _New connection string_ button
    1. You might need to scroll down a little to see it
1. For Name, enter "SampleDB"
    1. Note the name here doesn't need to  match the name of the database.  It does however need to be the name the application is expecting, so you'll likely need to get it from the developers.
1. For Value, paste in the connection string you copied
1. For Type select SQLServer
1. Click _OK_ button
1. Click _Save_ button in toolbar.
1. Confirm the save by clicking _Continue_ button.
    1. Note the message of this prompt.  Changing the settings causes the web app to have to restart.  This could lead to downtime so you want to be careful of this for production apps.

## Test SQL Connection
Now we should be able to test our connection to SQL. We want to use the Front Door URL, not the App Service URL. 
1. Navigate to your Azure Front Door.
2. On the overview blade find the Endpoint hostname and click the _Copy_ icon to the right of it.
3. Open a new browser tab and navigate to the URL you just copied.
4. Click the _Azure SQL Test_ menu item
5. You should get a screen that shows you the Connection String it tried to use (with the password masked) and messages about trying to Connect as well as a count of the number of tables in the database.
    1. If you don't have success, you should see error messages instead that hopefuly can guide you to where things went wrong.

## Use Azure AD Connection String
In the previous example we used SQL Authentication and a connection string with a password.  In this section we're going to switch to using Azure Active Directory (now called Entra ID) authentication instead.  This will allow us to avoid the password in the connection string and manage permissions with AAD.  Another approach would be to use the techniques we discuss later to store secrets in Azure Key Vault instead of directly in the App Service Configuration.

In this scenario the user account won't be the SQL Admin, but will instead be a Managed Identity representing the Web App. This is like a service account for the web app.

### Create a Managed Identity for the Web App
1. Navigate to your Web App/App Service
1. Select the _Identity_ blade in the left hand menu
    1. Note we are on the System assigned identity tab.  You can also create managed identities separately and assign them using the User assigned tab.  Using System assigned means this identity's lifecycle will be linked to this web app.  When you delete this web app the identity will also be removed.
1. Toggle the Status switch to On.
1. Click _Save_ button in toolbar.
1. Confirm the action by clicking the _Yes_ button.
1. Wait just a few seconds for it to enable then continue.

### Add the Managed Identity as a user in the database
Next we need to run some commands in the database.  At the moment that database is locked down to only be accessible by resources in the virtual network.  If you've been following along exactly and have created a new VNET your local PC is not on that VNET.  We're going to temporarily allow Public Access from your IP Address so you can connect and run these commands.  

(If however you were using your organization's existing private networking and everything was already setup correctly you may actually be able to connect without having to do these steps.)

1. Navigate to your SQL Database Server.
    1. The easiest might be to navigate to SampleDB database and then, on the Overview blade, click the Server name link in the top right of Essentials.
    1. You can also go the resource group and find it in the list.  It's Type will be SQL Server.
1. Select the _Networking_ blade in the left hand menu.
    1. If you don't see a Networking blade you're probably on your SQL Database, not your SQL Server.  See instructions just above.
1. Click the _Selected networks_ option under Public Access
1. Scroll down to the Firewall rules section and click _Add your client IPv4 address_ link.
    - Make a note of the IP address it adds here so you can use it again in the Key Vault section.
1. Click _Save_ button
1. After it has saved, select the _SQL Databases_ blade in the left hand menu
1. Click the row with your database (probably SampleDB)
1. Select the _Query editor (preview)_ blade
1. The right hand side should show you logged in with a blue button _Continue as yourname_.  Click that button.
1. On the right hand side will be a new query windw.  Paste the below code into that window:
    ```
    CREATE USER [<identity-name>] FROM EXTERNAL PROVIDER;
    ALTER ROLE db_datareader ADD MEMBER [<identity-name>];
    ALTER ROLE db_datawriter ADD MEMBER [<identity-name>];
    ALTER ROLE db_ddladmin ADD MEMBER [<identity-name>];
    GO
    ```
1. Replace the \<identity-name\> with the name of the managed identity.  Since we used a System Managed Identity that will be the same as the name of the Web App/App Service.  For me that is app-private-pass. So my block looks like
    ```
    CREATE USER [app-private-paas] FROM EXTERNAL PROVIDER;
    ALTER ROLE db_datareader ADD MEMBER [app-private-paas];
    ALTER ROLE db_datawriter ADD MEMBER [app-private-paas];
    ALTER ROLE db_ddladmin ADD MEMBER [app-private-paas];
    GO
    ```
1. Click the _Run_ button.  You should get a message in the Messages area (lower right) that says the query succeeded.

### Change the connection string
1. Select the _Connection Strings_ blade
    1. You can click _OK_ to discard our query window content.  It won't rollback the effect.
1. The first box should be labelled _ADO.NET (Active Directory passwordless authentication)_.  Click the _Copy to clipboard_ icon its lower right.
1. Navigate to the Web App/App Service
1. Select the _Configuration_ blade
1. Scoll down to find the connection string you created earlier and click its Name.
1. In the window that opens up, delete what's currently in the Value window and then paste in the connection string you copied from above.
1. Click _OK_ button
1. Click _Save_ button
1. Confirm by clicking _Continue_
    
Now that you've made the changes, go test things out following the instructions in the [Test SQL Connection](#test-sql-connection) section. Or maybe you still have that tab open and can refresh.  

Double check the connection string it shows to make sure it is the new one and doesn't have a password field in it.  If not, you may just need to wait a minute and refresh as you may have still been using the old site before it fully restarted.

## Cleanup Firewall
Let's also clean up that firewall rule and Public Access we had to create. 
1. Navigate back to the Database Server
1. Select the _Networking_ blade
1. Scroll down until you see the list of Firewall rules and then delete the rule you added with the trash can icon on the right.
1. Scroll back up and select the _Disable_ option under Public network access.
1. Click _Save_ button

Not a bad idea to [test again](#test-sql-connection).

---
[Back to main instructions](/README.md)