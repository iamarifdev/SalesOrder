## Docker Support
#### To run the application in docker, run from solution directory

###### Detached mode
```bash
docker-compose up --build -d
```
###### Foreground mode
```bash
docker-compose up --build
```
###### To stop the application
```bash
docker-compose down
```

### To navigate and run the Web App in browser
```bash
http://localhost:5555
```
##### Swagger UI is available at
```bash
http://localhost:5210/swagger/index.html
```

## Migration
#### To add migration, run from solution directory
```bash
dotnet ef migrations add Initial --project SalesOrder.Data --startup-project SalesOrder.API
```

#### To remove migrations, run from solution directory
```bash
dotnet ef migrations remove --project SalesOrder.Data --startup-project SalesOrder.API
```

#### To update database, run from solution directory
```bash
dotnet ef database update --project SalesOrder.Data --startup-project SalesOrder.API
```

#### To run the tests
```bash
dotnet test
```