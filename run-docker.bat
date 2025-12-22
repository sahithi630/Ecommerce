@echo off
echo ========================================
echo    ECommerce Microservices Launcher
echo ========================================
echo.

REM Check if Docker is running
docker info >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: Docker is not running. Please start Docker Desktop first.
    pause
    exit /b 1
)

echo Building and starting all services...
echo This may take a few minutes on first run...
echo.

REM Build and start all services
docker-compose up --build -d

if %errorlevel% neq 0 (
    echo ERROR: Failed to start services. Check docker-compose.yml
    pause
    exit /b 1
)

echo.
echo Waiting for services to initialize...
timeout /t 45 /nobreak

echo.
echo ========================================
echo         Services are now running!
echo ========================================
echo.
echo API Endpoints:
echo   API Gateway:     http://localhost:5100
echo   User Service:    http://localhost:5162/swagger
echo   Product Service: http://localhost:5085/swagger
echo   Order Service:   http://localhost:5106/swagger
echo.
echo Database:
echo   SQL Server:      localhost:1433
echo   Username: sa
echo   Password: YourStrong@Passw0rd
echo.
echo Management Commands:
echo   View logs:       docker-compose logs -f [service-name]
echo   Stop services:   docker-compose down
echo   Restart:         docker-compose restart
echo   Clean up:        docker-compose down -v
echo.
echo Import 'ECommerce-API-Collection.postman_collection.json' into Postman for testing
echo.
pause