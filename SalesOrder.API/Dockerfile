FROM mcr.microsoft.com/dotnet/sdk:7.0 AS test
WORKDIR /src
COPY SalesOrder.sln ./SalesOrder.sln
COPY . .
RUN dotnet test

FROM test AS post-test
WORKDIR /src

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5210

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY --from=post-test /src /src
COPY ["SalesOrder.API/SalesOrder.API.csproj", "SalesOrder.API/"]
COPY ["SalesOrder.Common/SalesOrder.Common.csproj", "SalesOrder.Common/"]
COPY ["SalesOrder.Service/SalesOrder.Service.csproj", "SalesOrder.Service/"]
COPY ["SalesOrder.Data/SalesOrder.Data.csproj", "SalesOrder.Data/"]

RUN dotnet restore "SalesOrder.API/SalesOrder.API.csproj"
COPY . .
WORKDIR "/src/SalesOrder.API"
RUN dotnet build "SalesOrder.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SalesOrder.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalesOrder.API.dll"]
