FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SalesOrder.App/SalesOrder.App.csproj", "SalesOrder.App/"]
COPY ["SalesOrder.Common/SalesOrder.Common.csproj", "SalesOrder.Common/"]
RUN dotnet restore "SalesOrder.App/SalesOrder.App.csproj"
COPY . .
#WORKDIR "/src/SalesOrder.App"
WORKDIR "/src/."
RUN dotnet build "SalesOrder.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SalesOrder.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalesOrder.App.dll"]

#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["SalesOrder.App/SalesOrder.App.csproj", "SalesOrder.App/"]
#COPY ["SalesOrder.Common/SalesOrder.Common.csproj", "SalesOrder.Common/"]
#RUN dotnet restore "SalesOrder.App/SalesOrder.App.csproj"
#COPY . .
#WORKDIR "/src/SalesOrder.App"
#RUN dotnet build "SalesOrder.App.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "SalesOrder.App.csproj" -c Release -o /app/publish
#
#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#EXPOSE 5154
#ENTRYPOINT ["dotnet", "SalesOrder.App.dll"]
