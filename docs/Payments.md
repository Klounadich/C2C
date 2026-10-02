# Payments API

The Payments module provides operations for creating payments, retrieving payment information, processing payment provider webhooks and refunding payments.

## Create Payment

Creates a payment for the specified order and returns a link to the external payment provider.

### Endpoint

```http
POST /api/payments/Payment
```

### Authentication

Required

### Request Body

| Parameter        | Type   | Required | Description                                   |
| ---------------- | ------ | :------: | --------------------------------------------- |
| `orderId`        | UUID   |    Yes   | Identifier of the order to be paid            |
| `idempotencyKey` | string |    Yes   | Unique key used to prevent duplicate payments |

### Request

```json
{
  "orderId": "196162c7-af76-4a5e-a31e-d3525067a874",
  "idempotencyKey": "key"
}
```

### Successful Response

**200 OK**

The response contains a link to the external payment provider.

```text
link to payment provider
```

---

## Get Payment

Returns information about a payment by its identifier.

### Endpoint

```http
GET /api/payments/Payment/{id}
```

### Authentication

Required

### Path Parameters

| Parameter | Type | Required | Description               |
| --------- | ---- | :------: | ------------------------- |
| `id`      | UUID |    Yes   | Identifier of the payment |

### Request

```http
GET /api/payments/Payment/e3ad7024-4497-4348-8d07-cba249c2b801
```

### Successful Response

**200 OK**

```json
{
  "paymentId": "e3ad7024-4497-4348-8d07-cba249c2b801",
  "orderId": "196162c7-af76-4a5e-a31e-d3525067a874",
  "paymentStatus": "Succeeded",
  "amount": 500114,
  "currency": "Rub",
  "updatedAt": "2026-10-02T11:09:44.098611Z"
}
```

| Field           | Type     | Description                        |
| --------------- | -------- | ---------------------------------- |
| `paymentId`     | UUID     | Identifier of the payment          |
| `orderId`       | UUID     | Identifier of the associated order |
| `paymentStatus` | string   | Current status of the payment      |
| `amount`        | number   | Payment amount                     |
| `currency`      | string   | Payment currency                   |
| `updatedAt`     | datetime | Date and time of the last update   |

---

## Payment Webhook

Receives payment status updates from the external payment provider.

### Endpoint

```http
POST /api/payments/Payment/webhook
```

### Authentication

Not required

### Request Body

| Parameter   | Type   | Required | Description               |
| ----------- | ------ | :------: | ------------------------- |
| `paymentId` | UUID   |    Yes   | Identifier of the payment |
| `status`    | string |    Yes   | New status of the payment |

### Request

```json
{
  "paymentId": "e3ad7024-4497-4348-8d07-cba249c2b801",
  "status": "Pending"
}
```

### Successful Response

**200 OK**

The webhook was successfully processed.

---

## Refund Payment

Creates a refund for the specified payment.

### Endpoint

```http
POST /api/payments/Payment/{id}/refund
```

### Authentication

Required

### Path Parameters

| Parameter | Type | Required | Description               |
| --------- | ---- | :------: | ------------------------- |
| `id`      | UUID |    Yes   | Identifier of the payment |

### Request

```http
POST /api/payments/Payment/e3ad7024-4497-4348-8d07-cba249c2b801/refund
```

### Successful Response

**200 OK**

The refund was successfully processed.
