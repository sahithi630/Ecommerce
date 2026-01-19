# ECommerce Microservices Application

A microservices-based e-commerce application built with .NET Core, featuring User Management, Product Catalog, Order Processing, and API Gateway.

## Architecture

- **UserService** (Port 5162): User registration, Role-based authentication, and JWT token management
- **ProductService** (Port 5085): Product catalog management
- **OrderService** (Port 5106): Order processing and management
- **APIGateway** (Port 5100): Centralized API gateway using Ocelot
- **SQL Server**: Database for all services

## Prerequisites

- .NET 8.0 SDK
- Docker Desktop
- Git

## Running Locally (Development)

### 1. Clone the Repository
```bash
git clone <your-repository-url>
cd ECommerce
```

### 2. Update Connection Strings
Update `appsettings.Development.json` in each service with your local SQL Server connection string.

### 3. Run Database Migrations
```bash
# UserService
cd UserServices
dotnet ef database update

# ProductService
cd ../ProductService
dotnet ef database update

# OrderService
cd ../OrderService
dotnet ef database update
```

### 4. Run Services
```bash
# Terminal 1 - UserService
cd UserServices
dotnet run

# Terminal 2 - ProductService
cd ProductService
dotnet run

# Terminal 3 - OrderService
cd OrderService
dotnet run

# Terminal 4 - API Gateway
cd APIGateway
dotnet run
```

## Running with Docker

### 1. Build and Run All Services
```bash
docker-compose up --build
```

### 2. Stop Services
```bash
docker-compose down
```

### 3. Clean Up (Remove volumes)
```bash
docker-compose down -v
```

## API Documentation

Once running, access Swagger documentation:

- **UserService**: http://localhost:5162/swagger
- **ProductService**: http://localhost:5085/swagger
- **OrderService**: http://localhost:5106/swagger
- **API Gateway**: http://localhost:5100

## Service Endpoints

### UserService (http://localhost:5162)
- `POST /api/user/register` - Register new user
- `POST /api/user/login` - User login
- `GET /api/user/{id}` - Get user by ID

### ProductService (http://localhost:5085)
- `GET /api/product` - Get all products
- `GET /api/product/{id}` - Get product by ID
- `POST /api/product` - Create product (Auth required)
- `PUT /api/product/{id}` - Update product (Auth required)
- `DELETE /api/product/{id}` - Delete product (Auth required)

### OrderService (http://localhost:5106)
- `GET /api/order` - Get user orders (Auth required)
- `GET /api/order/{id}` - Get order by ID (Auth required)
- `POST /api/order` - Create order (Auth required)

## Authentication

The application uses JWT Bearer tokens for authentication:

1. Register/Login via UserService to get JWT token
2. Include token in Authorization header: `Bearer <your-token>`
3. Token is valid for API calls to protected endpoints

## Database

Each service has its own database:
- `UserServiceDB` - User data
- `ProductServiceDB` - Product catalog
- `OrderServiceDB` - Order information

## Docker Configuration

The application uses multi-container setup:
- SQL Server container for database
- Individual containers for each microservice
- Shared network for inter-service communication

## Testing

Import the provided Postman collection to test all APIs with pre-configured requests and authentication flows.

## Troubleshooting

### Common Issues

1. **Port conflicts**: Ensure ports 5100, 5162, 5106, 5085, 1433 are available
2. **Database connection**: Wait for SQL Server container to be ready before services start
3. **JWT errors**: Ensure all services use the same JWT secret key

### Logs
```bash
# View service logs
docker-compose logs <service-name>

# Follow logs
docker-compose logs -f <service-name>
```

## Development

### Adding New Features
1. Create feature branch
2. Implement changes
3. Update tests
4. Update documentation
5. Create pull request

### Database Changes
```bash
# Add migration
dotnet ef migrations add <MigrationName>

# Update database
dotnet ef database update
```
