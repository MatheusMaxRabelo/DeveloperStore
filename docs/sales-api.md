[Back to README](../readme.md)

# Developer Store API Documentation

## Base URL
```
http://localhost:5233/api
```

---

## Endpoints

### 1. Create Sale
**Endpoint**: `POST /sales`  
**Description**: Creates a new sale record.  
**Request Body** (JSON):
```json
{
    "customerId": "integer",
    "branch": "string",
    "products": [
        {
            "productId": "integer",
            "quantity": "integer"
        },
        {
            "productId": "integer",
            "quantity": "integer"
        },
        {
            "productId": "integer",
            "quantity": "integer"
        },
        {
            "productId": "integer",
            "quantity": "integer"
        },
    ]
}
```
**Response** (JSON):
```json
{
    "id": "integer",
    "salesDate": "datetime",
    "customer": {
        "id": "integer",
        "firstName": "string",
        "lastName": "string",
        "email": "string"
    },
    "totalAmount": "numeric",
    "branch": "string",
    "products": [
        {
            "id": "integer",
            "title": "string",
            "unitPrice":  "numeric",
            "image": "string",
            "quantity": "integer",
            "discount":  "numeric",
            "totalAmount":  "numeric",
            "isCancelled": "boolean",
        },...
    ],
    "isCancelled": "boolean",
}
```


---

### 2. Get Sales
**Endpoint**: `GET /sales`  
**Description**: Retrieves all sales records.  
**Response** (JSON):
```json
{
    "data": [
        {
            "id":  "integer",
            "salesDate": "datetime",
            "customer": {
                "id":  "integer",
                "firstName": "string",
                "lastName": "string",
                "email": "string"
            },
            "totalAmount": "numeric",
            "branch": "string",
            "products": [
                {
                    "id": "integer",
                    "title": "string",
                    "unitPrice":  "numeric",
                    "quantity":  "integer",
                    "discount": "numeric",
                    "totalAmount": "numeric"
                }, ...
            ],
            "isCancelled": "boolean"
        }
    ],
    "totalItems": "integer",
    "currentPage": "integer",
    "totalPages": "integer"
}
```

---

### 3. Get Sale By Id
**Endpoint**: `GET /sales/{id}`  
**Description**: Retrieves a specific sale record by its ID.  
**Example**:  
`GET /sales/8`  
**Response** (JSON):
```json
{
    "id": "integer",
    "salesDate": "datetime",
    "customerid": "integer",
    "totalAmount": "numeric",
    "branch": "string",
    "products": [
        {
            "id": "integer",
            "title": "string",
            "unitPrice": "numeric",
            "quantity": "integer",
            "discount": "numeric",
            "totalAmount": "numeric",
            "isCancelled": "boolean"
        }
    ],
    "isCancelled": "boolean"
}
```
---

### 4. Update Sale
**Endpoint**: `PUT /sales/{id}`  
**Description**: Updates an existing sale record by its ID.  
**Request Body** (JSON):
```json
{
   "id": "integer",
    "salesDate": "datetime",
    "customerid": "integer",
    "totalAmount": "numeric",
    "branch": "string",
    "products": [
        {
            "id": "integer",
            "title": "string",
            "unitPrice": "numeric",
            "quantity": "integer",
            "discount": "numeric",
            "totalAmount": "numeric",
            "isCancelled": "boolean"
        }
    ],
    "isCancelled": "boolean"
}
```
**Response** (JSON):
```json
{
    "id": "integer",
    "salesDate": "datetime",
    "customerid": "integer",
    "totalAmount": "numeric",
    "branch": "string",
    "products": [
        {
            "id": "integer",
            "title": "string",
            "unitPrice": "numeric",
            "quantity": "integer",
            "discount": "numeric",
            "totalAmount": "numeric",
            "isCancelled": "boolean"
        }
    ],
    "isCancelled": "boolean"
}
```
---

### 5. Delete Sale
**Endpoint**: `DELETE /sales/{id}`  
**Description**: Deletes a specific sale record by its ID.  
**Example**:  
`DELETE /sales/13`
**Response** (JSON):
```json
{
    "message": "string",
}
```


<br/>
<div style="display: flex; justify-content: space-between;">
  <a href="./users-api.md">Previous: Users API</a>
  <a href="./project-structure.md">Next: Project Structure</a>
</div>