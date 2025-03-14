# Use the official .NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["LoggingMetricsWebApi.csproj", "./"]
RUN dotnet restore "LoggingMetricsWebApi.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "LoggingMetricsWebApi.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "LoggingMetricsWebApi.csproj" -c Release -o /app/publish

# Final stage: copy the published application to the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoggingMetricsWebApi.dll"]