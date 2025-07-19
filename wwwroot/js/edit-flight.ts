namespace flight_edit {
    function getIntValue(id: string): number | null {
        const VALUE = (document.getElementById(id) as HTMLInputElement | HTMLSelectElement).value;
        return VALUE === "" ? null : parseInt(VALUE);
    }

    function getFloatValue(id: string): number | null {
        const VALUE = (document.getElementById(id) as HTMLInputElement).value;
        return VALUE === "" ? null : parseFloat(VALUE);
    }

    function getBoolValue(id: string): boolean {
        return (document.getElementById(id) as HTMLInputElement).checked;
    }

    export function save(): void {
        const RAW_DATE = (document.getElementById("Date") as HTMLInputElement).value;
        const PARSED_DATE = new Date(`${RAW_DATE}T00:00:00`);
        const DATE_ISO = PARSED_DATE.toISOString();
        const RECORD = {
            ID: (document.getElementById("ID") as HTMLInputElement).value,
            Date: DATE_ISO,
            ICAODeparture: (document.getElementById("ICAODeparture") as HTMLInputElement).value,
            DepartureAirportName: (document.getElementById("DepartureAirportName") as HTMLInputElement).value,
            BaroPressureDeparture: getFloatValue("BaroPressureDeparture"),
            TransitionAltitudeDeparture: getIntValue("TransitionAltitudeDeparture"),
            ICAOArrival: (document.getElementById("ICAOArrival") as HTMLInputElement).value,
            ArrivalAirportName: (document.getElementById("ArrivalAirportName") as HTMLInputElement).value,
            BaroPressureArrival: getFloatValue("BaroPressureArrival"),
            TransitionAltitudeArrival: getIntValue("TransitionAltitudeArrival"),
            ArrivalRunwayElevation: getIntValue("ArrivalRunwayElevation"),
            ArrivalRunwayMinimumAltitude: getIntValue("ArrivalRunwayMinimumAltitude"),
            LocalizerFrequency: getFloatValue("LocalizerFrequency"),
            LocalizerVectorName: (document.getElementById("LocalizerVectorName") as HTMLInputElement).value,
            ApproachType: getIntValue("ApproachType"),
            DepartureRunway: getIntValue("DepartureRunway"),
            ArrivalRunway: getIntValue("ArrivalRunway"),
            AircraftModel: getIntValue("AircraftModel"),
            FlightType: getIntValue("FlightType"),
            ArrivalRunwayLength: getIntValue("ArrivalRunwayLength"),
            LocalizerVectorAltitude: getIntValue("LocalizerVectorAltitude"),
            IsCompleted: getBoolValue("IsCompleted"),
            FlightSpecs: {
                NauticalMiles: getIntValue("NauticalMiles")
            }
        };

        fetch("/Flight/Edit?handler=Save", {
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

            // Guardamos el ID como atributo en el botón
            const BTN = document.getElementById("btnSave");
            if (BTN) {
                BTN.setAttribute("data-id", data.id);
            }
            
            window.location.href = `/Flight/Details?ID=${encodeURIComponent(data.id)}`;
        })
        .catch(error => {
            alert(`Error al guardar el plan de vuelo: ${error.message}`);
        });
    }
}