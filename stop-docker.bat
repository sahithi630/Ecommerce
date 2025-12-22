@echo off
echo ========================================
echo    Stopping ECommerce Microservices
echo ========================================
echo.

echo Stopping all services...
docker-compose down

echo.
echo Removing unused containers and networks...
docker system prune -f

echo.
echo ========================================
echo      All services stopped successfully
echo ========================================
echo.
echo To remove all data (including database), run:
echo   docker-compose down -v
echo.
pause