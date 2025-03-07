# Use official .NET SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy and build the application
COPY . ./
RUN dotnet publish -c Release -o out

# Use runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the application port
EXPOSE 80
EXPOSE 443

# Start the application
ENTRYPOINT ["dotnet", "CarBookingService.dll"]
