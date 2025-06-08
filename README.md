# FlightPlanner Docker Setup

This project can be built and run using Docker. A `docker-compose.yml` file is provided that starts the application and a PostgreSQL database.

## Building the image

```bash
docker compose build
```

## Running the stack

```bash
docker compose up
```

The application will be available at [http://localhost:8080](http://localhost:8080).

Database data is stored in a named volume `db-data`.

The database connection string is provided via the
`ConnectionStrings__DefaultConnection` environment variable in
`docker-compose.yml`. The application now reads this value so that it can
successfully connect to the PostgreSQL container when running with Docker.