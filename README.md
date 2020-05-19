# CRMService

Asp.Net REST API to manage customer data for a small shop. It
will work as the backend side for a CRM interface

## Table of Contents (Optional)

- [Installation](#Installation)
- [Prerequisites](#Prerequisites)
- [Usage](#Usage)
- [Documentation](#Documentation)
- [Tests](#tests)
- [FAQ](#faq)
- [Support](#support)
- [License](#license)

## Installation

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

This API requires the following:  

  * At least [Sql server local utility](https://docs.microsoft.com/en-us/sql/tools/sqllocaldb-utility?view=sql-server-ver15)
  * [Visual Studio 2019](https://www.visualstudio.com/en-us/downloads) 
  * Either a [Microsoft account](https://www.outlook.com) or [work or school account](https://dev.office.com/devprogram)
  * 

### Clone

Clone this repo to your local machine using `https://github.com/jaespinoGIT/CRMService`

### Set up
This API serves as Resource and Authorization Server at the same time, so we are fixing the Audience Id and Audience Secret (Resource Server) in web.config file, this Audience Id and Secret will be used for HMAC265 and hash the JWT token, I’ve used this [implementation](https://github.com/tjoudeh/JWTAspNetWebApi/blob/master/AuthorizationServer.Api/AudiencesStore.cs#l24-34) to generate the Audience Id and Secret.
1. Open **CRMService.sln** file. 
2. In Solution Explorer, CRMService project, open the **Web.config** file. 
3. Update the CRMServiceConnectionString with the sql server connection string data
4. Update keys "as:AudienceId" and "as:AudienceSecret"
5. Set the TokenEnpointpath and the url of the token issuer in the file Startup.Auth.cs    

Determine your ASP.NET app's URL. In Visual Studio's Solution Explorer, select the CRMService project. In the Properties window, find the value of SSL URL 
![URL](https://user-images.githubusercontent.com/56151424/82246490-af846b80-993c-11ea-99cf-31ee2fd319d3.png)

### Superuser data
1. Open **CRMService.sln** file. 
2. In Solution Explorer, CRMService.Infrastructure project,open the **app.config** file. 
3. Update the CRMServiceConnectionString with the sql server connection string data
4. Update the parameter in the EntityFramewwork section with the sql server instance name
4. Open now the **Configuration.cs** file. 
3. Update the data with your new credentials
4. Go to Package Manager Console, choose CRMService.Infrastructure in Default project and execute the migration with the commands **enable-migrations**, **add-migration migrationName** and **update-database**

### Usage
1. First needs an auth token, build and run the app, and visit issuer token url (default https://localhost:44300/oauth/token) with PostMan 
2. Send a post like this with your credentials: ![token](https://user-images.githubusercontent.com/56151424/82249270-671b7c80-9941-11ea-9228-b5eb604b0af1.png)
3. Get the response with the access token needed

## Documentation

With the help of swagger we can easily get all the information about the API endpoints and use the controllers methods.
1. With the app running open swagger in your browser https://localhost:44300/swagger/
2. Insert the access token in the api_key textbox with the format `bearer yourtoken` and press explore
![swagger](https://user-images.githubusercontent.com/56151424/82341323-2e82ae00-99e8-11ea-8fa2-69334a17e432.png)
3. All the controllers can be tested this via too

## Tests

There are two test projects, CRMService.Core.UnitTests that tests the service methods and CRMService.UnitTests for the controllers testing. 
The repository is connected to an [Azure project](https://dev.azure.com/jaespino/CRMService%20Github), for CI and CD


## Deployment

You can easily deploy to a local IIS via this instructions [ASP.NET Web Deployment using Visual Studio](https://docs.microsoft.com/en-us/aspnet/web-forms/overview/deployment/visual-studio-web-deployment/deploying-to-iis)

## License

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
