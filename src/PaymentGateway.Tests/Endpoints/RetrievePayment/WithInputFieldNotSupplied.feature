@RetrievePayment
Feature: Retrieve Payment with Empty/NotSupplyed input

    Payment gateway endpoint to retrieve a payment with Empty/NotSupplyed input

    Scenario Outline: Retrieve Payment when a PaymentId is empty/NotSupplyed GUID
        When a GET is called on /api/payments/<GUID?>
        Then it returns response with status code <StatusCode?>
        Examples:
            | GUID? | StatusCode?      |
            #Empty
            |       | MethodNotAllowed |



