# Integrating with Optimizely CMS

_Note: Do all of the steps in the global README._

## Adding property support

- `dotnet add package Geta.OEmbed.Optimizely`

### Register services in Startup

Register the required services by executing the below extension method on your `IServiceCollection`.

```
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddGetaOEmbedOptimizely();
}
```

Then map the required oEmbed routes.

```
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    ...
    app.UseEndpoints(endpoints =>
    {
        ...
        endpoints.MapOEmbed();
    });
}
```

## Adding support for EPiServer.ContentDeliveryApi

- `dotnet add package  Geta.OEmbed.Optimizely.ContentDeliveryApi`

### Register services in Startup

Register the required `ConverterProvider` implementations by executing the below extension method on your `IServiceCollection`.

```
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddGetaOEmbedContentDeliveryApi();
}
```