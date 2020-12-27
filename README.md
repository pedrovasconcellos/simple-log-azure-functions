# SimpleLog
Simple Log

Install dependencies
Before you can get started, you should install .NET Core. You should also install Node.JS which includes npm, which is how you will obtain the Azure Functions Core Tools. If you prefer not to install Node, see the other installation options in our Core Tools reference.
Run the following command to install the Core Tools package:
````
$ npm install -g azure-functions-core-tools@3
````

Create an Azure Functions project
In the terminal window or from a command prompt, navigate to an empty folder for your project, and run the following command:
````
$ func init
````
You will also be prompted to choose a runtime for the project. Select dotnet.


Create a function
To create a function, run the following command:
````
$ func new
````
This will prompt you to choose a template for your function. We recommend HTTP trigger for getting started.


Run your function project locally
Run the following command to start your function app:
````
$ func  start
````
The runtime will output a URL for any HTTP functions, which can be copied and run in your browser's address bar.
To stop debugging, use Ctrl-C in the terminal.


Deploy your code to Azure
To publish your Functions project into Azure, enter the following command:
````
$ cd src/SimpleLog.HttpTrigger
````
````
$ az login
````
````
$ func azure functionapp publish SimpleLog --csharp
````


Install MSSQL (SqlServer) to test inside LocalHost
````
$ docker run --name sqlserver -i -e ACCEPT_EULA=Y -e SA_PASSWORD=MyPassword7 -p 1433:1433 -d mcr.microsoft.com/mssql/server:latest
````

Install Docker MongoDB to test inside LocalHost (Note: use port 17017 to connect to the bank)
````
$  docker run --name MongoDB -p 17017:27017 -d mongo
````


Create Environment Variable
````
Name = ConnectionStringMSQSQL.SimpleLog.HttpTrigger
Value = Server=localhost;Database=SimpleLog;MultipleActiveResultSets=true;User Id=sa;Password=MyPassword7

Name = ConnectionStringMongoDB.SimpleLog.HttpTrigger
Value = mongodb://localhost:17017/EmailForm
````

## Sponsor
[![Vasconcellos Solutions](https://vasconcellos.solutions/assets/open-source/images/company/vasconcellos-solutions-small-icon.jpg)](https://www.vasconcellos.solutions)
### Vasconcellos IT Solutions