#!/bin/bash

echo "Starting setup..."

# --- CONFIGURATION ---
DB_NAME="ShortieDb"
DB_USER="sa"
DB_HOST="localhost"
DB_PORT="1433"
DB_PASS="SuperSecretPassword123"
PROJECT_NAME="ShortieProgress.Api"
CONTEXT="ShortieDbContext"
CONTAINER_NAME="shortie-mssql"
# ----------------------

# --- 1. Start Docker MSSQL ---
echo "Starting MSSQL container via docker-compose..."
docker-compose up -d

echo "Sleeping for 20 seconds while SQL Server starts..." # This is not an optimal solution, it will be changed in later development
sleep 20

# --- 1. Run EF Migrations ---
echo "Applying EF Core migrations..."
dotnet ef database update --project "$PROJECT_NAME" --context "$CONTEXT"

# --- 2. Seed the DB ---
echo "Seeding the database with test data..."

# Create temp file with SQL and prepend CREATE DATABASE if not exists (optional)
export MSSQL_PASSWORD=$DB_PASS

sqlcmd -S $DB_HOST,$DB_PORT -U $DB_USER -P $DB_PASS -d $DB_NAME -i "$(dirname "$0")/seed.sql"

echo "Setup complete!"
