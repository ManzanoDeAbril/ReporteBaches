using System; // Importa el sistema base de .NET (tipos básicos)

namespace ReporteBaches.app.Models; // Organiza el código en el espacio 'Models' del proyecto

/// <summary>
/// Clase pública que representa al usuario (ciudadano) y sus logros en la app.
/// </summary>
public class UsuarioModel
{
    // 'string?' indica que el NombreUsuario puede ser una cadena de texto o ser nulo (null)
    // 'get; set;' permite leer (get) y escribir (set) el valor de esta propiedad
    public string? NombreUsuario { get; set; }

    // Correo del usuario (acepta valores nulos con el signo '?')
    public string? Email { get; set; }

    // Puntos acumulados (tipo 'int' para números enteros; no acepta nulos por defecto)
    public int PuntosAcumulados { get; set; }

    // Propiedad de solo lectura (get) que calcula el rango según los puntos.
    // Utiliza la sintaxis de expresión C# (=>) y una estructura 'switch' moderna.
    public string RangoCiudadano => PuntosAcumulados switch
    {
        < 100 => "Ciudadano Iniciante 🥉",  // Si es menor a 100 puntos
        < 300 => "Guardián de las Vías 🥈", // Si es menor a 300 puntos
        _ => "Héroe del Pavimento 🥇"        // El guion bajo (_) es el caso por defecto (cualquier otro valor)
    };
}