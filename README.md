# Pokedex

[Pokedox](https://localhost:5001/swagger/index.html) Is a simple WebApi service to translate pokemon descriptions in a funny way.

### API routs available are listed below

|Route |Parameter |Description |
|-|-|-|
|<b>[GET /api/Pokemons](localhost:5001/api/Pokemons)</b>|Number of pokemon names|Lists top number of pokemon names|
|<b>[GET api/Pokemon/<pokemon name>](localhost:5001/api/Pokemon)</b>|Pokemon name or number|Reads pokemon details|
|<b>[GET api/Pokemon/<pokemon name>/translated](localhost:5001/api/Pokemon/1/translated)</b>|Pokemon name or number|Reads pokemon details with translated names|
<hr>

## Prerequisites

Project runs on .NET Core 3.1 platform which can be downloaded and installed with [this link](https://dotnet.microsoft.com/download)

## Running the solution

To run the solution after downloading it from GitHub, in the command line please follow the steps below


```
dotnet build
dotnet run --project WebApi
```

## Testing the application

The tool comes with convenien way to test via web browser, please navigate to the [link](https://localhost:5001/swagger/index.html)

## For Production
  
When publishing to release in production the ASPNETCORE_ENVIRONMENT setting in launchSettings.json file would need to be changed to 'Production'.
Also the following tasks would be good to take before releasing to production:
 - Disable Swagger
 - Introduce Authentication with OAuth
 - Review security for OWASP
 - Monitor requests and its load
 - Enhance logging
 - Integrate with Azure for ie. monitoring services
  
# License

Open Source
