@ProcessPayment
Feature: With Valid Input Fields

    Payment gateway endpoint to process payment with valid input fields

    Scenario Outline: Process Payment when a valid request is provided
        Given a request with body as
            """
            {
                "CardNumber": "5500000000000004",
                "Expiry": "12/2024",
                "Amount": "<Amount?>",
                "Currency": "<Currency?>",
                "CVV": "123"
            }
            """
        When Bank Service Responds as
            """
            {
                "PaymentId": "d3a36044-9996-4f46-a73f-5d82a7b85a85",
                "Status": "<Status?>"
            }
            """
        And a POST is called on /api/payments
        Then it returns response with status code Created
        And payment Id is returned in response header
        And payment is recorded in data store
        And PaymentId in data store record is recorded as d3a36044-9996-4f46-a73f-5d82a7b85a85
        And CardNumber in data store record is recorded as 5500000000000004
        And Status in data store record is recorded as <Status?>
        And Expiry in data store record is recorded as 12/2024
        And Amount in data store record is recorded as <Amount?>
        And Currency in data store record is recorded as <CurrencyInResponse?>
        And CVV in data store record is not recorded as 123
        Examples:
            | Amount? | Currency? | Status?      | CurrencyInResponse? |
            #Currency is empty
            | 41.00   |           | successful   | GBP                 |
            #Amount is greter than 0
            | 41.00   | USD       | successful   | USD                 |
            #Amount is less than 0
            | -51.92  | USD       | successful   | USD                 |
            #Amount is equal to 0
            | 0.00    | GBP       | successful   | GBP                 |
            #BankTransection unsuccessful
            | 24.00   | USD       | unsuccessful | USD                 |

    Scenario: Process Payment when Bank Service is inaccessible
        Given a request with body as
            """
            {
                "CardNumber": "5500000000000004",
                "Expiry": "12/2024",
                "Amount": "24.56",
                "Currency": "GBP",
                "CVV": "123"
            }
            """
        When Bank Service is inaccessible
        And a POST is called on /api/payments
        Then it returns response with status code ServiceUnavailable
        And response body contains
            """
            "Error":"Bank Service Unavailable"
            """
        And payment is not recorded in data store

    Scenario: Process Payment when Bank Service response not valid
        Given a request with body as
            """
            {
                "CardNumber": "5500000000000004",
                "Expiry": "12/2024",
                "Amount": "24.56",
                "Currency": "GBP",
                "CVV": "123"
            }
            """
        When Bank Service Responds as
            """
            "<html>page not found</html>"
            """
        And a POST is called on /api/payments
        Then it returns response with status code ServiceUnavailable
        And response body contains
            """
            "Error":"Bank Service Incompatible"
            """
        And payment is not recorded in data store

    Scenario: Process Payment when Currency is not supplyed
        Given a request with body as
            """
            {
                "CardNumber": "5500000000000004",
                "Expiry": "12/2024",
                "Amount": "24.56",
                "CVV": "123"
            }
            """
        When Bank Service Responds as
            """
            {
                "PaymentId": "d3a36044-9996-4f46-a73f-5d82a7b85a85",
                "Status": "successful"
            }
            """
        And a POST is called on /api/payments
        Then it returns response with status code Created
        And payment Id is returned in response header
        And payment is recorded in data store
        And PaymentId in data store record is recorded as d3a36044-9996-4f46-a73f-5d82a7b85a85
        And Status in data store record is recorded as successful
        And CardNumber in data store record is recorded as 5500000000000004
        And Expiry in data store record is recorded as 12/2024
        And Amount in data store record is recorded as 24.56
        And Currency in data store record is recorded as GBP
        And CVV in data store record is not recorded as 123


