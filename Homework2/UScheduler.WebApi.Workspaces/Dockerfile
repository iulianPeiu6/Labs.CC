#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UScheduler.WebApi.Workspaces/UScheduler.WebApi.Workspaces.csproj", "UScheduler.WebApi.Workspaces/"]
RUN dotnet restore "UScheduler.WebApi.Workspaces/UScheduler.WebApi.Workspaces.csproj"
COPY . .
WORKDIR "/src/UScheduler.WebApi.Workspaces"
RUN dotnet build "UScheduler.WebApi.Workspaces.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UScheduler.WebApi.Workspaces.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UScheduler.WebApi.Workspaces.dll"]