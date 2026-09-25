# Order
Module for creating payment order
### Endpoint

```http
/api/order/Order
```
### Authentication

Required

### Request Body

| Field        | Type        | Description                 |
|--------------|-------------|-----------------------------|
| `productsID` | Array[UUID] | Identifiers of the products |
| `amount`     | decimal     | price for pay               |

### Successful Response

**200 OK**

```json
{
"orderId": "d8b15b57-eca3-4fec-bc9c-0274e619c2be"
}
````

#### 400 Bad Request

The confirmation code is invalid.

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "request": [
      "The request field is required."
    ],
    "$.productIds[0]": [
      "The JSON value could not be converted to System.Guid. Path: $.productIds[0] | LineNumber: 2 | BytePositionInLine: 6."
    ]
  },
  "traceId": "00-58b737bc47d0b4591d652d1ea2aa70c7-04459f4601f62769-00"
}
```
