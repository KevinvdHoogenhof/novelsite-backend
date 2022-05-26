# novelsite-backend

backend van mijn novelsite

Om de backend te runnen:

## Ssl certificate   
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p mypass123  

## SQL database 
Als er nog geen sql image is:   
docker pull mcr.microsoft.com/mssql/server:2017-latest  

## Sql container:  
docker run --name db -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=1Secure*Password1" -p 1450:1433 -v sqlvolume:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2017-latest   

## Api  
### Build docker image
Build docker image (in de folder; open cmd)
docker build -t kvdhoogenhof/novelsite . 
### Pull image van dockerhub
docker pull kvdhoogenhof/novelsite

## Run image  
docker run -d -e ASPNETCORE_ENVIRONMENT=Development –e ASPNETCORE_URLS=https://+:443;http://+:80 –e ASPNETCORE_HTTPS_PORT=9000  -e ASPNETCORE_Kestrel__Certificates__Default__Password=mypass123  -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -v %USERPROFILE%/.aspnet/https:/https:ro -p 9000:80 -p 9001:443 --name api kvdhoogenhof/novelsite
