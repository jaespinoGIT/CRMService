# CRMService

Asp.Net REST API to manage customer data for a small shop. It
will work as the backend side for a CRM interface

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

This sample requires the following:  

  * At least [Sql server local utility](https://docs.microsoft.com/en-us/sql/tools/sqllocaldb-utility?view=sql-server-ver15)
  * [Visual Studio 2019](https://www.visualstudio.com/en-us/downloads) 
  * Either a [Microsoft account](https://www.outlook.com) or [work or school account](https://dev.office.com/devprogram)

## Configure the app
This API serves as Resource and Authorization Server at the same time, so we are fixing the Audience Id and Audience Secret (Resource Server) in web.config file, this Audience Id and Secret will be used for HMAC265 and hash the JWT token, I’ve used this [implementation](https://github.com/tjoudeh/JWTAspNetWebApi/blob/master/AuthorizationServer.Api/AudiencesStore.cs#l24-34) to generate the Audience Id and Secret.
1. Open **CRMService.sln** file. 
2. In Solution Explorer, CRMService project, open the **Web.config** file. 
3. Update keys "as:AudienceId" and "as:AudienceSecret"
4. Now, you can set the TokenEnpointpath and the url of the token issuer in the file Startup.Auth.cs 

Determine your ASP.NET app's URL. In Visual Studio's Solution Explorer, select the CRMService project. In the Properties window, find the value of SSL URL
![URL](https://user-images.githubusercontent.com/56151424/82246490-af846b80-993c-11ea-99cf-31ee2fd319d3.png)

## Super user data
1. Open **CRMService.sln** file. 
2. In Solution Explorer, CRMService.Infrastructure project, open the **Configuration.cs** file. 
3. Update the data with your credentials
4. Go to Package Manager Console, choose CRMService.Infrastructure in Default project and execute the migration with the commando **update-database**

## Running the tests

With the help of swagger we can easily get all the documentation about the API and try the URL. 
1. First you need an auth token, visit issuer token url (default https://localhost:44300/oauth/token) with PostMan 
2. Send a post like this with your credentials: ![token](https://user-images.githubusercontent.com/56151424/82249270-671b7c80-9941-11ea-9228-b5eb604b0af1.png)
3. You will get a response with the access token needed
4.  


## Deployment

You can easily deploy to a local IIS via this instructions [ASP.NET Web Deployment using Visual Studio](https://docs.microsoft.com/en-us/aspnet/web-forms/overview/deployment/visual-studio-web-deployment/deploying-to-iis)

