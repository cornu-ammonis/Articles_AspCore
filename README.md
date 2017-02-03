# Articles_AspCore
rebuilt blog in asp.net core

1. install dotnet core SDK/tools and if necessary VS  from https://www.microsoft.com/net/download/core#/current/sdk     
 	 a. make sure to click current -- not LTS-- as we are running 1.1
2. clone repository 
3. open project/solution from visual studio 

  	a. if you receive an error about the wrong SDK version, update global.json under solution items so that the SDK version reflects your currently installed version (found under programfiles / dotnet)
4. open/navigate to package manager console
5. from project root directory run command ‘dotnet restore’
6. cd .\src, then cd .\Articles (from package manager console not terminal)
7. run ‘dotnet build’
8. run add-migrations init -- this will enable migrations and should create a blank migration file named init
 	 a. if the init file is not blank, either you modified an entity's model or something went wrong
	 
 	 b. if command is not recognized, first try closing and reopening visual studio
	 
 	 c. if command is still not recognized ensure that entity framework pacakges are installed by following these instructions https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/new-db
	 
    I. Install-Package Microsoft.EntityFrameworkCore.Tools –Pre
    II. Install-Package Microsoft.EntityFrameworkCore.Design
9. run update-databse. this will create a new sql database locally (if this is the first time you’ve run the command), and create the appropriate tables according to the entity builder / migrations information included in the github repository
10. to confirm that tables were created as expected, you can navigate directly to a representation of the database using View -> Sql server object explorer
11. to test with integration, ctrl - f5 to build and run the app, making sure that for this first launch that the seed method is uncommented in Startup.cs


FOR ACCOUNT REGISTRATION:

1. navigate from the root directory to the ‘articles’ directory -- e.g., cd .\src, cd .\Articles 

2. two variables are needed 
  a. SendGridUser = apikey
  b. SendGridKey = [Paste Key Here]

3. to set these variables, type 
    a. dotnet user-secrets set SendGridUser apikey
    b. dotnet user-secrets set SendGridKey [key here]
    
4. if the command is not recognized, make sure user secrets are installed via the pakcage manager (this should happen automatically when previously running dotnet restore
