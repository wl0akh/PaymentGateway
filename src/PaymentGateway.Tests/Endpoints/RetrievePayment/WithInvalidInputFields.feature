@RetrievePayment
Feature: Retrieve Payment with invalid input

    Payment gateway endpoint to retrieve a payment with invalid input

    Scenario Outline: Retrieve Payment when a PaymentId is invalid GUID
        When a GET is called on /api/payments/<GUID?>
        Then it returns response with status code BadRequest
        And response body contains
            """
            "The value '<GUID?>' is not valid for PaymentId."
            """
        Examples:
            | GUID?                                 |
            #length more than 36
            | 15b3d508-4ef4-4d76-a085-13d146c7d002g |
            #length less than 36
            | 15b3d508-4ef4-4d76-a085-13d146c7d00   |



