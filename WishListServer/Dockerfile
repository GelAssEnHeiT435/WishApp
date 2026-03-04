#  build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WishListServer.csproj", "./"]
RUN dotnet restore "./WishListServer.csproj"

# copy code to src
COPY . .
WORKDIR "/src/."

# publish project
RUN dotnet publish "./WishLishServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WishListServer.dll"]