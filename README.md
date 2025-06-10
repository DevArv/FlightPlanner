# FlightPlanner Docker Setup

This project can be built and run using Docker. Ensure you provide a PostgreSQL
connection string in the `ConnectionStrings__DefaultConnection` environment
variable when running the container.

## Building the image

```bash
docker build -t flightplanner .
```

## Running the container

```bash
docker run -e ConnectionStrings__DefaultConnection="<connection string>" -p 8080:80 flightplanner
```

The application will then be available at
[http://localhost:8080](http://localhost:8080).
