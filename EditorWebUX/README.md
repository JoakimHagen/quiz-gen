# Black Magic

Alternative title: Conventions that break things when changed

## ASP .NET Core

`appsettings.json` and `appsettings.<ENVIRONMENT>.json` are unreferenced files that are copied on build and read by ASP on start. The current environment name is read from the environment-variable `ASPNETCORE_ENVIRONMENT` (or `DOTNET_ENVIRONMENT`) which are then exposed by the `IWebHostEnvironment` interface passed to the `Startup::Configure` method. Both the content of the appsettings files and any passed command-line arguments are merged and exposed by an `IConfiguration` interface.

The goal is to allow an alternative to command-line arguments.

docs: <https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1>

`Properties/launchSettings.json` is another unreferenced file, responsible for defining possible runtime profiles, accessible through Visual Studio. This file is not involved in or copied on build. It is responsible for defining the different environments the application may start in using VS. This file serves no role in a published production build, but may describe the settings used for running in production.

docs: <https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-3.1#development-and-launchsettingsjson>

tutorial: <https://dotnettutorials.net/lesson/asp-net-core-launchsettings-json-file/>

---

## React

For the project to build, **these files must exist with exact filenames**:

* `public/index.html` is the page template;
* `src/index.js` is the JavaScript entry point.

You can delete or rename the other files.

You may create subdirectories inside `src`. Only files inside `src` are processed by a plugin called Webpack, used for bundling client resources according to ES6's `import` or ES5 `require()` statements.
<br>
You need to **put any JS and CSS files inside `src`**, otherwise Webpack won’t see them.

Only files inside `public` can be used from `public/index.html`.
