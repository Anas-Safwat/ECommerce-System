E-Commerce System — Project Requirements
========================================

Objective
---------

Develop a secure, scalable, and well-structured E-Commerce System using ASP.NET Core, incorporating:

*   RESTful API (CRUD, Filtering, Sorting, Pagination)
    
*   Repository Pattern & UnitOfWork (Clean Architecture)
    
*   Authentication & Authorization (JWT + Role-Based Access)
    
*   Validation & Error Handling (FluentValidation)
    
*   Asynchronous Programming (Optimized Performance)
    

A) Project Requirements
-----------------------

### User Management (Authentication & Authorization)

*   User Registration (Email, Password, Role)
    
*   User Login/Logout (JWT Token Generation)
    
*   Role-Based Access Control (RBAC)
    
    *   Admin (Full access)
        
    *   Seller (Add/Edit products, manage inventory)
        
    *   Customer (Browse, add to cart, checkout)
        

### Product Management (CRUD Operations)

*   Create, Read, Update, Delete Products (Name, Price, Description, Category, Stock)
    

### Product Categories (Many-to-Many Relationship)

*   Product Reviews & Ratings (One-to-Many)
    

### Order & Checkout System

*   Shopping Cart (Add/Remove items, Quantity)
    
*   Order History (View past orders)
    

### Caching — Product Catalog Caching

Cache the GET /api/products endpoint to reduce database load and improve response time.

*   First request → DB
    
*   Next requests → Cache
    
*   Cache invalidated on:
    
    *   Product Create
        
    *   Product Update
        
    *   Product Delete
        

### Real-Time Order Status Updates

When an order status changes, notify the customer instantly via SignalR. When implementing the order status request:

*   Update Database (OrderTable)
    
*   Save Changes
    
*   Notify Customer based on User ID (Use Grouping)
    

### Advanced API Features

*   Filtering (By Category, Price Range, Rating)
    
*   Sorting (By Price, Rating, Popularity)
    
*   Pagination (Limit/Offset or Page/Size)
    

### Validation & Error Handling

*   FluentValidation for model validation
    
*   Global Exception Handling (Custom error responses)
    

B) Entity Overview & Relationships
----------------------------------

### 1\. User

Represents a system user with role-based access.

*   **Properties:**
    
    *   Id (GUID)
        
    *   Email
        
    *   PasswordHash
        
    *   Role (Admin, Seller, Customer)
        
    *   CreatedAt
        
*   **Relationships:**
    
    *   One-to-Many with Order (Customer)
        
    *   One-to-Many with Product (Seller)
        
    *   One-to-Many with Review (Customer)
        

### 2\. Product

Represents an item listed for sale.

*   **Properties:**
    
    *   Id
        
    *   Name
        
    *   Description
        
    *   Price
        
    *   Stock
        
    *   SellerId (FK to User)
        
    *   CreatedAt
        
*   **Relationships:**
    
    *   Many-to-Many with Category via ProductCategory
        
    *   One-to-Many with Review
        
    *   One-to-Many with OrderItem
        

### 3\. Category

Defines product classification.

*   **Properties:**
    
    *   Id
        
    *   Name
        
*   **Relationships:**
    
    *   Many-to-Many with Product via ProductCategory
        

### 4\. ProductCategory (Join Table)

Handles many-to-many between Product and Category.

*   **Properties:**
    
    *   ProductId
        
    *   CategoryId
        

### 5\. Review

Captures customer feedback on products.

*   **Properties:**
    
    *   Id
        
    *   Rating (1–5)
        
    *   Comment
        
    *   ProductId (FK)
        
    *   UserId (FK)
        
    *   CreatedAt
        
*   **Relationships:**
    
    *   Many-to-One with Product
        
    *   Many-to-One with User
        

### 6\. CartItem

Temporary storage for items in a user's cart.

*   **Properties:**
    
    *   Id
        
    *   UserId (FK)
        
    *   ProductId (FK)
        
    *   Quantity
        
*   **Relationships:**
    
    *   Many-to-One with User
        
    *   Many-to-One with Product
        

### 7\. Order

Represents a completed purchase.

*   **Properties:**
    
    *   Id
        
    *   UserId (FK)
        
    *   OrderDate
        
    *   TotalAmount
        
    *   Status (Pending, Paid, Shipped, Delivered)
        
*   **Relationships:**
    
    *   One-to-Many with OrderItem
        
    *   Many-to-One with User
        

### 8\. OrderItem

Details each product in an order.

*   **Properties:**
    
    *   Id
        
    *   OrderId (FK)
        
    *   ProductId (FK)
        
    *   Quantity
        
    *   UnitPrice
        
*   **Relationships:**
    
    *   Many-to-One with Order
        
    *   Many-to-One with Product
        

C) Controllers
--------------

### 1\. AuthController

Handles user registration, login, and JWT token issuance.

*   POST /api/auth/register
    
*   POST /api/auth/login
    
*   POST /api/auth/logout
    

### 2\. UserController (Admin-only)

Manages users and roles.

*   GET /api/users
    
*   GET /api/users/{id}
    
*   PUT /api/users/{id}
    
*   DELETE /api/users/{id}
    

### 3\. ProductController

CRUD operations for products (Seller/Admin), browsing for customers.

*   GET /api/products
    
*   GET /api/products/{id}
    
*   POST /api/products
    
*   PUT /api/products/{id}
    
*   DELETE /api/products/{id}
    

### 4\. CategoryController

Manage product categories.

*   GET /api/categories
    
*   GET /api/categories/{id}
    
*   POST /api/categories
    
*   PUT /api/categories/{id}
    
*   DELETE /api/categories/{id}
    

### 5\. ReviewController

Customer reviews and ratings.

*   GET /api/products/{productId}/reviews
    
*   POST /api/products/{productId}/reviews
    
*   PUT /api/reviews/{id}
    
*   DELETE /api/reviews/{id}
    

### 6\. CartController

Manage shopping cart items.

*   GET /api/cart
    
*   POST /api/cart
    
*   PUT /api/cart/{productId}
    
*   DELETE /api/cart/{productId}
    

### 7\. OrderController

Checkout and order history.

*   GET /api/orders
    
*   GET /api/orders/{id}
    
*   POST /api/orders
    
*   PUT /api/orders/{id}/status
    

D) Project Structure
--------------------

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   ECommerceSystem/  │── Core/              # Business Logic  │   ├── Entities/       # Models (Product, User, Order, Category, Review)  │   ├── Interfaces/      # Repository Contracts  │   └── Services/       # Business Logic (ProductService, OrderService)  │  │── Infrastructure/    # Data Access & External Services  │   ├── Data/            # DbContext & Migrations  │   ├── Repositories/    # Repository Implementations  │   ├── Identity/        # ASP.NET Core Identity Config  │   └── Payments/        # Stripe/PayPal Integration  │  │── API/                # Presentation Layer  │   ├── Controllers/     # (ProductController, OrderController, AuthController)  │   ├── DTOs/            # Data Transfer Objects  │   └── Middlewares/     # Exception Handling, JWT  │  ├── appsettings.json    # JWT, Database  └── Program.cs          # Startup Configuration   `

E) Submission Guidelines
------------------------

*   Deadline: \[Due Date\]
    
*   Submit:
    
    *   Source Code (.zip)
        
    *   Demo Video (3-5 mins, Show the core features of the project)