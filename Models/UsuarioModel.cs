using CommunityToolkit.Mvvm.ComponentModel;

namespace ReporteBaches.app.Models;

// Extendemos ObservableObject para que los cambios en propiedades avisen a la UI
public partial class UsuarioModel : ObservableObject
{
    [ObservableProperty]
    public partial string? NombreUsuario { get; set; }

    [ObservableProperty]
    public partial string? Email { get; set; }

    // [NotifyPropertyChangedFor] hace que cuando PuntosAcumulados cambie,
    // también se notifique que RangoCiudadano cambió (ya que depende de él)
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RangoCiudadano))]
    public partial int PuntosAcumulados { get; set; }

    // Esta propiedad calculada sigue igual, solo lectura
    public string RangoCiudadano => PuntosAcumulados switch
    {
        < 100 => "Ciudadano Iniciante 🥉",
        < 300 => "Guardián de las Vías 🥈",
        _ => "Héroe del Pavimento 🥇"
    };
}