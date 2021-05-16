![Build status](https://www.codeship.io/projects/1be33f30-9869-403c-b30b-7084c6112c38/status?branch=master)
# PaymentGateway
PaymentGateway

# Understanding and Assumption

Build PaymentGateway which will be used by Consumer Apps/Services and it's responsibility is to process and retrieve payments for merchants 

1) As discussed we will be making this service REST compliant 
2) ProcessPayment Request and Response  validation and acceptance criterion is capture in the following feature files 
    https://github.com/wl0akh/PaymentGateway/tree/main/src/PaymentGateway.Tests/Endpoints/ProcessPayment
3) Retrieve Payments Request and Response  validation and acceptance criterion is capture in the following feature files 
    https://github.com/wl0akh/PaymentGateway/tree/main/src/PaymentGateway.Tests/Endpoints/RetrievePayment
4) on referring PCIDSS recommendations we will not be storing CVV and retrieve payment response will also show masked card number


# User Guide

Steps to use the Payment service

1) Run ```git pull git@github.com:wl0akh/PaymentGateway.git``` and cd into PaymentGateway
2) Run ``` ./scripts/start.sh ```
3) Open postman and import https://raw.githubusercontent.com/wl0akh/PaymentGateway/main/PaymentGateway.postman_collection.json
4) Click on ProcessPayment and inspect the request body and click send as shown below.
<img width="1362" alt="post" src="https://user-images.githubusercontent.com/13693247/108630584-6b8b3d80-745d-11eb-9d33-0737435221d6.png">

5) Copy the PaymentId from the response body and click on RetrievePayment request and replace "REPLACE-PAYMENT-ID-FROM-PROCESS-PAYMENT-RESPONSE" with the copied payment id and click send as shown below.

<img width="1362" alt="get" src="https://user-images.githubusercontent.com/13693247/108630613-907fb080-745d-11eb-887f-2be70178ecac.png">

# Testing

1) To Run all tests Run ``` ./scripts/test.sh ```

# Logging and Analysis

1) To view Log Run ``` docker logs PaymentGateway -f ```
2) Every request should have  RequestId in the response header you can copy it and search the logs example logs as shown below

```
[18:37:42 INF] RequestId:e95968ef-0fd9-4a78-ac5d-ec36239e7fab 
            Started for controller:ProcessPayment and action:ProcessPayment
[18:37:42 INF] Start processing HTTP request POST http://mounte-bank:6060/payments
[18:37:42 INF] Sending HTTP request POST http://mounte-bank:6060/payments
[18:37:42 INF] Received HTTP response after 30.957ms - OK
[18:37:42 INF] End processing HTTP request after 37.4477ms - OK
[18:37:42 INF] RequestId:e95968ef-0fd9-4a78-ac5d-ec36239e7fab 
                    Bank Payout finished with status:APPROVED
                    For Card Ending: ***************6789
[18:37:42 INF] Entity Framework Core 3.1.15 initialized 'DataStoreDbContext' using provider 'MySql.Data.EntityFrameworkCore' with options: None
[18:37:42 INF] Executed DbCommand (4ms) [Parameters=[@p0='?' (Size = 16) (DbType = Binary), @p1='?' (DbType = Decimal), @p2='?' (Size = 4000), @p3='?' (Size = 4000), @p4='?' (Size = 4000), @p5='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
INSERT INTO `Payments` (`PaymentId`, `Amount`, `CardNumber`, `Currency`, `Expiry`, `PaymentStatus`)
VALUES (@p0, @p1, @p2, @p3, @p4, @p5);
[18:37:42 INF] RequestId:e95968ef-0fd9-4a78-ac5d-ec36239e7fab 
            Finished in Duration: 245.4722 Milliseconds
[18:37:42 INF] Executing ObjectResult, writing value of type 'PaymentGateway.API.Endpoints.ProcessPayment.ProcessPaymentResponse'.
[18:37:42 INF] Executed action PaymentGateway.API.Endpoints.ProcessPayment.ProcessPaymentController.ProcessPaymentAsync (PaymentGateway.API) in 356.7469ms
[18:37:42 INF] Executed endpoint 'PaymentGateway.API.Endpoints.ProcessPayment.ProcessPaymentController.ProcessPaymentAsync (PaymentGateway.API)'
[18:37:42 INF] Request finished in 437.4716ms 201 application/json; charset=utf-8

```
