using FlightPlanner.Constants;
using FlightPlanner.DatabaseConnection;
using FlightPlanner.Enum;
using FlightPlanner.Models;
using FlightPlanner.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Repositories;

public class PlannerRepository
{
    public bool Validate(FlightPlannerSimpleViewModel Obj)
    {
        //Required fields validation
        if (string.IsNullOrWhiteSpace(Obj.ICAODeparture))
            throw new ApplicationException("El campo ICAO de salida es requerido.");
        
        if (string.IsNullOrWhiteSpace(Obj.ICAOArrival))
            throw new ApplicationException("El campo ICAO de llegada es requerido.");
        
        if (Obj.FlightSpecs.NauticalMiles <= 0)
            throw new ApplicationException("Las Millas Náuticas deben ser mayores a cero.");
        
        if (Obj.AircraftModel == AircraftModelEnum.DEFAULT)
            throw new ApplicationException("El modelo de Avión es requerido.");
        
        if (Obj.FlightType == FlightTypesEnum.DEFAULT)
            throw new ApplicationException("El tipo de vuelo es requerido.");
        
        if (Obj.ApproachType == ApproachTypeEnum.ILS && 
            (!Obj.LocalizerFrequency.HasValue || Obj.LocalizerFrequency <= 0))
            throw new ApplicationException("La frecuencia del Localizador es requerida para el tipo de aproximación ILS.");

        return true;
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
                LocalizerVectorAltitude = ViewModel.LocalizerVectorAltitude,
                IsCompleted = ViewModel.IsCompleted,
                
                FullFlightName = $"{ViewModel.ICAODeparture} -> {ViewModel.ICAOArrival}",
            };

            context.FlightPlanner.Add(planner);
            await context.SaveChangesAsync();

            var nauticalMiles = ViewModel.FlightSpecs?.NauticalMiles ?? 0;

            //Calculate flight specs
            var flightSpecs = CalculateFlightSpecs(
                PlannerID: planner.ID,
                NauticalMiles: nauticalMiles,
                AircraftModel: ViewModel.AircraftModel,
                FlightType: ViewModel.FlightType);
            
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
                    LocalizerVectorAltitude = p.LocalizerVectorAltitude,
                    FullFlightName = p.FullFlightName,
                    IsCompleted = p.IsCompleted,
                    FlightSpecs = s == null ? new FlightSpecsDetailsViewModel() : new FlightSpecsDetailsViewModel
                    {
                        NauticalMiles = s.NauticalMiles,
                        CruiseSpeedKnots = s.CruiseSpeedKnots,
                        FlightEstimatedHourTime = s.FlightEstimatedHourTime,
                        FlightEstimatedMinutesTime = s.FlightEstimatedMinutesTime,
                        BasicFuel = s.BasicFuel,
                        AverageFuelConsumption = s.AverageFuelConsumption,
                        ReserveFuel = s.ReserveFuel,
                        ReserveFuelGal = s.ReserveFuelGal,
                        TotalFuel = s.TotalFuel,
                        TotalFuelGal = s.TotalFuelGal,
                        AltitudeFeet = s.AltitudeFeet,
                        AscentRateFeetPerMinute = s.AscentRateFeetPerMinute,
                        DescentRateFeetPerMinute = s.DescentRateFeetPerMinute,
                        FinalApproachSpeed = s.FinalApproachSpeed,
                        DescentVerticalSpeed = s.DescentVerticalSpeed
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

    public async Task DeleteFlightPlanAsync(Guid ID)
    {
        await using var context = new FlightPlannerContext();
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var planner = await context.FlightPlanner.FirstOrDefaultAsync(p => p.ID == ID);
            if (planner == null)
                throw new ApplicationException("El plan de vuelo no existe o ha sido eliminado.");

            var specs = await context.FlightSpecs.Where(s => s.PlannerID == ID).ToListAsync();
            context.FlightSpecs.RemoveRange(specs);
            context.FlightPlanner.Remove(planner);
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new ApplicationException($"Error al eliminar el plan de vuelo: {ex.Message}");
        }
    }

    public async Task UpdateFlightPlanAsync(FlightPlannerSimpleViewModel ViewModel)
    {
        await using var context = new FlightPlannerContext();
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var planner = await context.FlightPlanner.FirstOrDefaultAsync(p => p.ID == ViewModel.ID);
            if (planner == null)
                throw new ApplicationException("El plan de vuelo no existe o ha sido eliminado.");
            
            planner.Date = ViewModel.Date;
            planner.ICAODeparture = ViewModel.ICAODeparture;
            planner.DepartureAirportName = ViewModel.DepartureAirportName;
            planner.BaroPressureDeparture = ViewModel.BaroPressureDeparture;
            planner.TransitionAltitudeDeparture = ViewModel.TransitionAltitudeDeparture;
            planner.DepartureRunway = ViewModel.DepartureRunway;
            planner.ICAOArrival = ViewModel.ICAOArrival;
            planner.ArrivalAirportName = ViewModel.ArrivalAirportName;
            planner.ArrivalRunway = ViewModel.ArrivalRunway;
            planner.BaroPressureArrival = ViewModel.BaroPressureArrival;
            planner.TransitionAltitudeArrival = ViewModel.TransitionAltitudeArrival;
            planner.ArrivalRunwayElevation = ViewModel.ArrivalRunwayElevation;
            planner.ArrivalRunwayMinimumAltitude = ViewModel.ArrivalRunwayMinimumAltitude;
            planner.LocalizerFrequency = ViewModel.LocalizerFrequency;
            planner.LocalizerVectorName = ViewModel.LocalizerVectorName;
            planner.ApproachType = ViewModel.ApproachType;
            planner.AircraftModel = ViewModel.AircraftModel;
            planner.FlightType = ViewModel.FlightType;
            planner.ArrivalRunwayLength = ViewModel.ArrivalRunwayLength;
            planner.LocalizerVectorAltitude = ViewModel.LocalizerVectorAltitude;
            planner.IsCompleted = ViewModel.IsCompleted;

            planner.FullFlightName = $"{ViewModel.ICAODeparture} -> {ViewModel.ICAOArrival}";
            
            await context.SaveChangesAsync();
            
            var nauticalMiles = ViewModel.FlightSpecs?.NauticalMiles ?? 0;
            
            var newSpecs = CalculateFlightSpecs(
                PlannerID: planner.ID,
                NauticalMiles: nauticalMiles,
                AircraftModel: ViewModel.AircraftModel,
                FlightType: ViewModel.FlightType);

            var specs = await context.FlightSpecs.FirstOrDefaultAsync(s => s.PlannerID == planner.ID);
            if (specs == null)
            {
                specs = newSpecs;
                context.FlightSpecs.Add(specs);
            }
            else
            {
                specs.NauticalMiles = newSpecs.NauticalMiles;
                specs.CruiseSpeedKnots = newSpecs.CruiseSpeedKnots;
                specs.FlightEstimatedHourTime = newSpecs.FlightEstimatedHourTime;
                specs.FlightEstimatedMinutesTime = newSpecs.FlightEstimatedMinutesTime;
                specs.BasicFuel = newSpecs.BasicFuel;
                specs.AverageFuelConsumption = newSpecs.AverageFuelConsumption;
                specs.ReserveFuel = newSpecs.ReserveFuel;
                specs.ReserveFuelGal = newSpecs.ReserveFuelGal;
                specs.TotalFuel = newSpecs.TotalFuel;
                specs.TotalFuelGal = newSpecs.TotalFuelGal;
                specs.AltitudeFeet = newSpecs.AltitudeFeet;
                specs.AscentRateFeetPerMinute = newSpecs.AscentRateFeetPerMinute;
                specs.DescentRateFeetPerMinute = newSpecs.DescentRateFeetPerMinute;
                specs.FinalApproachSpeed = newSpecs.FinalApproachSpeed;
                specs.DescentVerticalSpeed = newSpecs.DescentVerticalSpeed;
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new ApplicationException($"Error al actualizar el plan de vuelo: {ex.Message}", ex);
        }
    }

    private static (int Speed, int AverageFuel, int ReserveFuel, int AltitudeFeet, int AscentRate, int DescentRate, int FinalApproachSpeed, int DescentVerticalSpeed) GetAircraftConfig(AircraftModelEnum AircraftModel, FlightTypesEnum FlightType)
    {
        int speed = 0;
        int averageFuel = 0;
        int reserveFuel = 0;
        int altitudeFeet = 0;
        int ascentRate = 0;
        int descentRate = 0;
        int finalApproachSpeed = 0;
        int descentVerticalSpeed = 0;

        if (AircraftModel == AircraftModelEnum.CESSNA_CITATION_LONGITUDE)
        {
            switch (FlightType)
            {
                case FlightTypesEnum.HIGH_ALTITUDE_FLIGHT:
                    altitudeFeet = GlobalFormulas.CESSNACL_HA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.CESSNACL_HA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.CESSNACL_HA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.CESSNACL_HA_RESERVE_FUEL;
                    break;
                    
                case FlightTypesEnum.NORMAL_FLIGHT:
                    altitudeFeet = GlobalFormulas.CESSNACL_MA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.CESSNACL_MA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.CESSNACL_MA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.CESSNACL_MA_RESERVE_FUEL;
                    break;
                    
                case FlightTypesEnum.SHORT_FLIGHT:
                    altitudeFeet = GlobalFormulas.CESSNACL_LA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.CESSNACL_LA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.CESSNACL_LA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.CESSNACL_LA_RESERVE_FUEL;
                    break;
            }
        }
        else if (AircraftModel == AircraftModelEnum.EMBRAER_E190)
        {
            switch (FlightType)
            {
                case FlightTypesEnum.HIGH_ALTITUDE_FLIGHT:
                    altitudeFeet = GlobalFormulas.EMBRAER_E190_HA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.EMBRAER_E190_HA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.EMBRAER_E190_HA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.EMBRAER_E190_HA_RESERVE_FUEL;
                    break;
                    
                case FlightTypesEnum.NORMAL_FLIGHT:
                    altitudeFeet = GlobalFormulas.EMBRAER_E190_MA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.EMBRAER_E190_MA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.EMBRAER_E190_MA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.EMBRAER_E190_MA_RESERVE_FUEL;
                    break;
                    
                case FlightTypesEnum.SHORT_FLIGHT:
                    altitudeFeet = GlobalFormulas.EMBRAER_E190_LA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.EMBRAER_E190_LA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.EMBRAER_E190_LA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.EMBRAER_E190_LA_RESERVE_FUEL;
                    break;
            }
        }
        else if (AircraftModel == AircraftModelEnum.EMBRAER_E195)
        {
            switch (FlightType)
            {
                case FlightTypesEnum.HIGH_ALTITUDE_FLIGHT:
                    altitudeFeet = GlobalFormulas.EMBRAER_E195_HA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.EMBRAER_E195_HA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.EMBRAER_E195_HA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.EMBRAER_E195_HA_RESERVE_FUEL;
                    break;
                    
                case FlightTypesEnum.NORMAL_FLIGHT:
                    altitudeFeet = GlobalFormulas.EMBRAER_E195_MA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.EMBRAER_E195_MA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.EMBRAER_E195_MA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.EMBRAER_E195_MA_RESERVE_FUEL;
                    break;
                    
                case FlightTypesEnum.SHORT_FLIGHT:
                    altitudeFeet = GlobalFormulas.EMBRAER_E195_LA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.EMBRAER_E195_LA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.EMBRAER_E195_LA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.EMBRAER_E195_LA_RESERVE_FUEL;
                    break;
            }
        }
        else if (AircraftModel == AircraftModelEnum.AIRBUS_A320NEO)
        {
            switch (FlightType)
            {
                case FlightTypesEnum.HIGH_ALTITUDE_FLIGHT:
                    altitudeFeet = GlobalFormulas.AIRBUS_A320NEO_HA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.AIRBUS_A320NEO_HA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.AIRBUS_A320NEO_HA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.AIRBUS_A320NEO_HA_RESERVE_FUEL;
                    break;
                    
                case FlightTypesEnum.NORMAL_FLIGHT:
                    altitudeFeet = GlobalFormulas.AIRBUS_A320NEO_MA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.AIRBUS_A320NEO_MA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.AIRBUS_A320NEO_MA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.AIRBUS_A320NEO_MA_RESERVE_FUEL;
                    break;
                    
                case FlightTypesEnum.SHORT_FLIGHT:
                    altitudeFeet = GlobalFormulas.AIRBUS_A320NEO_LA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.AIRBUS_A320NEO_LA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.AIRBUS_A320NEO_LA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.AIRBUS_A320NEO_LA_RESERVE_FUEL;
                    break;
            }
        }
        else if (AircraftModel == AircraftModelEnum.BOEING_737_MAX)
        {
            switch (FlightType)
            {
                case FlightTypesEnum.HIGH_ALTITUDE_FLIGHT:
                    altitudeFeet = GlobalFormulas.BOEING_737_MAX_HA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.BOEING_737_MAX_HA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.BOEING_737_MAX_HA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.BOEING_737_MAX_HA_RESERVE_FUEL;
                    break;
                    
                case FlightTypesEnum.NORMAL_FLIGHT:
                    altitudeFeet = GlobalFormulas.BOEING_737_MAX_MA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.BOEING_737_MAX_MA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.BOEING_737_MAX_MA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.BOEING_737_MAX_MA_RESERVE_FUEL;
                    break;
                    
                case FlightTypesEnum.SHORT_FLIGHT:
                    altitudeFeet = GlobalFormulas.BOEING_737_MAX_LA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.BOEING_737_MAX_LA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.BOEING_737_MAX_LA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.BOEING_737_MAX_LA_RESERVE_FUEL;
                    break;
            }
        }
        else if (AircraftModel == AircraftModelEnum.CESSNA_S172_SKYHAWK)
        {
            /*
                 Este avión tiene un caso muy particular respecto a las distancias
                 de vuelo:
                 Largo Alcance: 450 - 640 NM
                 Mediano Alcance: 250 - 450 NM
                 Corto Alcance: 100 - 250 NM
             */
            switch (FlightType)
            {
                case FlightTypesEnum.HIGH_ALTITUDE_FLIGHT:
                    altitudeFeet = GlobalFormulas.CESSNA_S172_HA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.CESSNA_S172_HA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.CESSNA_S172_HA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.CESSNA_S172_HA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.CESSNA_S172_ASCENT_RATE;
                    descentRate = GlobalFormulas.CESSNA_S172_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.CESSNA_S172_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.CESSNA_S172_DESCENT_VERTICAL_SPEED;
                    break;
                    
                case FlightTypesEnum.NORMAL_FLIGHT:
                    altitudeFeet = GlobalFormulas.CESSNA_S172_MA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.CESSNA_S172_MA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.CESSNA_S172_MA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.CESSNA_S172_MA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.CESSNA_S172_ASCENT_RATE;
                    descentRate = GlobalFormulas.CESSNA_S172_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.CESSNA_S172_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.CESSNA_S172_DESCENT_VERTICAL_SPEED;
                    break;
                    
                case FlightTypesEnum.SHORT_FLIGHT:
                    altitudeFeet = GlobalFormulas.CESSNA_S172_LA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.CESSNA_S172_LA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.CESSNA_S172_LA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.CESSNA_S172_LA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.CESSNA_S172_ASCENT_RATE;
                    descentRate = GlobalFormulas.CESSNA_S172_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.CESSNA_S172_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.CESSNA_S172_DESCENT_VERTICAL_SPEED;
                    break;
            }
        }
        else if (AircraftModel == AircraftModelEnum.DIAMOND_DA40)
        {
            /*
                 Este avión tiene un caso muy particular respecto a las distancias
                 de vuelo:
                 Largo Alcance: 720 NM
                 Mediano Alcance: 480 NM
                 Corto Alcance: 250 NM
             */
            switch (FlightType)
            {
                case FlightTypesEnum.HIGH_ALTITUDE_FLIGHT:
                    altitudeFeet = GlobalFormulas.DIAMOND_DA40_HA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.DIAMOND_DA40_HA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.DIAMOND_DA40_HA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.DIAMOND_DA40_HA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.DIAMOND_DA40_ASCENT_RATE;
                    descentRate = GlobalFormulas.DIAMOND_DA40_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.DIAMOND_DA40_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.DIAMOND_DA40_DESCENT_VERTICAL_SPEED;
                    break;
                    
                case FlightTypesEnum.NORMAL_FLIGHT:
                    altitudeFeet = GlobalFormulas.DIAMOND_DA40_MA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.DIAMOND_DA40_MA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.DIAMOND_DA40_MA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.DIAMOND_DA40_MA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.DIAMOND_DA40_ASCENT_RATE;
                    descentRate = GlobalFormulas.DIAMOND_DA40_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.DIAMOND_DA40_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.DIAMOND_DA40_DESCENT_VERTICAL_SPEED;
                    break;
                    
                case FlightTypesEnum.SHORT_FLIGHT:
                    altitudeFeet = GlobalFormulas.DIAMOND_DA40_LA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.DIAMOND_DA40_LA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.DIAMOND_DA40_LA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.DIAMOND_DA40_LA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.DIAMOND_DA40_ASCENT_RATE;
                    descentRate = GlobalFormulas.DIAMOND_DA40_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.DIAMOND_DA40_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.DIAMOND_DA40_DESCENT_VERTICAL_SPEED;
                    break;
            }
        }
        else if (AircraftModel == AircraftModelEnum.DIAMOND_DA62)
        {
            /*
                 Este avión tiene un caso muy particular respecto a las distancias
                 de vuelo:
                 Largo Alcance: 1000 NM
                 Mediano Alcance: 400 NM
                 Corto Alcance: 150 NM
             */
            switch (FlightType)
            {
                case FlightTypesEnum.HIGH_ALTITUDE_FLIGHT:
                    altitudeFeet = GlobalFormulas.DIAMOND_DA62_HA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.DIAMOND_DA62_HA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.DIAMOND_DA62_HA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.DIAMOND_DA62_HA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.DIAMOND_DA62_ASCENT_RATE;
                    descentRate = GlobalFormulas.DIAMOND_DA62_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.DIAMOND_DA62_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.DIAMOND_DA62_DESCENT_VERTICAL_SPEED;
                    break;
                    
                case FlightTypesEnum.NORMAL_FLIGHT:
                    altitudeFeet = GlobalFormulas.DIAMOND_DA62_MA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.DIAMOND_DA62_MA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.DIAMOND_DA62_MA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.DIAMOND_DA62_MA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.DIAMOND_DA62_ASCENT_RATE;
                    descentRate = GlobalFormulas.DIAMOND_DA62_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.DIAMOND_DA62_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.DIAMOND_DA62_DESCENT_VERTICAL_SPEED;
                    break;
                    
                case FlightTypesEnum.SHORT_FLIGHT:
                    altitudeFeet = GlobalFormulas.DIAMOND_DA62_LA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.DIAMOND_DA62_LA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.DIAMOND_DA62_LA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.DIAMOND_DA62_LA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.DIAMOND_DA62_ASCENT_RATE;
                    descentRate = GlobalFormulas.DIAMOND_DA62_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.DIAMOND_DA62_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.DIAMOND_DA62_DESCENT_VERTICAL_SPEED;
                    break;
            }
        }
        else if (AircraftModel == AircraftModelEnum.CESSNA_C400)
        {
            /*
                 Este avión tiene un caso muy particular respecto a las distancias
                 de vuelo:
                 Largo Alcance: 1100 NM
                 Mediano Alcance: 400 NM
                 Corto Alcance: 150 NM
             */
            switch (FlightType)
            {
                case FlightTypesEnum.HIGH_ALTITUDE_FLIGHT:
                    altitudeFeet = GlobalFormulas.CESSNA_C400_HA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.CESSNA_C400_HA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.CESSNA_C400_HA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.CESSNA_C400_HA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.CESSNA_C400_ASCENT_RATE;
                    descentRate = GlobalFormulas.CESSNA_C400_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.CESSNA_C400_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.CESSNA_C400_DESCENT_VERTICAL_SPEED;
                    break;
                    
                case FlightTypesEnum.NORMAL_FLIGHT:
                    altitudeFeet = GlobalFormulas.CESSNA_C400_MA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.CESSNA_C400_MA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.CESSNA_C400_MA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.CESSNA_C400_MA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.CESSNA_C400_ASCENT_RATE;
                    descentRate = GlobalFormulas.CESSNA_C400_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.CESSNA_C400_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.CESSNA_C400_DESCENT_VERTICAL_SPEED;
                    break;
                    
                case FlightTypesEnum.SHORT_FLIGHT:
                    altitudeFeet = GlobalFormulas.CESSNA_C400_LA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.CESSNA_C400_LA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.CESSNA_C400_LA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.CESSNA_C400_LA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.CESSNA_C400_ASCENT_RATE;
                    descentRate = GlobalFormulas.CESSNA_C400_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.CESSNA_C400_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.CESSNA_C400_DESCENT_VERTICAL_SPEED;
                    break;
            }
        }
        else if (AircraftModel == AircraftModelEnum.PIPISTREL_VIRUS)
        {
            /*
                 Este avión tiene un caso muy particular respecto a las distancias
                 de vuelo:
                 Largo Alcance: 480 NM
                 Mediano Alcance: 300 NM
                 Corto Alcance: 120 NM
             */
            switch (FlightType)
            {
                case FlightTypesEnum.HIGH_ALTITUDE_FLIGHT:
                    altitudeFeet = GlobalFormulas.PIPISTREL_VIRUS_HA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.PIPISTREL_VIRUS_HA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.PIPISTREL_VIRUS_HA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.PIPISTREL_VIRUS_HA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.PIPISTREL_VIRUS_ASCENT_RATE;
                    descentRate = GlobalFormulas.PIPISTREL_VIRUS_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.PIPISTREL_VIRUS_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.PIPISTREL_VIRUS_DESCENT_VERTICAL_SPEED;
                    break;
                    
                case FlightTypesEnum.NORMAL_FLIGHT:
                    altitudeFeet = GlobalFormulas.PIPISTREL_VIRUS_MA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.PIPISTREL_VIRUS_MA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.PIPISTREL_VIRUS_MA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.PIPISTREL_VIRUS_MA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.PIPISTREL_VIRUS_ASCENT_RATE;
                    descentRate = GlobalFormulas.PIPISTREL_VIRUS_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.PIPISTREL_VIRUS_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.PIPISTREL_VIRUS_DESCENT_VERTICAL_SPEED;
                    break;
                    
                case FlightTypesEnum.SHORT_FLIGHT:
                    altitudeFeet = GlobalFormulas.PIPISTREL_VIRUS_LA_CRUISE_ALTITUDE;
                    speed = GlobalFormulas.PIPISTREL_VIRUS_LA_CRUISE_SPEED;
                    averageFuel = GlobalFormulas.PIPISTREL_VIRUS_LA_AVERAGE_FUEL;
                    reserveFuel = GlobalFormulas.PIPISTREL_VIRUS_LA_RESERVE_FUEL;
                    ascentRate = GlobalFormulas.PIPISTREL_VIRUS_ASCENT_RATE;
                    descentRate = GlobalFormulas.PIPISTREL_VIRUS_DESCENT_RATE;
                    finalApproachSpeed = GlobalFormulas.PIPISTREL_VIRUS_FINAL_APPROACH_SPEED;
                    descentVerticalSpeed = GlobalFormulas.PIPISTREL_VIRUS_DESCENT_VERTICAL_SPEED;
                    break;
            }
        }

        return (
            Speed: speed,
            AverageFuel: averageFuel,
            ReserveFuel: reserveFuel,
            AltitudeFeet: altitudeFeet,
            AscentRate: ascentRate,
            DescentRate: descentRate,
            FinalApproachSpeed: finalApproachSpeed,
            DescentVerticalSpeed: descentVerticalSpeed);
    }
    
    private static FlightSpecs CalculateFlightSpecs(Guid PlannerID, int NauticalMiles, AircraftModelEnum AircraftModel, FlightTypesEnum FlightType)
    {
        var config = GetAircraftConfig(
            AircraftModel: AircraftModel,
            FlightType: FlightType);
        int speed = config.Speed;
        int averageFuel = config.AverageFuel;
        int reserveFuel = config.ReserveFuel;
        int altitudeFeet = config.AltitudeFeet;
        int ascentRate = config.AscentRate;
        int descentRate = config.DescentRate;
        int finalApproachSpeed = config.FinalApproachSpeed;
        int descentVerticalSpeed = config.DescentVerticalSpeed;

        decimal flightEstimatedHourTime = Math.Round((decimal)NauticalMiles / speed, 2);
        int flightEstimatedMinutesTime = (int)(flightEstimatedHourTime * 60);

        decimal basicFuel = averageFuel * flightEstimatedHourTime;
        double alternateFuel = Math.Round(averageFuel * 0.33);
        double contingencyFuel = Math.Round((double)basicFuel * 0.15);

        int taxiHoldingFuel;
        if (AircraftModel == AircraftModelEnum.DIAMOND_DA40)
        {
            taxiHoldingFuel = GlobalFormulas.DIAMOND_DA40_TAXI_HOLDING_FUEL;
        }
        else if (AircraftModel == AircraftModelEnum.DIAMOND_DA62)
        {
            taxiHoldingFuel = GlobalFormulas.DIAMOND_DA62_TAXI_HOLDING_FUEL;
        }
        else if (AircraftModel == AircraftModelEnum.CESSNA_S172_SKYHAWK)
        {
            taxiHoldingFuel = GlobalFormulas.CESSNA_S172_TAXI_HOLDING_FUEL;
        }
        else if (AircraftModel == AircraftModelEnum.CESSNA_C400)
        {
            taxiHoldingFuel = GlobalFormulas.CESSNA_C400_TAXI_HOLDING_FUEL;
        }
        else if (AircraftModel == AircraftModelEnum.PIPISTREL_VIRUS)
        {
            taxiHoldingFuel = GlobalFormulas.PIPISTREL_VIRUS_TAXI_HOLDING_FUEL;
        }
        else
        {
            taxiHoldingFuel = GlobalFormulas.TAXI_HOLDING_FUEL;
        }
        
        double totalFuel = (double)basicFuel + alternateFuel + contingencyFuel + reserveFuel + taxiHoldingFuel;

        decimal totalFuelGal = Math.Round((decimal)totalFuel / GlobalFormulas.DENSITY_FUEL_GAL, 2);
        decimal reserveFuelGal = Math.Round(reserveFuel / GlobalFormulas.DENSITY_FUEL_GAL, 2);

        return new FlightSpecs
        {
            PlannerID = PlannerID,
            NauticalMiles = NauticalMiles,
            CruiseSpeedKnots = speed,
            FlightEstimatedHourTime = flightEstimatedHourTime,
            FlightEstimatedMinutesTime = flightEstimatedMinutesTime,
            BasicFuel = basicFuel,
            AverageFuelConsumption = averageFuel,
            ReserveFuel = reserveFuel,
            ReserveFuelGal = reserveFuelGal,
            TotalFuel = (decimal)totalFuel,
            TotalFuelGal = totalFuelGal,
            AltitudeFeet = altitudeFeet,
            AscentRateFeetPerMinute = ascentRate,
            DescentRateFeetPerMinute = descentRate,
            FinalApproachSpeed = finalApproachSpeed,
            DescentVerticalSpeed = descentVerticalSpeed
        };
    }
}