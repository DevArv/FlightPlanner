namespace flight_create {
    export function save(): void {
        const rawDate = (document.getElementById("Date") as HTMLInputElement).value;
        const parsedDate = new Date(`${rawDate}T00:00:00`);
        const dateIso = parsedDate.toISOString();
        
        const RECORD = {
            Date: dateIso,
            ICAODeparture: (document.getElementById("ICAODeparture") as HTMLInputElement).value,
            DepartureAirportName: (document.getElementById("DepartureAirportName") as HTMLInputElement).value,
            BaroPressureDeparture: parseFloat((document.getElementById("BaroPressureDeparture") as HTMLInputElement).value),
            TransitionAltitudeDeparture: parseInt((document.getElementById("TransitionAltitudeDeparture") as HTMLInputElement).value),
            ICAOArrival: (document.getElementById("ICAOArrival") as HTMLInputElement).value,
            ArrivalAirportName: (document.getElementById("ArrivalAirportName") as HTMLInputElement).value,
            BaroPressureArrival: parseFloat((document.getElementById("BaroPressureArrival") as HTMLInputElement).value),
            TransitionAltitudeArrival: parseInt((document.getElementById("TransitionAltitudeArrival") as HTMLInputElement).value),
            ArrivalRunwayElevation: parseInt((document.getElementById("ArrivalRunwayElevation") as HTMLInputElement).value),
            ArrivalRunwayMinimumAltitude: parseInt((document.getElementById("ArrivalRunwayMinimumAltitude") as HTMLInputElement).value),
            LocalizerFrequency: parseInt((document.getElementById("LocalizerFrequency") as HTMLInputElement).value),
            LocalizerVectorName: (document.getElementById("LocalizerVectorName") as HTMLInputElement).value,
            ApproachType: parseInt((document.getElementById("ApproachType") as HTMLSelectElement).value),
            DepartureRunway: parseInt((document.getElementById("DepartureRunway") as HTMLInputElement).value),
            ArrivalRunway: parseInt((document.getElementById("ArrivalRunway") as HTMLInputElement).value),
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
            // window.location.href = "/Index"; //TODO: Redirigir a la página de inicio
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