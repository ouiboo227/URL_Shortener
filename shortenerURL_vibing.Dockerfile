FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY URLvibing.sln ./
COPY shortenerURL_vibing/shortenerURL_vibing.csproj shortenerURL_vibing/
COPY shortenerURL_API/shortenerURL_API.csproj shortenerURL_API/
RUN dotnet restore URLvibing.sln
COPY . .
RUN dotnet publish shortenerURL_vibing/shortenerURL_vibing.csproj -c Release -o /app/publish /p:UseAppHost=false
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet","shortenerURL_vibing.dll"]