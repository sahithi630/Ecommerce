@echo off
echo ========================================
echo    ECommerce Microservices Logs
echo ========================================
echo.

if "%1"=="" (
    echo Usage: logs.bat [service-name]
    echo.
    echo Available services:
    echo   - userservice
    echo   - productservice  
    echo   - orderservice
    echo   - apigateway
    echo   - sqlserver
    echo.
    echo Examples:
    echo   logs.bat userservice
    echo   logs.bat all          ^(shows all logs^)
    echo.
    pause
    exit /b 1
)

if "%1"=="all" (
    echo Showing logs for all services...
    docker-compose logs -f
) else (
    echo Showing logs for %1...
    docker-compose logs -f %1
)