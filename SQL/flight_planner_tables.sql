-- This SQL script creates the FlightPlanner table
"
CREATE TABLE "FlightPlanner" (
	"ID" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
	"Date" DATE NOT NULL,
	"ICAODeparture" VARCHAR(10) NOT NULL,
	"DepartureAirportName" VARCHAR(100) NULL NULL,
	"BaroPressureDeparture" DOUBLE PRECISION NOT NULL,
	"TransitionAltitudeDeparture" INTEGER NOT NULL,
	"DepartureRunway" INTEGER NOT NULL,
	"ICAOArrival" VARCHAR(10) NOT NULL,
	"ArrivalAirportName" VARCHAR(100) NOT NULL,
	"ArrivalRunway" INTEGER NOT NULL,
	"BaroPressureArrival" DOUBLE PRECISION NOT NULL,
	"TransitionAltitudeArrival" INTEGER NOT NULL,
	"ArrivalRunwayElevation" INTEGER NOT NULL,
	"ArrivalRunwayMinimumAltitude" INTEGER NOT NULL,
	"LocalizerFrequency" INTEGER NOT NULL,
	"LocalizerVectorName" VARCHAR(50) NOT NULL,
	"ApproachType" INTEGER NOT NULL
)
              
CREATE TABLE "FlightSpecs" (
    "ID" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "PlannerID" UUID NOT NULL,
    "NauticalMiles" INTEGER NOT NULL,
    "CruiseSpeedKnots" INTEGER NOT NULL,
    "FlightEstimatedHourTime" INTEGER NOT NULL,
    "FlightEstimatedMinutesTime" INTEGER NOT NULL,
    CONSTRAINT fk_planner FOREIGN KEY ("PlannerID") REFERENCES "FlightPlanner"("ID") ON DELETE CASCADE
)
               
";