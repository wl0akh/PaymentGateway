@ProcessPayment
Feature: With Valid Input Fields

    Payment gateway endpoint to process payment with valid input fields

    Scenario Outline: Process Payment when a valid request is provided
        Given a request with body as
            """
            {
                "cardNumber": "5500000000000004",
                "expiry": "12/2024",
                "amount": "<amount?>",
                "currency": "<currency?>",
                "cvv": "123"
            }
            """
        When Bank Service Responds as
            """
            {
                "paymentId": "d3a36044-9996-4f46-a73f-5d82a7b85a85",
                "status": "<status?>"
            }
            """
        And a POST is called on /api/payments
        Then it returns response with status code Created
        And payment Id is returned in response header
        And payment is recorded in data store
        And payment is recorded in data store with paymentId as d3a36044-9996-4f46-a73f-5d82a7b85a85
        And payment is recorded in data store with cardNumber as 5500000000000004
        And payment is recorded in data store with status as <status?>
        And payment is recorded in data store with expiry as 12/2024
        And payment is recorded in data store with amount as <amount?>
        And payment is recorded in data store with currency as <currencyInResponse?>
        Examples:
            | amount? | currency? | status?      | currencyInResponse? |
            #currency is empty
            | 41.00   |           | successful   | GBP                 |
            #amount is greter than 0
            | 41.00   | USD       | successful   | USD                 |
            #amount is less than 0
            | -51.92  | USD       | successful   | USD                 |
            #amount is equal to 0
            | 0.00    | GBP       | successful   | GBP                 |
            #BankTransection unsuccessful
            | 24.00   | USD       | unsuccessful | USD                 |
            #BankTransection not working
            | 24.00   | USD       | not working  | USD                 |

    Scenario: Process Payment when Bank Service is inaccessible
        Given a request with body as
            """
            {
                "cardNumber": "5500000000000004",
                "expiry": "12/2024",
                "amount": "24.56",
                "currency": "GBP",
                "cvv": "123"
            }
            """
        When Bank Service is inaccessible
        And a POST is called on /api/payments
        Then it returns response with status code ServiceUnavailable
        And Bank Service Unavailable is shown in response body
        And payment is not recorded in data store

    Scenario: Process Payment when Bank Service response not valid
        Given a request with body as
            """
            {
                "cardNumber": "5500000000000004",
                "expiry": "12/2024",
                "amount": "24.56",
                "currency": "GBP",
                "cvv": "123"
            }
            """
        When Bank Service Responds as
            """
            "<html>page not found</html>"
            """
        And a POST is called on /api/payments
        Then it returns response with status code ServiceUnavailable
        And Bank Service Incompatible is shown in response body
        And payment is not recorded in data store

    Scenario: Process Payment when currency is not supplyed
        Given a request with body as
            """
            {
                "cardNumber": "5500000000000004",
                "expiry": "12/2024",
                "amount": "24.56",
                "cvv": "123"
            }
            """
        When Bank Service Responds as
            """
            {
                "paymentId": "d3a36044-9996-4f46-a73f-5d82a7b85a85",
                "status": "successful"
            }
            """
        And a POST is called on /api/payments
        Then it returns response with status code Created
        And payment Id is returned in response header
        And payment is recorded in data store
        And payment is recorded in data store with paymentId as d3a36044-9996-4f46-a73f-5d82a7b85a85
        And payment is recorded in data store with status as successful
        And payment is recorded in data store with cardNumber as 5500000000000004
        And payment is recorded in data store with expiry as 12/2024
        And payment is recorded in data store with amount as 24.56
        And payment is recorded in data store with currency as GBP


