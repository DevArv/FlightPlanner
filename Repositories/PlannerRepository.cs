using FlightPlanner.DatabaseConnection;
using FlightPlanner.Models;
using FlightPlanner.ViewModels;

namespace FlightPlanner.Repositories;

public class PlannerRepository
{
    public Task<bool> ValidateAsync(FlightPlannerSimpleViewModel Obj)
    {
        //Required fields validation
        if (string.IsNullOrWhiteSpace(Obj.ICAODeparture))
            throw new ApplicationException("El campo ICAO de salida es obligatorio.");
        
        if (string.IsNullOrWhiteSpace(Obj.ICAOArrival))
            throw new ApplicationException("El campo ICAO de llegada es obligatorio.");

        return Task.FromResult(true);
    }
    
    public FlightPlannerSimpleViewModel MapToSimpleViewModel(Planner Record)
    {
        if (Record == null)
            throw new ApplicationException("Plan de vuelo no encontrado.");

        return new FlightPlannerSimpleViewModel
        {
            ID = Record.ID,
            Date = Record.Date,
            ICAODeparture = Record.ICAODeparture,
            DepartureAirportName = Record.DepartureAirportName,
            BaroPressureDeparture = Record.BaroPressureDeparture,
            TransitionAltitudeDeparture = Record.TransitionAltitudeDeparture,
            DepartureRunway = Record.DepartureRunway,
            ICAOArrival = Record.ICAOArrival,
            ArrivalAirportName = Record.ArrivalAirportName,
            ArrivalRunway = Record.ArrivalRunway,
            BaroPressureArrival = Record.BaroPressureArrival,
            TransitionAltitudeArrival = Record.TransitionAltitudeArrival,
            ArrivalRunwayElevation = Record.ArrivalRunwayElevation,
            ArrivalRunwayMinimumAltitude = Record.ArrivalRunwayMinimumAltitude,
            LocalizerFrequency = Record.LocalizerFrequency,
            LocalizerVectorName = Record.LocalizerVectorName,
            ApproachType = Record.ApproachType
        };
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
                FullFlightName = $"{ViewModel.ICAODeparture} -> {ViewModel.ICAOArrival}"
            };

            context.FlightPlanner.Add(planner);
            await context.SaveChangesAsync();
            transaction.Commit();
            
            return planner.ID;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new ApplicationException($"Error al guardar el plan de vuelo", ex);
        }
    }
}