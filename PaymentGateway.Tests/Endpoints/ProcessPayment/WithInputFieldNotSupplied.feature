@ProcessPayment
Feature: WithInput Field NotSupplied

    Payment gateway endpoint to process payment with fields not supplyed

    Scenario: Process Payment when request body is empty
        Given a request with body as
            """
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain value with "" key
        And payment is not recorded in data store

    Scenario: Process Payment when required request field CardNumber is not provided
        Given a request with body as
            """
            {
                "Expiry": "12/2024",
                "Amount": "24.56",
                "Currency": "GBP",
                "CVV": "123"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain value with CardNumber key
        And payment is not recorded in data store

    Scenario: Process Payment when required request field Expiry is not provided
        Given a request with body as
            """
            {
                "CardNumber": "5500000000000004",
                "Amount": "24.56",
                "Currency": "GBP",
                "CVV": "123"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain value with Expiry key
        And payment is not recorded in data store

    Scenario: Process Payment when required request field Amount is not provided
        Given a request with body as
            """
            {
                "CardNumber": "5500000000000004",
                "Expiry": "12/2024",
                "Currency": "GBP",
                "CVV": "123"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain value with Amount key
        And payment is not recorded in data store


    Scenario: Process Payment when required request field CVV is not provided
        Given a request with body as
            """
            {
                "CardNumber": "5500000000000004",
                "Expiry": "12/2024",
                "Amount": "24.56",
                "Currency": "GBP"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain value with CVV key
        And payment is not recorded in data store
