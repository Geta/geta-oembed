# Geta.OEmbed

[![Platform](https://img.shields.io/badge/Platform-.NET%206-blue.svg?style=flat)](https://docs.microsoft.com/en-us/dotnet/)
[![Platform](https://img.shields.io/badge/Optimizely-%2012-orange.svg?style=flat)](http://world.episerver.com/cms/)

## What is does?

Provides tooling for integrating with the oEmbed standard

## How to get started?

Requires using .NET 5.0 or higher

- `dotnet add package Geta.OEmbed`

In Startup.cs

```
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddGetaOEmbed();
}
```

### Documentation

- [Adding support for Optimizely](./docs/optimizely.md)

## Package maintainer

https://github.com/svenrog

## Changelog

[Changelog](CHANGELOG.md)
