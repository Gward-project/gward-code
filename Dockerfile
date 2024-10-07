FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . ./
RUN dotnet publish Gwards.Api -c Release --no-self-contained -o /src/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /src

COPY --from=build /src/publish .

# SkiaSharp dependencies
RUN apt-get update
RUN apt-get install -y libfreetype6
RUN apt-get install -y libfontconfig1

ENTRYPOINT ["dotnet", "Gwards.Api.dll"]

EXPOSE 8080
