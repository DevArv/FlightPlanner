/* This SQL script alters the FlightPlanner table to add new columns.
   
ALTER TABLE "FlightPlanner" ADD COLUMN "FullFlightName" VARCHAR(50) NOT NULL;
ALTER TABLE "FlightPlanner" ADD COLUMN "AircraftModel" INTEGER NOT NULL DEFAULT 0;
ALTER TABLE "FlightPlanner" ADD COLUMN "FlightType" INTEGER NOT NULL DEFAULT 0;
ALTER TABLE "FlightPlanner" ADD COLUMN "ArrivalRunwayLength" INTEGER NULL;
ALTER TABLE "FlightPlanner" ADD COLUMN "LocalizerVectorAltitude" INTEGER NULL;
   
ALTER TABLE "FlightSpecs" ADD COLUMN "BasicFuel" INTEGER NULL;
ALTER TABLE "FlightSpecs" ADD COLUMN "AverageFuelConsumption" NUMERIC NOT NULL;
ALTER TABLE "FlightSpecs" ADD COLUMN "ReserveFuel" INTEGER NOT NULL;
ALTER TABLE "FlightSpecs" ADD COLUMN "EmergencyFuel" INTEGER NOT NULL;
   
ALTER TABLE "FlightSpecs" ADD COLUMN "ReserveFuelGal" NUMERIC NOT NULL;
ALTER TABLE "FlightSpecs" ADD COLUMN "EmergencyFuelGal" NUMERIC NOT NULL;
ALTER TABLE "FlightSpecs" ADD COLUMN "TotalFuel" NUMERIC NOT NULL;
ALTER TABLE "FlightSpecs" ADD COLUMN "TotalFuelGal" NUMERIC NOT NULL;
   
ALTER TABLE "FlightSpecs" ADD COLUMN "AltitudeFeet" INTEGER NULL;
   
ALTER TABLE "FlightPlanner" ADD COLUMN "IsCompleted" BOOLEAN NOT NULL DEFAULT FALSE;
*/