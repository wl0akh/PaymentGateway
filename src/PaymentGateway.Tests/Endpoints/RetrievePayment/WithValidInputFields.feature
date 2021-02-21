@RetrievePayment
Feature: Retrieve Payment with valid input

    Payment gateway endpoint to retrieve a payment with valid input

    Scenario: Retrieve Payment when a PaymentId is valid GUID
        Given following payment record exists in data store
            """
            {
                "PaymentId": "15b3d508-4ef4-4d76-a085-13d146c7d002",
                "CardNumber": "5500000000000004",
                "Status": "successful",
                "Expiry": "12/2024",
                "Amount": 34,
                "Currency": "GBP",
                "CVV": "123"
            }
            """
        When a GET is called on /api/payments/15b3d508-4ef4-4d76-a085-13d146c7d002
        Then it returns response with status code OK
        And response body contains
            """
            "PaymentId":"15b3d508-4ef4-4d76-a085-13d146c7d002"
            """
        And response body contains
            """
            "CardNumber":"************0004"
            """
        And response body contains
            """
            "Expiry":"12/2024"
            """
        And response body contains
            """
            "Amount":34.00
            """
        And response body contains
            """
            "Currency":"GBP"
            """
        And response body should not contains
            """
            "CVV":"123"
            """

    Scenario: Retrieve Payment when a PaymentId is valid GUID but record does not exist
        Given record with PaymentId 24e17f20-3807-445a-963c-9b7e821d482a does not exists in data store
        When a GET is called on /api/payments/24e17f20-3807-445a-963c-9b7e821d482a
        Then it returns response with status code NotFound
        And response body contains
            """
            "Type":"NotFound"
            """
        And response body contains
            """
            "Error":"No Record found for PaymentId : 24e17f20-3807-445a-963c-9b7e821d482a"
            """
        And response body contains
            """
            "RequestTraceId":
            """


