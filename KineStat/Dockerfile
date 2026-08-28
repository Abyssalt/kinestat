FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["KineStat/KineStat.csproj", "KineStat/"]
RUN dotnet restore "KineStat/KineStat.csproj"

COPY . .
WORKDIR "/src/KineStat"
RUN dotnet build "./KineStat.csproj" -c %BUILD_CONFIGURATION% -o /app/build

RUN dotnet publish "KineStat.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KineStat.dll"]