/* This SQL script alters the FlightPlanner table to add new columns.
   

CREATE TABLE "FlightPlanner" (
    "ID" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Date" DATE NOT NULL,
    "ICAODeparture" VARCHAR(10) NOT NULL,
    "DepartureAirportName" VARCHAR(100) NULL,
    "BaroPressureDeparture" NUMERIC NULL,
    "TransitionAltitudeDeparture" INTEGER NULL,
    "DepartureRunway" INTEGER NULL,
    "ICAOArrival" VARCHAR(10) NOT NULL,
    "ArrivalAirportName" VARCHAR(100) NULL,
    "ArrivalRunway" INTEGER NULL,
    "BaroPressureArrival" NUMERIC NULL,
    "TransitionAltitudeArrival" INTEGER NULL,
    "ArrivalRunwayElevation" INTEGER NULL,
    "ArrivalRunwayMinimumAltitude" INTEGER NULL,
    "LocalizerFrequency" INTEGER NULL,
    "LocalizerVectorName" VARCHAR(50) NULL,
    "ApproachType" INTEGER NULL
)

CREATE TABLE "FlightSpecs" (
    "ID" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "PlannerID" UUID NOT NULL,
    "NauticalMiles" INTEGER NOT NULL,
    "CruiseSpeedKnots" INTEGER NOT NULL,
    "FlightEstimatedHourTime" INTEGER NULL,
    "FlightEstimatedMinutesTime" INTEGER  NULL,
    CONSTRAINT fk_planner FOREIGN KEY ("PlannerID") REFERENCES "FlightPlanner"("ID") ON DELETE CASCADE
)
               
*/