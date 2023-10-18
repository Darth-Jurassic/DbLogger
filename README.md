# Hexagon architectures in .NET Core - operations logger sample

This sample demonstrates how to implement the Hexagonal Architecture in .NET Core via C#.

## Introduction

This sample contains two interacting domains:
- **Sample** - the domain that is responsible for demonstrating the usage of DbLogger when working with simple entities (Employee, Company);
- **DbLogger** - the very small domain that is responsible for logging operations into the database.

The DbLogger is reusable by any other DbContext by simply inheriting from LoggingDbContext and providing a proper logging strategy.

## Architecture

Main idea of the hexagonal architecture is to separate the core logic of the domain from the infrastructure. This is achieved by defining the interfaces of the application services in the core project and implementing them in the infrastructure project. This allows to easily replace the infrastructure implementation without changing the core logic.

![Sample Architecture](docs/Sample%20Architecture.png)

In addition there is a separate Seeder application that is responsible for creating the database schema.

![Sample Migrations.png](docs%2FSample%20Migrations.png)

Sample implementation contains the following projects from left to right:
- **Sample.Api.Web** — the web API that is responsible for exposing the functionality of the Sample domain.
- **Sample.Core** — core logic of the Sample domain.
- **Sample.Abstractions** — for inversion of control purposes it is necessary to define the interfaces of the application services in a separate project. This project is referenced by the Sample.Core project and all infrastructure implementations, e.g. Sample.Persistence.Npgsql.
- **Sample.Persistence** — contains the implementation of persistence into PostgreSQL database.
- **Sample.Host** — contains the web application that is responsible for hosting the entire sample.
- **Sample.Migrator** — contains additional application that is responsible for creating and migrating the database schema.

DbLogging implementation contains the following projects from left to right:
- **DbLogging** — implementation of the logging functionality as a reusable library.
- **DbLogging.Tests** — a very simple unit test for LoggingDbContext.

Key Artifacts:
- **docker-compose.yml** — contains the definition of the docker-compose environment that is used to run the sample.
- **Dockerfile-Host** — contains the definition of the docker image that is used to build the Sample.Host container.
- **Dockerfile-Migrator** — contains the definition of the docker image that is used to build the Sample.Migrator container.

## How to build

### Prerequisites

The sample requires the following software installed on the build machine:
- .NET 7 SDK
- Visual Studio, Rider or any other IDE to build the solution

### Process

Main delivery artifact is the Sample.Host project. It is a web application that is responsible for hosting the entire sample. It can be built by running the following command in the root folder of the repository:

```bash
dotnet build
```

# Build and run

## Running in Docker

If you don't have PostgreSQL and .NET 7 installed or just want to omit any conflicts with your local environment, you can run the sample in Docker. 

### Prerequisites
- Docker
- Docker Compose
- Following images:
  - postgres:16
  - mcr.microsoft.com/dotnet/sdk:7.0
  - mcr.microsoft.com/dotnet/aspnet:7.0
  - mcr.microsoft.com/dotnet/runtime:7.0
  - or access to https://hub.docker.com/ to download them

### Process

When running the sample, the following deployment scheme is going to be created:

![Build And Run In Docker.png](docs%2FBuild%20And%20Run%20In%20Docker.png)

The sample can be run by executing the following command in the root folder of the repository:

```bash
docker-compose up --build
```

As a result, the following containers are going to be created:
1. **postgres** — PostgreSQL DBMS will start and stay online,
2. **migrator** — container responsible for creating and migrating the database schema will start, perform migrations and exit,
3. **host** — container running the sample will start.

NOTE: that for several times both migrator and service may show errors like:

```
dblogger-migrator-1  |       Application is shutting down...
dblogger-migrator-1  | Unhandled exception. Npgsql.NpgsqlException (0x80004005): Failed to connect to 172.23.0.2:5432
dblogger-migrator-1  |  ---> System.Net.Sockets.SocketException (111): Connection refused**
```

Stay calm, it's ok. It happens because the service starts faster than the database. The service will restart and retry to connect to the database and will succeed soon.

## Running locally

When running locally, the following deployment scheme is going to be created:

![Build And Run Locally.png](docs%2FBuild%20And%20Run%20Locally.png)

If you have PostgreSQL and .NET 7 installed, you can run the sample locally.

### Prerequisites
- PostgreSQL 16 (local or remote installation)
- .NET 7 SDK
- Visual Studio, Rider or any other IDE to build and the solution

### Process

1. Create login and database in your PostgreSQL installation.
2. Update the ConnectionStrings.DefaultConnection setting in the appsettings.Development.json file of the Sample.Host and Sample.Migrator projects.
3. Run the Sample.Migrator project to create and migrate the database schema.
4. Run the Sample.Host project to start the sample.