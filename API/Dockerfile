##See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
#
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
#COPY ["API.csproj", "."]
COPY *.sln .
COPY API.csproj .
COPY APITests.csproj ../APITests/
RUN dotnet restore 

COPY . .

WORKDIR "/src/."
RUN dotnet test -c -release --results-directory /testresults --logger "trx;LogFileName=test_results.trx" API.sln
RUN dotnet build "API.csproj" -c Release -o /app/build
#
FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish
#
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]


#WORKDIR "/src"
#RUN dotnet test --logger:trx API.csproj
#RUN dotnet test -c -release --results-directory /testresults --logger "trx;LogFileName=test_results.trx" 

#FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
#WORKDIR /app
##
## copy csproj and restore
#COPY *.sln .
#COPY API/*.csproj .
#COPY APITests/*.csproj ./APITests/ 
#RUN dotnet restore 
##
## copy everything else and build app
#COPY . .
#RUN dotnet build
##
#FROM build AS testrunner
#WORKDIR /app/APITests
#CMD ["dotnet", "test", "--logger:trx"]
##
## run the controller tests
#FROM build AS test
#WORKDIR /app/APITests/Controllers
#RUN dotnet test --logger:trx
## run the service tests
#FROM build AS componenttestrunner
#WORKDIR /app/APITests/Services
#CMD ["dotnet", "test", "--logger:trx"]
#
#WORKDIR /app/API
#RUN dotnet publish -c Release -o out
#
#FROM mcr.microsoft.com/dotnet/core/aspnet:5.0 AS runtime
#WORKDIR /app
#
#COPY --from=build /app/API/out ./
#EXPOSE 80
#EXPOSE 443
#ENTRYPOINT ["dotnet", "API.dll"]