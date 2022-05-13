#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["UScheduler.WebApi/UScheduler.WebApi.Boards/UScheduler.WebApi.Boards.csproj", "UScheduler.WebApi/UScheduler.WebApi.Boards/"]
RUN dotnet restore "UScheduler.WebApi/UScheduler.WebApi.Boards/UScheduler.WebApi.Boards.csproj"
COPY . .
WORKDIR "/src/UScheduler.WebApi/UScheduler.WebApi.Boards"
RUN dotnet build "UScheduler.WebApi.Boards.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UScheduler.WebApi.Boards.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UScheduler.WebApi.Boards.dll"]