# Cart API

The Cart module provides operations for managing the authenticated user's shopping cart.

## Get Cart

Returns all items currently added to the authenticated user's cart.

### Endpoint

```http
GET /api/cart/Cart/get
```

### Authentication

Required

### Request Body

None

### Request

```http
GET /api/cart/Cart/get
```

### Successful Response

**200 OK**

```json
{
  "items": [
    {
      "id": "7063de8d-6ac1-461d-a8bf-2eaa15a3bfbe",
      "cart_id": "1df20313-ffcc-4e83-845e-fab7fc868462",
      "item_id": "01db6f0f-7fa2-48b4-ac27-a2248e1edbc7",
      "added_at": "2026-09-24T18:42:14.610964Z"
    }
  ]
}
```

| Field              | Type     | Description                                       |
| ------------------ | -------- | ------------------------------------------------- |
| `items`            | array    | List of items in the user's cart                  |
| `items[].id`       | UUID     | Cart item identifier                              |
| `items[].cart_id`  | UUID     | Cart identifier                                   |
| `items[].item_id`  | UUID     | Marketplace item identifier                       |
| `items[].added_at` | datetime | Date and time when the item was added to the cart |

---

## Get Cart Count

Returns the number of items currently in the authenticated user's cart.

### Endpoint

```http
GET /api/cart/Cart/count
```

### Authentication

Required

### Request Body

None

### Request

```http
GET /api/cart/Cart/count
```

### Successful Response

**200 OK**

```json
{
  "count": 1
}
```

| Field   | Type    | Description                        |
| ------- | ------- | ---------------------------------- |
| `count` | integer | Number of items in the user's cart |

---

## Add Item to Cart

Adds an item to the authenticated user's cart.

### Endpoint

```http
POST /api/cart/Cart/add
```

### Authentication

Required

### Request Body

| Parameter | Type | Required | Description                               |
| --------- | ---- | :------: | ----------------------------------------- |
| `itemId`  | UUID |    Yes   | Identifier of the item to add to the cart |

### Request

```json
{
  "itemId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

### Successful Response

**204 No Content**

The item was successfully added to the cart.

---

## Delete Item from Cart

Removes an item from the authenticated user's cart.

### Endpoint

```http
DELETE /api/cart/Cart/delete
```

### Authentication

Required

### Request Body

| Parameter | Type | Required | Description                      |
| --------- | ---- | :------: | -------------------------------- |
| `itemId`  | UUID |    Yes   | Identifier of the item to remove |
| `cartId`  | UUID |    Yes   | Identifier of the cart           |

### Request

```json
{
  "itemId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "cartId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

### Successful Response

**200 OK**

The item was successfully removed from the cart.

### Errors

#### 500 Internal Server Error

The server failed to delete the item.

```text
Failed to delete item
```
