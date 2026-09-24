# Catalog API

The Catalog module provides operations for creating, searching and retrieving marketplace items.

## Get Item

Returns detailed information about an item.

### Endpoint

```http
GET /api/catalog/item?id={id}
```

### Authentication

None

### Query Parameters

| Parameter | Type | Required | Description     |
| --------- | ---- | -------: | --------------- |
| `id`      | UUID |      Yes | Item identifier |

### Request

```http
GET /api/catalog/item?id=8f4c2d91-6c3d-4e5e-bc1c-2a8e2a4b9f12
Authorization: Bearer <token>
```

### Successful Response

**200 OK**

```json
{
  "id": "8f4c2d91-6c3d-4e5e-bc1c-2a8e2a4b9f12",
  "userId": "f72d2f9e-4a7c-4a31-9e1e-7e3e3b3b1c22",
  "category": "Electronics",
  "title": "iPhone 15",
  "price": 699.99,
  "city": "Nizhny Novgorod",
  "imgLink": "https://example.com/image.jpg",
  "views": 124,
  "createdAt": "2026-09-24T10:30:00Z",
  "updatedAt": "2026-09-24T12:15:00Z"
}
```

### Errors

#### 400 Bad Request

The provided item identifier is invalid.

```json
{
  "error": "Invalid item id"
}
```

#### 404 Not Found

The requested item does not exist.

```json
{
  "error": "Item not found"
}
```

#### 401 Unauthorized

Authentication is required.

```json
{
  "error": "Unauthorized"
}
```

### Notes

The `views` field represents the number of recorded item views.

View counters may be temporarily stored in Redis before being persisted to PostgreSQL.

## FIND

returns products found based on the query

### Endpoint

```http
GET /api/catalog/find?query=&page=&pageSize=
```

### Authentication

None

### Query Parameters

| Parameter  | Type    | Required | Description |
|------------|---------|---------:|----|
| `query`    | string  |      Yes | request text |
| `page`     | int | yes | page number |
| `pageSize` |int|yes| number of products per page   |
### Request

```http
GET /api/catalog/find?query=iphone&page=1&pageSize=20

```

### Successful Response

**200 OK**

```json
{
  "id": "8f4c2d91-6c3d-4e5e-bc1c-2a8e2a4b9f12",
  "userId": "f72d2f9e-4a7c-4a31-9e1e-7e3e3b3b1c22",
  "category": "Electronics",
  "title": "iPhone 15",
  "price": 699.99,
  "city": "Nizhny Novgorod",
  "imgLink": "https://example.com/image.jpg",
  "views": 124,
  "createdAt": "2026-09-24T10:30:00Z",
  "updatedAt": "2026-09-24T12:15:00Z"
}
```

### Errors

#### 400 Bad Request

The provided item identifier is invalid.

```json
{
  "error": "Invalid item id"
}
```

#### 404 Not Found

The requested item does not exist.

```json
{
  "error": "Item not found"
}
```

#### 401 Unauthorized

Authentication is required.

```json
{
  "error": "Unauthorized"
}
```

### Notes

The `views` field represents the number of recorded item views.

View counters may be temporarily stored in Redis before being persisted to PostgreSQL.

