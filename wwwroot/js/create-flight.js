"use strict";
var flight_create;
(function (flight_create) {
    function save() {
        const rawDate = document.getElementById("Date").value;
        const parsedDate = new Date(`${rawDate}T00:00:00`);
        const dateIso = parsedDate.toISOString();
        const RECORD = {
            Date: dateIso,
            ICAODeparture: document.getElementById("ICAODeparture").value,
            DepartureAirportName: document.getElementById("DepartureAirportName").value,
            BaroPressureDeparture: parseFloat(document.getElementById("BaroPressureDeparture").value),
            TransitionAltitudeDeparture: parseInt(document.getElementById("TransitionAltitudeDeparture").value),
            ICAOArrival: document.getElementById("ICAOArrival").value,
            ArrivalAirportName: document.getElementById("ArrivalAirportName").value,
            BaroPressureArrival: parseFloat(document.getElementById("BaroPressureArrival").value),
            TransitionAltitudeArrival: parseInt(document.getElementById("TransitionAltitudeArrival").value),
            ArrivalRunwayElevation: parseInt(document.getElementById("ArrivalRunwayElevation").value),
            ArrivalRunwayMinimumAltitude: parseInt(document.getElementById("ArrivalRunwayMinimumAltitude").value),
            LocalizerFrequency: parseInt(document.getElementById("LocalizerFrequency").value),
            LocalizerVectorName: document.getElementById("LocalizerVectorName").value,
            ApproachType: parseInt(document.getElementById("ApproachType").value),
            DepartureRunway: parseInt(document.getElementById("DepartureRunway").value),
            ArrivalRunway: parseInt(document.getElementById("ArrivalRunway").value),
        };
        fetch("/Flight/Create?handler=Save", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(RECORD)
        })
            .then(response => {
            if (!response.ok)
                throw new Error("Error en la respuesta");
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
    flight_create.save = save;
    document.addEventListener("DOMContentLoaded", () => {
        const SAVE_BUTTON = document.getElementById("btnSaveFlight");
        SAVE_BUTTON === null || SAVE_BUTTON === void 0 ? void 0 : SAVE_BUTTON.addEventListener("click", () => save());
    });
})(flight_create || (flight_create = {}));
