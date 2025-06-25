#!/bin/bash

echo "Seeding the database..."

# Change these if needed
DB_NAME="ShortieDb"
DB_USER="sa"
DB_HOST="localhost"
DB_PORT="1433"
DB_PASS="Nenormalen0252"

export PGPASSWORD=$DB_PASS

psql "host=$DB_HOST port=$DB_PORT user=$DB_USER dbname=$DB_NAME" -f "$(dirname "$0")/seed.sql"

unset PGPASSWORD
