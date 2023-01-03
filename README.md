# Geta.OEmbed

[![Build](https://github.com/Geta/geta-oembed/actions/workflows/build.yml/badge.svg)](https://github.com/Geta/geta-oembed/actions/workflows/build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Geta_geta-oembed&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Geta_geta-oembed)
[![Platform](https://img.shields.io/badge/Platform-.NET%206-blue.svg?style=flat)](https://docs.microsoft.com/en-us/dotnet/)
[![Platform](https://img.shields.io/badge/Optimizely-%2012-orange.svg?style=flat)](http://world.episerver.com/cms/)

## What is does?

Provides tooling for integrating with the oEmbed standard

## How to get started?

Requires using .NET 6.0 or higher

- `dotnet add package Geta.OEmbed.Client`

In Startup.cs

```
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddGetaOEmbed();
}
```

### Documentation

- [Extending service behavior](./docs/extension-points.md)
- [Adding support for Optimizely](./docs/optimizely.md)

## Package maintainer

https://github.com/svenrog

## Changelog

[Changelog](CHANGELOG.md)
