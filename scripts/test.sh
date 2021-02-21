
docker-compose up -d
sleep 5
dotnet clean src; dotnet test src/PaymentGateway.Tests/PaymentGateway.Tests.csproj 