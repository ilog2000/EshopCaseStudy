# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY EshopCaseStudy.sln ./
COPY backend/Core/Core.csproj ./backend/Core/
COPY backend/ProductService/ProductService.csproj ./backend/ProductService/

RUN dotnet restore ./backend/ProductService/ProductService.csproj

COPY . .
RUN dotnet publish ./backend/ProductService/ProductService.csproj -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductService.dll"]
