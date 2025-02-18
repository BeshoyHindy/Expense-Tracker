FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=5001
ENV ASPNETCORE_URLS="http://+:5001"
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ExpenseTracker.Api/ExpenseTracker.Api.csproj", "src/ExpenseTracker.Api/"]
COPY ["src/ExpenseTracker.Application/ExpenseTracker.Application.csproj", "src/ExpenseTracker.Application/"]
COPY ["src/ExpenseTracker.Domain/ExpenseTracker.Domain.csproj", "src/ExpenseTracker.Domain/"]
COPY ["src/ExpenseTracker.Infrastructure/ExpenseTracker.Infrastructure.csproj", "src/ExpenseTracker.Infrastructure/"]
RUN dotnet restore "src/ExpenseTracker.Api/ExpenseTracker.Api.csproj"
COPY . .
WORKDIR "/src/src/ExpenseTracker.Api"
RUN dotnet build "ExpenseTracker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExpenseTracker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpenseTracker.Api.dll"]
