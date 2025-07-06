/* This SQL script alters the FlightPlanner table to add new columns.
   
ALTER TABLE "FlightPlanner" ADD COLUMN "FullFlightName" VARCHAR(50) NOT NULL DEFAULT '';
ALTER TABLE "FlightPlanner" ADD COLUMN "AircraftModel" INTEGER NOT NULL DEFAULT 0;
ALTER TABLE "FlightPlanner" ADD COLUMN "FlightType" INTEGER NOT NULL DEFAULT 0;
ALTER TABLE "FlightPlanner" ADD COLUMN "ArrivalRunwayLength" INTEGER NOT NULL DEFAULT 0;
ALTER TABLE "FlightPlanner" ADD COLUMN "LocalizerVectorAltitude" INTEGER NOT NULL DEFAULT 0;
   
ALTER TABLE "FlightSpecs" ADD COLUMN "BasicFuel" INTEGER NULL;
ALTER TABLE "FlightSpecs" ADD COLUMN "AverageFuelConsumption" NUMERIC(10, 2) NOT NULL DEFAULT 0.00;
ALTER TABLE "FlightSpecs" ADD COLUMN "ReserveFuel" INTEGER NOT NULL DEFAULT 0;
   
ALTER TABLE "FlightSpecs" ADD COLUMN "ReserveFuelGal" NUMERIC(10, 2) NOT NULL;
ALTER TABLE "FlightSpecs" ADD COLUMN "TotalFuel" NUMERIC(10, 2) NOT NULL;
ALTER TABLE "FlightSpecs" ADD COLUMN "TotalFuelGal" NUMERIC(10, 2) NOT NULL;
   
ALTER TABLE "FlightSpecs" ADD COLUMN "AltitudeFeet" INTEGER NOT NULL DEFAULT 0;
   
ALTER TABLE "FlightPlanner" ADD COLUMN "IsCompleted" BOOLEAN NOT NULL DEFAULT FALSE;
*/