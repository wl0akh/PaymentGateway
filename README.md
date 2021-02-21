# PaymentGateway
PaymentGateway

# Understanding and Assumption

Build PaymentGateway which will be used by Merchant and it's responsibility is to process and retrieve payments for merchants 

1) As discussed we will be making this service REST compliant 
2) ProcessPayment Request and Response  validation and acceptance criterion is capture in the following feature files 
```src/PaymentGateway.Tests/Endpoints/ProcessPayment```
3) Retrieve Payments Request and Response  validation and acceptance criterion is capture in the following feature files 
```src/PaymentGateway.Tests/Endpoints/RetrievePayment```
4) on referring PCIDSS recommendations we will not be storing CVV and retrieve payment response will also show masked card number


# User Guide

Steps to use the Payment service

1) Run ```git pull git@github.com:wl0akh/PaymentGateway.git``` and cd into PaymentGateway
2) Run ``` ./scripts/start.sh ```
3) Open postman and import ```PaymentGateway.postman_collection.json```
4) Click on ProcessPayment and inspect the request body and click send as shown below.
<img width="1362" alt="post" src="https://user-images.githubusercontent.com/13693247/108630584-6b8b3d80-745d-11eb-9d33-0737435221d6.png">

5) Copy the PaymentId from the response body and click on RetrievePayment request and replace "REPLACE-PAYMENT-ID-FROM-PROCESS-PAYMENT-RESPONSE" with the copied payment id and click send as shown below.

<img width="1362" alt="get" src="https://user-images.githubusercontent.com/13693247/108630613-907fb080-745d-11eb-887f-2be70178ecac.png">

# Testing

1) To Run all tests Run ``` ./scripts/test.sh ```

# Logging and Analysis

1) To view Log Run ``` docker logs PaymentGateway -f ```
2) Every request should have  RequestTrackingId in the response header you can copy it and search the logs example logs as shown below

```

info: PaymentGateway.API.Utils.Filters.TrackingActionFilter[0]
      ***RequestId:a865ac81-6d4b-4c1b-9669-4a6e8d6da16e***
                  Started for controller:RetrievePayment and action:RetrievePayment
info: PaymentGateway.API.Utils.Filters.TrackingActionFilter[0]
      ***RequestId:a865ac81-6d4b-4c1b-9669-4a6e8d6da16e***
                  Finished in Duration: 14.3268 Milliseconds






info: PaymentGateway.API.Utils.Filters.TrackingActionFilter[0]
      ***RequestId:30ad5169-e5d7-489a-b2a0-ee1d78bbdb87***
                  Started for controller:RetrievePayment and action:RetrievePayment
warn: PaymentGateway.API.Endpoints.RetrievePayment.RetrievePaymentController[0]
      ***RequestId:30ad5169-e5d7-489a-b2a0-ee1d78bbdb87***
                      Payment details Not Found in DB for PaymentId:0cb245f9-0c6c-458d-9ad4-cb5cbd33cbba
info: PaymentGateway.API.Utils.Filters.TrackingActionFilter[0]
      ***RequestId:30ad5169-e5d7-489a-b2a0-ee1d78bbdb87***
                  Finished in Duration: 8.4638 Milliseconds
```
