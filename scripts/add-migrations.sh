#!/bin/bash

# This script is used to add migrations to the project.

dotnet ef migrations add $1 --project src/Sample.Migrator/Sample.Migrator.csproj -c ApplicationDbContext
