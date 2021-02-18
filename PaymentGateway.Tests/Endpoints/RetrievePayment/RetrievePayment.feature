Feature: Retrieve Payment

    Payment gateway endpoint to retrieve a payment

    Scenario Outline: Retrieve Payment when a PaymentId is valid GUID
        Given PaymentId as <GUID> is provided
        And record with PaymentId <Exist?> in data store
        And request header has valid Authorization token
        When a GET is called on "/api/payments/"<GUID>
        Then it returns response with status code <StatusCode>
        And response body <Contains?> Id as <GUID>
        And response body <Contains?> Amount Value
        And response body <Contains?> Currency Value
        And response body <Contains?> TransectionStatus Value
        And response body <Contains?> masked CardNumber Value
        Examples:
            | GUID                                 | Exist?         | StatusCode | Contains?         |
            #Record exist
            | 15b3d508-4ef4-4d76-a085-13d146c7d002 | exist          | OK         | contains          |
            #Record does not exist
            | 24e17f20-3807-445a-963c-9b7e821d482a | does not exist | NotFound   | does not contains |

    Scenario Outline: Retrieve Payment when a PaymentId is invalid GUID
        Given PaymentId as <GUID> is provided
        And request header has valid Authorization token
        When a GET is called on "/api/payments/"<GUID>
        Then it returns response with status code <StatusCode>
        And response body indicate record with <GUID> not found
        Examples:
            | GUID                                  | StatusCode |
            #length more than 36
            | 15b3d508-4ef4-4d76-a085-13d146c7d002g | BadRequest |
            #length less than 36
            | 15b3d508-4ef4-4d76-a085-13d146c7d00   | BadRequest |
            #Empty
            |                                       | NotFound   |

    Scenario Outline: Retrieve Payment when request with invalid Authorization token
        Given request header has <Authorization> Authorization token
        When a GET is called on "/api/payments/15b3d508-4ef4-4d76-a085-13d146c7d002"
        Then it returns response with status code Unauthorized
        And response body indicate Authorization is invalid
        Examples:
            | Authorization |
            | invalid       |
            | Empty         |
            | not supplyed  |


