#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sandbox/Foundation/Foundation.csproj", "sandbox/Foundation/"]
COPY ["src/Geta.OEmbed.Optimizely.ContentDeliveryApi/Geta.OEmbed.Optimizely.ContentDeliveryApi.csproj", "src/Geta.OEmbed.Optimizely.ContentDeliveryApi/"]
COPY ["src/Geta.OEmbed.Optimizely/Geta.OEmbed.Optimizely.csproj", "src/Geta.OEmbed.Optimizely/"]
COPY ["src/Geta.OEmbed.AspNetCore.Mvc/Geta.OEmbed.AspNetCore.Mvc.csproj", "src/Geta.OEmbed.AspNetCore.Mvc/"]
COPY ["src/Geta.OEmbed/Geta.OEmbed.csproj", "src/Geta.OEmbed/"]
RUN dotnet restore "sandbox/Foundation/Foundation.csproj"
COPY . .
WORKDIR "/src/sandbox/Foundation"
RUN dotnet build "Foundation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Foundation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Foundation.dll"]