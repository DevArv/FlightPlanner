"use strict";
var flight_create;
(function (flight_create) {
    function getIntValue(id) {
        const value = document.getElementById(id).value;
        return value === "" ? null : parseInt(value);
    }
    function getFloatValue(id) {
        const value = document.getElementById(id).value;
        return value === "" ? null : parseFloat(value);
    }
    function save() {
        const rawDate = document.getElementById("Date").value;
        const parsedDate = new Date(`${rawDate}T00:00:00`);
        const dateIso = parsedDate.toISOString();
        const RECORD = {
            Date: dateIso,
            ICAODeparture: document.getElementById("ICAODeparture").value,
            DepartureAirportName: document.getElementById("DepartureAirportName").value,
            BaroPressureDeparture: getFloatValue("BaroPressureDeparture"),
            TransitionAltitudeDeparture: getIntValue("TransitionAltitudeDeparture"),
            ICAOArrival: document.getElementById("ICAOArrival").value,
            ArrivalAirportName: document.getElementById("ArrivalAirportName").value || null,
            BaroPressureArrival: getFloatValue("BaroPressureArrival"),
            TransitionAltitudeArrival: getIntValue("TransitionAltitudeArrival"),
            ArrivalRunwayElevation: getIntValue("ArrivalRunwayElevation"),
            ArrivalRunwayMinimumAltitude: getIntValue("ArrivalRunwayMinimumAltitude"),
            LocalizerFrequency: getFloatValue("LocalizerFrequency"),
            LocalizerVectorName: document.getElementById("LocalizerVectorName").value || null,
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
            window.location.href = `/Flight/Details/${data.flightId}`;
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
