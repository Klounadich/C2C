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
{
  "items": [
    {
      "id": "0b1f6157-c1b4-4993-a016-36175f2e1ef5",
      "userId": "a3f1c2d4-8b6e-4a1f-9c3d-2e7f6a8b1c4d",
      "category": "electronics",
      "title": "iPhone 13 Pro Max, 128 ГБ, SIM + eSIM",
      "price": 32990,
      "city": "d1e2f3a4-3333-4b2c-8d3e-4f5a6b7c8d03",
      "img_link": "https://60.img.avito.st/image/1/1.FnFeF7aAupgAsVCffitVXkm3uJzsoL6Y7MfbnOxcvXLttL6CALFQn-g.kPg2ZdrLOm8FJB1YO9irbHOxSz-C9OP7J4uZ259rMZQ",
      "views": 3,
      "moderated": true,
      "created_at": "2026-08-18T17:01:48.842789Z",
      "updated_at": "2026-08-18T17:01:48.842789Z"
    }
  ]
}
}
```

### Errors

#### 400 Bad Request

The provided item identifier is invalid.

```json
{
  "errors": {
    "request": [
      "The request field is required."
    ],
}
```

#### 404 Not Found

The requested item does not exist.

```json
{
{
  "items": []
}
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
The search involves matching against aliases. WIP (automated alias addition service)
## FIND_CATEGORY

returns products found based on the selected category

### Endpoint

```http
POST /api/catalog/Catalog/find/category
```

### Authentication

None

### Query Parameters

| Parameter  | Type    | Required | Description                                  |
|------------|---------|---------:|----------------------------------------------|
| `request`  | string  |      Yes | categroy type (rnd - get most viewed goods ) |
| `page`     | int | yes | page number                                  |
| `pageSize` |int|yes| number of products per page                  |
### Request

```http
{
  "request": "rnd",
  "page": 1,
  "pageSize": 1
}

```

### Successful Response

**200 OK**

```json
{
{
  "items": [
    {
      "id": "00494a4b-1cc0-4b46-a650-33ecdc091cff",
      "userId": "a3f1c2d4-8b6e-4a1f-9c3d-2e7f6a8b1c4d",
      "category": "home",
      "title": "Брусчатка Прямоугольник жёлтый",
      "price": 825,
      "city": "d1e2f3a4-3333-4b2c-8d3e-4f5a6b7c8d03",
      "img_link": "https://70.img.avito.st/image/1/1.tgMehba1GupAI_DtfsuQLQklGOqgLvDtQCMY6A.y02ytqfv8jdflUn1vQyDqaMf6QmK4_X33olrzum2n8Y",
      "views": 21,
      "moderated": true,
      "created_at": "2026-08-18T17:01:48.842789Z",
      "updated_at": "2026-08-18T17:01:48.842789Z"
    }
  ]
}
}
```

### Errors

#### 400 Bad Request

The provided item identifier is invalid.

```json
{
  "errors": {
    "request": [
      "The request field is required."
    ],
}
```

#### 404 Not Found

The requested item does not exist.

```json
{
{
  "items": []
}
}
```

#### 401 Unauthorized

Authentication is required.

```json
{
  "error": "Unauthorized"
}
```
## Add_Item

places an advertisement for the sale of the product

### Endpoint

```http
POST /api/catalog/Catalog/add_item
```

### Authentication

Required

### Query Parameters

| Parameter  | Type     | Required | Description          |
|------------|----------|---------:|----------------------|
| `UserId`   | UUID     |      Yes | user identifier      |
| `itemName` | string   |      yes | name of product      |
| `category` | string   |      yes | product category     |
| `price`    | decimal  |   yes(0) | price of the product |
| `city`     | UUID     |      yes | city identifier      |
| `image`    | png/jpg/ |      yes | product image        |

### Request

```http
{
  "request": "rnd",
  "page": 1,
  "pageSize": 1
}

```

### Successful Response

**200 OK**

```json
{
  true
}
```

### Errors

#### 400 Bad Request

The provided item identifier is invalid.

```json
{
  "errors": {
    "request": [
      "The request field is required."
    ],
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
Before data is saved to the database, product information is moderated using [AI](https://huggingface.co/cointegrated/rubert-tiny-toxicity/tree/main).

## REPORT_ITEM

Reports an item that violates the marketplace rules.

### Endpoint

```http
POST /api/catalog/Catalog/report_item
```

### Authentication

None

### Request Body

| Parameter | Type   | Required | Description                              |
| --------- | ------ | :------: | ---------------------------------------- |
| `text`    | string |    Yes   | Description of the reason for the report |
| `itemId`  | UUID   |    Yes   | Identifier of the reported item          |

### Request

```http
POST /api/catalog/Catalog/report_item
Content-Type: application/json
```

```json
{
  "text": "The item description contains prohibited content.",
  "itemId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

### Successful Response

**200 OK**

```json
true
```

### Errors

#### 400 Bad Request

The request contains invalid or missing parameters.

```json
{
  "errors": {
    "request": [
      "The request field is required."
    ]
  }
}
```

#### 404 Not Found

The reported item does not exist.

```json
{
  "error": "Item not found"
}
```





