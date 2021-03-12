FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build 
WORKDIR /source
EXPOSE 3000
COPY ./ /source
RUN dotnet publish -c release -o /release

FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /webapp
COPY --from=build /release ./
ENTRYPOINT ["dotnet", "ToyBlockFactoryWebApp.dll"]