# Use the official .NET Core SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Use the official .NET Core runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Create uploads directory
RUN mkdir -p /app/uploads

WORKDIR /app

# Expose port 5000
EXPOSE 5000

# Set the environment variables
ENV DB_SERVER_PERPUS=database
ENV DB_PORT_PERPUS=5432
ENV DB_NAME_PERPUS=db_perpus
ENV DB_USERNAME_PERPUS=postgres
ENV DB_PASSWORD_PERPUS=postgres

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Perpustakaan.dll"]
