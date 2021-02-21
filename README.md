# PaymentGateway
PaymentGateway

# Understanding and Assumption

Build PaymentGateway which will be used by Merchant and it's responsibility is to process and retrieve payments for merchants 

1) As discussed we will be making this service REST compliant 
2) ProcessPayment Request and Response  validation and acceptance criterion is capture in the following feature files 
```src/PaymentGateway.Tests/Endpoints/ProcessPayment```
3) Retrieve Payments Request and Response  validation and acceptance criterion is capture in the following feature files 
```src/PaymentGateway.Tests/Endpoints/RetrievePayment```
4) on referring PCIDSS recommendations we will not be storing CVV and retrieve payment response will show masked card number


# User Guide

Steps to use the Payment service

1) Run ```git pull git@github.com:wl0akh/PaymentGateway.git``` and cd into PaymentGateway
2) Run ``` ./scripts/start.sh ```
3) Open postman and import ```PaymentGateway.postman_collection.json```
4) Click on ProcessPayment and inspect the request body and click send as shown below.
5) Copy the PaymentId from the response body and click on RetrievePayment request and replace "REPLACE-PAYMENT-ID-FROM-PROCESS-PAYMENT-RESPONSE" with the copied payment id and click send as shown below.
