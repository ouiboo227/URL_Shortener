FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY URLvibing.sln ./
COPY shortenerURL_API/shortenerURL_API.csproj shortenerURL_API/
COPY shortenerURL_vibing/shortenerURL_vibing.csproj shortenerURL_vibing/
RUN dotnet restore shortenerURL_API/shortenerURL_API.csproj
COPY . .
WORKDIR /src/shortenerURL_API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false /p:ErrorOnDuplicatePublishOutputFiles=false
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8081
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet","shortenerURL_API.dll"]