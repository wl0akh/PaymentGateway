@ProcessPayment
Feature: With Invalid Input Fields

    Payment gateway endpoint to process payment with invalid fields
    Scenario Outline: Process Payment when card number is invalid
        Given a request with body as
            """
            {
                "CardNumber": "<CardNumber?>",
                "Expiry": "12/2024",
                "Amount": "24.56",
                "Currency": "GBP",
                "CVV": "123"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contains
            """
            "The cardNumber must be of 12 to 19 digits"
            """
        And payment is not recorded in data store
        Examples:
            | CardNumber?           |
            #Length more than 19
            | 55000000000000000041  |
            #Length less than 12
            | 55000000000           |
            #Contains String
            | 55000A0A09000090004   |
            #Contains space
            | '550000000 0000 0004' |

    Scenario Outline: Process Payment when invalid Expiry is provided
        Given a request with body as
            """
            {
                "CardNumber": "5500000000000004",
                "Expiry": "<Expiry?>",
                "Amount": "24.56",
                "CVV": "123"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contains
            """
            "The expiry must be in future and in the formate: MM/YYYY"
            """
        And payment is not recorded in data store
        Examples:
            | Expiry?  |
            #Expiry has passed
            | 12/1990  |
            #Month value more than 12
            | 13/2024  |
            #Month value less than 1
            | 00/2024  |
            #Year value length less than 4
            | 00/202   |
            #Year value length more than 4
            | 00/20225 |

    Scenario: Process Payment when invalid Amount is provided
        Given a request with body as
            """
            {
                "CardNumber": "5500000000000004",
                "Expiry": "12/2024",
                "Amount": "4FH",
                "Currency": "GBP",
                "CVV": "123"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contains
            """
            Could not convert string to decimal
            """
        And payment is not recorded in data store

    Scenario Outline: Process Payment when invalid Currency is provided
        Given a request with body as
            """
            {
                "CardNumber": "5500000000000004",
                "Expiry": "12/2024",
                "Amount": "24.56",
                "Currency": "<Currency?>",
                "CVV": "123"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contains
            """
            "The currency must be 3 letter string"
            """
        And payment is not recorded in data store
        Examples:
            | Currency? |
            #Length less than 3
            | HF        |
            #Length more than 3
            | HFHH      |
            #Contains Digits
            | HF3       |
            #Contains String out of (A-Z or a-z)
            | ab!       |

    Scenario Outline: Process Payment when invalid CVV is provided
        Given a request with body as
            """
            {
                "CardNumber": "5500000000000004",
                "Expiry": "12/2024",
                "Amount": "24.56",
                "Currency": "GBP",
                "CVV": "<CVV>"
            }
            """
        When a POST is called on /api/payments
        Then it returns response with status code BadRequest
        And response body contains
            """
            "The cvv must 3 digits"
            """
        And payment is not recorded in data store
        Examples:
            | CVV  |
            #Length less than 3
            | 12   |
            #Length more than 4
            | 1234 |
            #Contains String
            | 1A3  |