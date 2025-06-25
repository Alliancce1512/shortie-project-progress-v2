#!/bin/bash

echo "Starting full backend setup..."

# --- CONFIGURATION ---
DB_NAME="ShortieDb"
DB_USER="sa"
DB_HOST="localhost"
DB_PORT="1433"
DB_PASS="SuperSecretPassword123"
PROJECT_NAME="ShortieProgress.Api"
CONTEXT="ShortieDbContext"
# ----------------------

# --- 1. Start Docker MSSQL ---
echo "Starting MSSQL container via docker-compose..."
docker-compose up -d

# --- 2. Wait for MSSQL to be ready ---
echo "Waiting for SQL Server to be ready..."
RETRIES=10
until docker exec shortie-mssql /opt/mssql-tools/bin/sqlcmd -S localhost -U $DB_USER -P $DB_PASS -Q "SELECT 1" &> /dev/null || [ $RETRIES -eq 0 ]; do
  echo "Waiting... ($RETRIES)"
  sleep 3
  ((RETRIES--))
done

if [ $RETRIES -eq 0 ]; then
  echo "SQL Server did not start in time."
  exit 1
fi

# --- 3. Run EF Migrations ---
echo "Applying EF Core migrations..."
dotnet ef database update --project "$PROJECT_NAME" --context "$CONTEXT"

# --- 4. Seed the DB ---
echo "Seeding the database with test data..."
sqlcmd -S $DB_HOST,$DB_PORT -U $DB_USER -P $DB_PASS -d $DB_NAME -i "$(dirname "$0")/seed.sql"

echo "Setup complete!"
