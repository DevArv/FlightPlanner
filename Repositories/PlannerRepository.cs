using FlightPlanner.DatabaseConnection;
using FlightPlanner.Enums;
using FlightPlanner.Models;
using FlightPlanner.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Repositories;

public class PlannerRepository
{
    public Task<bool> ValidateAsync(FlightPlannerSimpleViewModel Obj)
    {
        //Required fields validation
        if (string.IsNullOrWhiteSpace(Obj.ICAODeparture))
            throw new ApplicationException("El campo ICAO de salida es requerido.");
        
        if (string.IsNullOrWhiteSpace(Obj.ICAOArrival))
            throw new ApplicationException("El campo ICAO de llegada es requerido.");
        
        if (Obj.FlightSpecs.NauticalMiles == null)
            throw new ApplicationException("Las Millas Náuticas deben ser mayores a cero.");
        
        if (Obj.AircraftModel == AircraftModelEnum.DEFAULT)
            throw new ApplicationException("El modelo de Avión es requerido.");
        
        if (Obj.FlightType == FlightTypesEnum.DEFAULT)
            throw new ApplicationException("El tipo de vuelo es requerido.");

        return Task.FromResult(true);
    }

    public async Task<Guid> SaveFlightPlanAsync(FlightPlannerSimpleViewModel ViewModel)
    {
        await using var context = new FlightPlannerContext();
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            var planner = new Planner
            {
                ID = ViewModel.ID,
                Date = ViewModel.Date,
                ICAODeparture = ViewModel.ICAODeparture,
                DepartureAirportName = ViewModel.DepartureAirportName,
                BaroPressureDeparture = ViewModel.BaroPressureDeparture,
                TransitionAltitudeDeparture = ViewModel.TransitionAltitudeDeparture,
                DepartureRunway = ViewModel.DepartureRunway,
                ICAOArrival = ViewModel.ICAOArrival,
                ArrivalAirportName = ViewModel.ArrivalAirportName,
                ArrivalRunway = ViewModel.ArrivalRunway,
                BaroPressureArrival = ViewModel.BaroPressureArrival,
                TransitionAltitudeArrival = ViewModel.TransitionAltitudeArrival,
                ArrivalRunwayElevation = ViewModel.ArrivalRunwayElevation,
                ArrivalRunwayMinimumAltitude = ViewModel.ArrivalRunwayMinimumAltitude,
                LocalizerFrequency = ViewModel.LocalizerFrequency,
                LocalizerVectorName = ViewModel.LocalizerVectorName,
                ApproachType = ViewModel.ApproachType,
                AircraftModel = ViewModel.AircraftModel,
                FlightType = ViewModel.FlightType,
                ArrivalRunwayLength = ViewModel.ArrivalRunwayLength,
                AltitudeFeet = ViewModel.AltitudeFeet,
                LocalizerVectorAltitude = ViewModel.LocalizerVectorAltitude,
                
                FullFlightName = $"{ViewModel.ICAODeparture} -> {ViewModel.ICAOArrival}",
            };

            context.FlightPlanner.Add(planner);
            await context.SaveChangesAsync();

            var nauticalMiles = ViewModel.FlightSpecs?.NauticalMiles ?? 0;
            var speed = ViewModel.FlightSpecs?.CruiseSpeedKnots ?? 1;
            
            decimal hoursDecimal = (decimal)nauticalMiles / speed;
            decimal flightEstimatedHourTime = Math.Round(hoursDecimal, 2);
            int flightEstimatedMinutesTime = (int)(flightEstimatedHourTime * 60);

            var flightSpecs = new FlightSpecs
            {
                PlannerID = planner.ID,
                NauticalMiles = ViewModel.FlightSpecs?.NauticalMiles ?? 0,
                CruiseSpeedKnots = ViewModel.FlightSpecs?.CruiseSpeedKnots ?? 0,
                FlightEstimatedHourTime = flightEstimatedHourTime,
                FlightEstimatedMinutesTime = flightEstimatedMinutesTime
            };
            
            context.FlightSpecs.Add(flightSpecs);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return planner.ID;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new ApplicationException($"Error al guardar el plan de vuelo", ex);
        }
    }

    public async Task<FlightPlannerDetailsViewModel> GetFlightPlanAsync(Guid ID)
    {
        await using var context = new FlightPlannerContext();
        
        var planner = await context.FlightPlanner.AsNoTracking()
            .FirstOrDefaultAsync(p => p.ID == ID);

        if (planner == null)
            throw new ApplicationException("El plan de vuelo no existe o ha sido eliminado.");
        
        var specs = await context.FlightSpecs.AsNoTracking()
            .FirstOrDefaultAsync(s => s.PlannerID == planner.ID);

        return new FlightPlannerDetailsViewModel
        {
            ID = planner.ID,
            Date = planner.Date,
            ICAODeparture = planner.ICAODeparture,
            DepartureAirportName = planner.DepartureAirportName,
            BaroPressureDeparture = planner.BaroPressureDeparture,
            TransitionAltitudeDeparture = planner.TransitionAltitudeDeparture,
            DepartureRunway = planner.DepartureRunway,
            ICAOArrival = planner.ICAOArrival,
            ArrivalAirportName = planner.ArrivalAirportName,
            ArrivalRunway = planner.ArrivalRunway,
            BaroPressureArrival = planner.BaroPressureArrival,
            TransitionAltitudeArrival = planner.TransitionAltitudeArrival,
            ArrivalRunwayElevation = planner.ArrivalRunwayElevation,
            ArrivalRunwayMinimumAltitude = planner.ArrivalRunwayMinimumAltitude,
            LocalizerFrequency = planner.LocalizerFrequency,
            LocalizerVectorName = planner.LocalizerVectorName,
            ApproachType = planner.ApproachType,
            AircraftModel = planner.AircraftModel,
            FlightType = planner.FlightType,
            ArrivalRunwayLength = planner.ArrivalRunwayLength,
            AltitudeFeet = planner.AltitudeFeet,
            LocalizerVectorAltitude = planner.LocalizerVectorAltitude,
            FullFlightName = planner.FullFlightName,
            FlightSpecs = specs == null
                ? new FlightSpecsDetailsViewModel()
                : new FlightSpecsDetailsViewModel()
                {
                    NauticalMiles = specs.NauticalMiles,
                    CruiseSpeedKnots = specs.CruiseSpeedKnots,
                    FlightEstimatedHourTime = specs.FlightEstimatedHourTime,
                    FlightEstimatedMinutesTime = specs.FlightEstimatedMinutesTime
                }
        };
    }
}