using System;

namespace ReporteBaches.app.Models;

/// <summary>
/// Clase pública que define la estructura de datos de una incidencia vial.
/// </summary>
public class ReporteModel
{
    // 'Guid.NewGuid().ToString()' genera un identificador único global aleatorio (ej. 9b1deb4d-3b7d-4bad...)
    // para que cada bache tenga un ID único.
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Descripción del daño de la calle (string que puede ser nulo)
    public string? Descripcion { get; set; }

    // Latitud (coordenada GPS decimal de doble precisión, ej: 19.432608)
    public double Latitud { get; set; }

    // Longitud (coordenada GPS decimal, ej: -99.133209)
    public double Longitud { get; set; }

    // Ruta física de la imagen guardada en el dispositivo (ej: /cache/photo.jpg)
    public string? RutaImagenLocal { get; set; }

    // Guarda la fecha y hora del sistema en el instante exacto en que se crea la clase
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    // Estado de la reparación (Inicializado en "Pendiente")
    public string Estado { get; set; } = "Pendiente";
}
