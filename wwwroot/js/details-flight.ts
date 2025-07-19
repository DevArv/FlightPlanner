namespace flight_details { 
    export function remove(ID : string): void {
        if (!confirm("¿Estás seguro de que quieres eliminar este vuelo? Esta acción no se puede deshacer.")) {
            return;
        }
        
        fetch(`/Flight/Details?handler=Delete`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ ID: ID })
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
            
            window.location.href = "/Index";
        })
        .catch(error => {
            alert(`Error al eliminar el plan de vuelo: ${error.message}`);
        });
    }
    
    export function edit(ID: string): void {
        window.location.href = `/Flight/Edit?ID=${encodeURIComponent(ID)}`;
    }
}
