@ProcessPayment
Feature: With Invalid Input Fields

    Payment gateway endpoint to process payment with invalid fields
    Scenario Outline: Process Payment when card number is invalid
        Given a request with body as
            """
            {
                "cardNumber": "<CardNumber>",
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
        Examples:
            | CardNumber            |
            #Length more than 19
            | 55000000000000000041  |
            #Length less than 12
            | 55000000000           |
            #Empty string
            |                       |
            #Contains String
            | 55000A0A09000090004   |
            #Contains space
            | '550000000 0000 0004' |

    Scenario Outline: Process Payment when invalid expiry is provided
        Given a request with body as
            """
            {
                "cardNumber": "5500000000000004",
                "expiry": "<Expiry>",
                "amount": "24.56",
                "cvv": "123"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain expiry in error key
        And payment is not recorded in data store
        Examples:
            | Expiry   |
            #Expiry has passed
            | 12/1990  |
            #Empty string
            |          |
            #Month value more than 12
            | 13/2024  |
            #Month value less than 1
            | 00/2024  |
            #Year value length less than 4
            | 00/202   |
            #Year value length more than 4
            | 00/20225 |

    Scenario Outline: Process Payment when invalid Amount is provided
        Given a request with body as
            """
            {
                "cardNumber": "5500000000000004",
                "expiry": "12/2024",
                "amount": "<Amount>",
                "currency": "GBP",
                "cvv": "123"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain amount in error key
        And payment is not recorded in data store
        Examples:
            | Amount |
            #Empty string
            |        |
            #Contains String
            | 4FH    |

    Scenario Outline: Process Payment when invalid Currency is provided
        Given a request with body as
            """
            {
                "cardNumber": "5500000000000004",
                "expiry": "12/2024",
                "amount": "24.56",
                "currency": "<Currency>",
                "cvv": "123"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain currency in error key
        And payment is not recorded in data store
        Examples:
            | Currency |
            #Length less than 3
            | HF       |
            #Length more than 3
            | HFHH     |
            #Contains Digits
            | HF3      |
            #Contains String out of (A-Z or a-z)
            | ab!      |

    Scenario Outline: Process Payment when invalid CVV is provided
        Given a request with body as
            """
            {
                "cardNumber": "5500000000000004",
                "expiry": "12/2024",
                "amount": "24.56",
                "currency": "GBP",
                "cvv": "<CVV>"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contain cvv in error key
        And payment is not recorded in data store
        Examples:
            | CVV  |
            #Length less than 3
            | 12   |
            #Length more than 4
            | 1234 |
            #Contains String
            | 1A3  |