#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["UScheduler.WebApi/UScheduler.WebApi.Gateway/UScheduler.WebApi.Gateway.csproj", "UScheduler.WebApi/UScheduler.WebApi.Gateway/"]
RUN dotnet restore "UScheduler.WebApi/UScheduler.WebApi.Gateway/UScheduler.WebApi.Gateway.csproj"
COPY . .
WORKDIR "/src/UScheduler.WebApi/UScheduler.WebApi.Gateway"
RUN dotnet build "UScheduler.WebApi.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UScheduler.WebApi.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UScheduler.WebApi.Gateway.dll"]