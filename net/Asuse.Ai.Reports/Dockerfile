FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

RUN apt-get update && apt-get install -y libgdiplus && ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Asuse.Ai.Reports.csproj", "."]
RUN dotnet restore "./Asuse.Ai.Reports.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Asuse.Ai.Reports.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Asuse.Ai.Reports.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Asuse.Ai.Reports.dll"]
