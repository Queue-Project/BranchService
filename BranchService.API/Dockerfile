FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5001
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BranchService/BranchService.API/BranchService.API.csproj", "BranchService.API/"]
COPY ["BranchService/BranchService.Application/BranchService.Application.csproj", "BranchService.Application/"]
COPY ["BranchService/BranchService.Contracts/BranchService.Contracts.csproj", "BranchService.Contracts/"]
COPY ["BranchService/BranchService.Domain/BranchService.Domain.csproj", "BranchService.Domain/"]
COPY ["BranchService/BranchService.Infrastructure/BranchService.Infrastructure.csproj", "BranchService.Infrastructure/"]
RUN dotnet restore "BranchService.API/BranchService.API.csproj"
COPY . .
WORKDIR "/src/BranchService/BranchService.API"
RUN dotnet build "./BranchService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BranchService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BranchService.API.dll"]
