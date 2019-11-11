# reactjs-ts-identityserver
Demo project for using Identity Server with React + TypeScript and Angular

The project is in 4 parts:

#### IdentityServer (http://localhost:5000)

Based off the IdentityServer4 samples, it uses uses ASP.NET Identity for identity management. You'll need to create the database (instructions bellow) to begin creating user accounts.

#### Api (http://localhost:5200)

Based off the `dotnet new webapi` template. Has a global authorize filter.

#### spaAngular (http://localhost:4200)

#### spaReact (http://localhost:5100)

A single page application made with React, TypeScript and Webpack. Uses [redux-oidc](https://github.com/maxmantz/redux-oidc) package for managing authentication.

#### MVC (http://localhost:5100)


### Stuff to install

Dotnet Core 3.0 SDK  
Node.js  
SQL Server (or at least LocalDb)

### Running the project

Clone repository

Open a command prompt in project location:

`cd Spa`

`npm install`

`npm start`


In a second command prompt:

`cd IdentityServer`

`dotnet ef database update`

`dotnet run`

In a third command prompt:


`cd Api`

`dotnet run`

Go to identity server (http://localhost:5000) create an account then go to the single page app (http://localhost:5100)

You may need to change the environment variable on each project, either
* run `set ASPNETCORE_ENVIRONMENT=Development` in Windows cmd terminal
* run `$Env:ASPNETCORE_ENVIRONMENT = "Development"` in Windows powershell terminal

### Credits

* [IdentityServer4.Samples](https://github.com/IdentityServer/IdentityServer4.Samples)
* [Dotnet core templates](https://github.com/aspnet/JavaScriptServices) 
* [redux-oidc](https://github.com/maxmantz/redux-oidc)
