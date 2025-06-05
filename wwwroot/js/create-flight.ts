namespace flight_create {
    function getIntValue(id: string): number | null {
        const value = (document.getElementById(id) as HTMLInputElement | HTMLSelectElement).value;
        return value === "" ? null : parseInt(value);
    }

    function getFloatValue(id: string): number | null {
        const value = (document.getElementById(id) as HTMLInputElement).value;
        return value === "" ? null : parseFloat(value);
    }
    
    export function save(): void {
        const rawDate = (document.getElementById("Date") as HTMLInputElement).value;
        const parsedDate = new Date(`${rawDate}T00:00:00`);
        const dateIso = parsedDate.toISOString();

        const RECORD = {
            Date: dateIso,
            ICAODeparture: (document.getElementById("ICAODeparture") as HTMLInputElement).value,
            DepartureAirportName: (document.getElementById("DepartureAirportName") as HTMLInputElement).value,
            BaroPressureDeparture: getFloatValue("BaroPressureDeparture"),
            TransitionAltitudeDeparture: getIntValue("TransitionAltitudeDeparture"),
            ICAOArrival: (document.getElementById("ICAOArrival") as HTMLInputElement).value,
            ArrivalAirportName: (document.getElementById("ArrivalAirportName") as HTMLInputElement).value || null,
            BaroPressureArrival: getFloatValue("BaroPressureArrival"),
            TransitionAltitudeArrival: getIntValue("TransitionAltitudeArrival"),
            ArrivalRunwayElevation: getIntValue("ArrivalRunwayElevation"),
            ArrivalRunwayMinimumAltitude: getIntValue("ArrivalRunwayMinimumAltitude"),
            LocalizerFrequency: getFloatValue("LocalizerFrequency"),
            LocalizerVectorName: (document.getElementById("LocalizerVectorName") as HTMLInputElement).value || null,
            ApproachType: getIntValue("ApproachType"),
            DepartureRunway: getIntValue("DepartureRunway"),
            ArrivalRunway: getIntValue("ArrivalRunway"),
            AircraftModel: getIntValue("AircraftModel"),
            FlightType: getIntValue("FlightType"),
            ArrivalRunwayLength: getIntValue("ArrivalRunwayLength"),
            LocalizerVectorAltitude: getIntValue("LocalizerVectorAltitude"),
            AltitudeFeet: getIntValue("AltitudeFeet"),
            
            FlightSpecs: {
                NauticalMiles: getIntValue("NauticalMiles"),
                CruiseSpeedKnots: getIntValue("CruiseSpeedKnots")
            }
        };

        fetch("/Flight/Create?handler=Save", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(RECORD)
        })
        .then(response => {
        if (!response.ok) throw new Error("Error en la respuesta");
        return response.json();
        })
        .then(data => {
            if (!data.success) {
                alert(data.message);
                return;
            }
            
            alert("Plan de vuelo guardado correctamente.");
            window.location.href = `/Flight/Details?ID=${data.ID}`;
        })
        .catch(error => {
            alert("Error al guardar el plan de vuelo.");
        });
    }
    
    document.addEventListener("DOMContentLoaded", () => {
        const SAVE_BUTTON = document.getElementById("btnSaveFlight");
        SAVE_BUTTON?.addEventListener("click", () => save());
    })
}