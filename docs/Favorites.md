# Favorites API

The Favorites module provides operations for adding, retrieving and removing items from the authenticated user's favorites.

## Add Item to Favorites

Adds an item to the authenticated user's favorites.

### Endpoint

```http
POST /api/favorites/Favorites/add
```

### Authentication

Required

### Request Body

| Parameter | Type | Required | Description                                |
| --------- | ---- | :------: | ------------------------------------------ |
| `itemId`  | UUID |    Yes   | Identifier of the item to add to favorites |

### Request

```json
{
  "itemId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

### Successful Response

**204 No Content**

The item was successfully added to the user's favorites.

---

## Get Favorite Items

Returns the identifiers of items saved in the authenticated user's favorites.

### Endpoint

```http
GET /api/favorites/Favorites/items?page={page}&pageSize={pageSize}
```

### Authentication

Required

### Query Parameters

| Parameter  | Type    | Required | Description              |
| ---------- | ------- | :------: | ------------------------ |
| `page`     | integer |    Yes   | Page number              |
| `pageSize` | integer |    Yes   | Number of items per page |

### Request

```http
GET /api/favorites/Favorites/items?page=1&pageSize=20
```

### Successful Response

**200 OK**

```json
{
  "itemsId": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ]
}
```

| Field     | Type          | Description                             |
| --------- | ------------- | --------------------------------------- |
| `itemsId` | array of UUID | Identifiers of items saved in favorites |

---

## Delete Item from Favorites

Removes an item from the authenticated user's favorites.

### Endpoint

```http
DELETE /api/favorites/Favorites/delete?itemId={itemId}
```

### Authentication

Required

### Query Parameters

| Parameter | Type | Required | Description                                     |
| --------- | ---- | :------: | ----------------------------------------------- |
| `itemId`  | UUID |    Yes   | Identifier of the item to remove from favorites |

### Request

```http
DELETE /api/favorites/Favorites/delete?itemId=3fa85f64-5717-4562-b3fc-2c963f66afa6
```

### Successful Response

**200 OK**

```json
{
  "isSuccess": true,
  "errorType": null,
  "errorMessage": null
}
```

| Field          | Type          | Description                                      |
| -------------- | ------------- | ------------------------------------------------ |
| `isSuccess`    | boolean       | Indicates whether the operation was successful   |
| `errorType`    | string / null | Type of error if the operation failed            |
| `errorMessage` | string / null | Description of the error if the operation failed |
