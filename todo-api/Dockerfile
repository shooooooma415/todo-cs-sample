# Use the official .NET 9.0 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET 9.0 SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["todo-api.csproj", "./"]
RUN dotnet restore "todo-api.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src"

# Build the application
RUN dotnet build "todo-api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "todo-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "todo-api.dll"] 