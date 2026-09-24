# Identity API

The Identity module provides operations for user registration, authentication, email verification, two-factor authentication, session management and retrieving the current user's information.

## Enable Two-Factor Authentication

Enables two-factor authentication for the authenticated user and sends a confirmation code.

### Endpoint

```http
POST /api/identity/User/2fa/enable
```

### Authentication

Required

### Request Body

None

### Request

```http
POST /api/identity/User/2fa/enable
```

### Successful Response

**200 OK**

```json
{
  "codeId": "b3d165e1-21ff-4798-a9dd-a399c2f10506",
  "codeSent": true
}
```

| Field      | Type    | Description                                                   |
| ---------- | ------- | ------------------------------------------------------------- |
| `codeId`   | UUID    | Identifier of the confirmation code                           |
| `codeSent` | boolean | Indicates whether the confirmation code was sent successfully |

---

## Confirm Two-Factor Authentication

Confirms enabling two-factor authentication using the verification code sent to the user.

### Endpoint

```http
POST /api/identity/User/2fa/enable/confirm
```

### Authentication

Required

### Request Body

| Parameter | Type   | Required | Description                         |
| --------- | ------ | :------: | ----------------------------------- |
| `codeId`  | UUID   |    Yes   | Identifier of the confirmation code |
| `code`    | string |    Yes   | Confirmation code                   |

### Request

```json
{
  "codeId": "b3d165e1-21ff-4798-a9dd-a399c2f10506",
  "code": "6323"
}
```

### Successful Response

**200 OK**

The two-factor authentication setup was confirmed successfully.

### Errors

#### 400 Bad Request

The confirmation code is invalid.

```json
{
  "message": "Invalid confirmation code"
}
```

---

## Verify Two-Factor Authentication During Login

Verifies the two-factor authentication code during the login process.

### Endpoint

```http
POST /api/identity/User/2fa/login-verify
```

### Authentication

None

### Request Body

| Parameter | Type   | Required | Description                               |
| --------- | ------ | :------: | ----------------------------------------- |
| `codeId`  | UUID   |    Yes   | Identifier of the login verification code |
| `code`    | string |    Yes   | Two-factor authentication code            |

### Request

```json
{
  "codeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "code": "6323"
}
```

### Successful Response

**200 OK**

The two-factor authentication code was verified successfully.

---

## Email Verification

Verifies the user's email address using a confirmation code.

### Endpoint

```http
POST /api/identity/User/email-verification
```

### Authentication

None

### Request Body

| Parameter | Type   | Required | Description                               |
| --------- | ------ | :------: | ----------------------------------------- |
| `codeId`  | UUID   |    Yes   | Identifier of the email verification code |
| `code`    | string |    Yes   | Email verification code                   |

### Request

```json
{
  "codeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "code": "6323"
}
```

### Successful Response

**200 OK**

The email address was verified successfully.

### Errors

#### 400 Bad Request

There is no active verification code for the user.

```json
{
  "message": "No active verification code. Please request a new one."
}
```

---

## Register

Creates a new user account.

### Endpoint

```http
POST /api/identity/User/register
```

### Authentication

None

### Request Body

| Parameter         | Type   | Required | Description           |
| ----------------- | ------ | :------: | --------------------- |
| `email`           | string |    Yes   | User email address    |
| `username`        | string |    Yes   | Username              |
| `password`        | string |    Yes   | User password         |
| `confirmPassword` | string |    Yes   | Password confirmation |

### Request

```json
{
  "email": "user@example.com",
  "username": "string",
  "password": "123321",
  "confirmPassword": "123321"
}
```

### Successful Response

**200 OK**

```json
{
  "codeId": "",
  "codeSent": false
}
```

| Field      | Type    | Description                                           |
| ---------- | ------- | ----------------------------------------------------- |
| `codeId`   | UUID    | Identifier of the email verification code             |
| `codeSent` | boolean | Indicates whether an email verification code was sent |

---

## Authenticate

Authenticates a user using their email and password.

If two-factor authentication is enabled, the endpoint returns a verification code identifier instead of completing authentication immediately.

### Endpoint

```http
POST /api/identity/User/auth
```

### Authentication

None

### Request Body

| Parameter  | Type   | Required | Description        |
| ---------- | ------ | :------: | ------------------ |
| `email`    | string |    Yes   | User email address |
| `password` | string |    Yes   | User password      |

### Request

```json
{
  "email": "user@example.com",
  "password": "123321"
}
```

### Successful Response

**200 OK**

When two-factor authentication is required:

```json
{
  "requiresTwoFactor": true,
  "codeId": "6d1b48ae-5bf5-4bbc-ab17-268de85534b1"
}
```

| Field               | Type    | Description                                             |
| ------------------- | ------- | ------------------------------------------------------- |
| `requiresTwoFactor` | boolean | Indicates whether two-factor authentication is required |
| `codeId`            | UUID    | Identifier of the two-factor authentication code        |

---

## Refresh Token

Refreshes the user's authentication session.

### Endpoint

```http
POST /api/identity/User/refresh
```

### Authentication

Required

### Request Body

None

### Request

```http
POST /api/identity/User/refresh
```

### Successful Response

**200 OK**

The authentication session was refreshed successfully.

### Errors

#### 401 Unauthorized

The user is not authenticated or the refresh token is invalid or expired.

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.2",
  "title": "Unauthorized",
  "status": 401,
  "traceId": "00-7f2624b820d34d0297f993b8ce472898-e956bbbdc16df9af-00"
}
```

---

## Get Current User

Returns information about the currently authenticated user.

### Endpoint

```http
GET /api/identity/User/me
```

### Authentication

Required

### Request Body

None

### Request

```http
GET /api/identity/User/me
```

### Successful Response

**200 OK**

```json
{
  "username": "mrtest",
  "email": "user@example.com",
  "userId": "e1487736-a946-424b-aa3a-ef9dfe883fe3",
  "registrationDate": "Wednesday, 16 September 2026"
}
```

| Field              | Type   | Description            |
| ------------------ | ------ | ---------------------- |
| `username`         | string | Username               |
| `email`            | string | User email address     |
| `userId`           | UUID   | User identifier        |
| `registrationDate` | string | User registration date |

---

## Logout

Logs out the currently authenticated user.

### Endpoint

```http
POST /api/identity/User/logout
```

### Authentication

Required

### Request Body

None

### Request

```http
POST /api/identity/User/logout
```

### Successful Response

**200 OK**

```json
{
  "message": "Successfully logged out"
}
```
