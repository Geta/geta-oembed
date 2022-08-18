# Extending service behaviour

## IProviderUrlBuilder

Provider url builders are used to compile an outgoing requests based on a given `IOEmbedProvider` and an instance of `OEmbedOptions`.

The `DefaultProviderUrlBuilder` will act on all providers, find the first available endpoint and produce an url like `{endpoint}?url={yourvideo}&{parameters}`.
If you wish to modify this behaviour for one or many providers you can implement the `IProviderUrlBuilder` interface and register it for dependency injection before calling `AddGetaOEmbed`.

```
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.TryAddEnumerable(new ServiceDescriptor(typeof(IProviderUrlBuilder), typeof(YourProviderUrlBuilder), ServiceLifetime.Singleton));
    services.AddGetaOEmbed();
}
```

## IProviderResponseFormatter

Provider response formatters are used to rewrite or format incoming instances of `OEmbedResponse` before they are returned by the configured `IOEmbedService`.
Included in this package is one response formatter, `YouTubeVideoResponseFormatter` which appends parameters to the `src` attribute on the `<iframe>` element returned from YouTube.

If you wish to extend this behaviour for one or many providers you can implement the `IProviderResponseFormatter` interface and register it for dependency injection before calling `AddGetaOEmbed`.

```
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.TryAddEnumerable(new ServiceDescriptor(typeof(IProviderResponseFormatter), typeof(YourProviderResponseFormatter), ServiceLifetime.Singleton));
    services.AddGetaOEmbed();
}
```