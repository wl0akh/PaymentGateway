@ProcessPayment
Feature: WithInput Field NotSupplied

    Payment gateway endpoint to process payment with fields not supplyed

    Scenario: Process Payment when request body is empty
        Given a request with body as
            """
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain "" in error key
        And payment is not recorded in data store

    Scenario: Process Payment when required request field cardNumber is not provided
        Given a request with body as
            """
            {
                "expiry": "12/2024",
                "amount": "24.56",
                "currency": "GBP",
                "cvv": "123"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain cardNumber in error key
        And payment is not recorded in data store

    Scenario: Process Payment when required request field expiry is not provided
        Given a request with body as
            """
            {
                "cardNumber": "5500000000000004",
                "amount": "24.56",
                "currency": "GBP",
                "cvv": "123"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain expiry in error key
        And payment is not recorded in data store

    Scenario: Process Payment when required request field amount is not provided
        Given a request with body as
            """
            {
                "cardNumber": "5500000000000004",
                "expiry": "12/2024",
                "currency": "GBP",
                "cvv": "123"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain amount in error key
        And payment is not recorded in data store


    Scenario: Process Payment when required request field cvv is not provided
        Given a request with body as
            """
            {
                "cardNumber": "5500000000000004",
                "expiry": "12/2024",
                "amount": "24.56",
                "currency": "GBP"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain cvv in error key
        And payment is not recorded in data store
