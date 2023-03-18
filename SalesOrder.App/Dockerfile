FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SalesOrder.App/SalesOrder.App.csproj", "SalesOrder.App/"]
COPY ["SalesOrder.Common/SalesOrder.Common.csproj", "SalesOrder.Common/"]
RUN dotnet restore "SalesOrder.App/SalesOrder.App.csproj"
COPY . .
WORKDIR "/src/SalesOrder.App"
RUN dotnet build "SalesOrder.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SalesOrder.App.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY SalesOrder.App/nginx.conf /etc/nginx/nginx.conf