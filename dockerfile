FROM mcr.microsoft.com/dotnet/sdk:8.0 as BUILD

RUN mkdir /code
WORKDIR /code
COPY *.csproj .
COPY *.sln .

RUN dotnet restore

COPY . .

RUN dotnet build -c Release --no-restore
RUN dotnet publish -c Release --no-build -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as RUNTIME

RUN mkdir /app
WORKDIR /app

COPY --from=BUILD /app ./

ENTRYPOINT ["dotnet", "LogAnalytic.dll"]
