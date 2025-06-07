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
        
        if (Obj.FlightSpecs.NauticalMiles == null || Obj.FlightSpecs.NauticalMiles <= 0)
            throw new ApplicationException("Las Millas Náuticas deben ser mayores a cero.");
        
        if (Obj.FlightSpecs.CruiseSpeedKnots == null || Obj.FlightSpecs.CruiseSpeedKnots <= 0)
            throw new ApplicationException("La Velocidad Crucero deben ser mayores a cero.");
        
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
            // Generate a new ID when creating the record if none was supplied
            var plannerID = ViewModel.ID == Guid.Empty ? Guid.NewGuid() : ViewModel.ID;

            var planner = new Planner
            {
                ID = plannerID,
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
        
        var query = await (from p in context.FlightPlanner.AsNoTracking()
                join s in context.FlightSpecs.AsNoTracking() on p.ID equals s.PlannerID into plannerGroup
                from s in plannerGroup.DefaultIfEmpty()
                where p.ID == ID
                select new FlightPlannerDetailsViewModel
                {
                    ID = p.ID,
                    Date = p.Date,
                    ICAODeparture = p.ICAODeparture,
                    DepartureAirportName = p.DepartureAirportName,
                    BaroPressureDeparture = p.BaroPressureDeparture,
                    TransitionAltitudeDeparture = p.TransitionAltitudeDeparture,
                    DepartureRunway = p.DepartureRunway,
                    ICAOArrival = p.ICAOArrival,
                    ArrivalAirportName = p.ArrivalAirportName,
                    ArrivalRunway = p.ArrivalRunway,
                    BaroPressureArrival = p.BaroPressureArrival,
                    TransitionAltitudeArrival = p.TransitionAltitudeArrival,
                    ArrivalRunwayElevation = p.ArrivalRunwayElevation,
                    ArrivalRunwayMinimumAltitude = p.ArrivalRunwayMinimumAltitude,
                    LocalizerFrequency = p.LocalizerFrequency,
                    LocalizerVectorName = p.LocalizerVectorName,
                    ApproachType = p.ApproachType,
                    AircraftModel = p.AircraftModel,
                    FlightType = p.FlightType,
                    ArrivalRunwayLength = p.ArrivalRunwayLength,
                    AltitudeFeet = p.AltitudeFeet,
                    LocalizerVectorAltitude = p.LocalizerVectorAltitude,
                    FullFlightName = p.FullFlightName,
                    FlightSpecs = s == null ? new FlightSpecsDetailsViewModel() : new FlightSpecsDetailsViewModel
                    {
                        NauticalMiles = s.NauticalMiles,
                        CruiseSpeedKnots = s.CruiseSpeedKnots,
                        FlightEstimatedHourTime = s.FlightEstimatedHourTime,
                        FlightEstimatedMinutesTime = s.FlightEstimatedMinutesTime
                    }
                }).FirstOrDefaultAsync();

        if (query == null)
            throw new ApplicationException("El plan de vuelo no existe o ha sido eliminado.");

        return query;
    }

    public async Task<List<FlightPlannerDetailsViewModel>> GetTableFlightPlansAsync()
    {
        await using var context = new FlightPlannerContext();
        
        var query = await (from p in context.FlightPlanner.AsNoTracking()
                orderby p.Date descending
                select new FlightPlannerDetailsViewModel
                {
                    ID = p.ID,
                    Date = p.Date,
                    FullFlightName = p.FullFlightName,
                    DepartureAirportName = p.DepartureAirportName,
                    ArrivalAirportName = p.ArrivalAirportName,
                    DepartureRunway = p.DepartureRunway,
                    ArrivalRunway = p.ArrivalRunway
                }).ToListAsync();

        return query;
    }
}