FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
RUN dotnet restore "LibraryService/LibraryService.csproj"
COPY . .
WORKDIR "/src/LibraryService"
RUN dotnet build "LibraryService/LibraryService.csproj" -c Release -o /app

FROM build AS publish
WORKDIR "/src/LibraryService"
RUN dotnet publish "LibraryService/LibraryService.csproj" -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "LibraryService.dll"]