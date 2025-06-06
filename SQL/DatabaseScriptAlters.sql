/* This SQL script alters the FlightPlanner table to add new columns.
   
ALTER TABLE "FlightPlanner" ADD COLUMN "FullFlightName" VARCHAR(50) NOT NULL
ALTER TABLE "FlightPlanner" ADD COLUMN "AircraftModel" INTEGER NOT NULL DEFAULT 0
ALTER TABLE "FlightPlanner" ADD COLUMN "FlightType" INTEGER NOT NULL DEFAULT 0
ALTER TABLE "FlightPlanner" ADD COLUMN "ArrivalRunwayLength" INTEGER NULL
ALTER TABLE "FlightPlanner" ADD COLUMN "LocalizerVectorAltitude" INTEGER NULL
ALTER TABLE "FlightPlanner" ADD COLUMN "AltitudeFeet" INTEGER NULL
             
*/