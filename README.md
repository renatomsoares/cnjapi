# CNJ API

## About the API

The API was developed in .NET Core in its version 3.1 and provides endpoints responsible for performing CRUD operations in a SQL Server database hosted on Azure, where lawsuits are stored.

## How to compile and run

To run successfully you will need to have .NET Core 3.1 installed on your machine. After installed, just perform the following steps:

1. clone the repository in some folder on your machine;
1. open the solution in Visual Studio;
1. define the "Application" project as the startup project;
1. run with IIS.

## How use the API
You can use the API through the Swagger interface, locally, after compile and run, or on the website http://cnjapi.azurewebsites.net/, which is the address for the Azure website where the deployment is being performed from the merges performed on the master branch.

## How run the API Tests
The tests were implemented with xUnit and you can run them in the Tests project.
