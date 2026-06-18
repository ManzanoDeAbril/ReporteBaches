using System;
using System.Threading.Tasks; // Necesario para trabajar con programación asíncrona (Task)
using CommunityToolkit.Mvvm.ComponentModel; // Biblioteca MVVM para dar soporte a clases observables
using CommunityToolkit.Mvvm.Input; // Biblioteca MVVM para el manejo de comandos (botones)

namespace ReporteBaches.app.ViewModels;

/// <summary>
/// 'partial' permite que el generador de código del MVVM Toolkit agregue métodos automáticamente en otro archivo oculto.
/// 'ObservableObject' le da a esta clase la capacidad de avisar a la pantalla XAML cuando cambie alguna propiedad.
/// </summary>
public partial class LoginViewModel : ObservableObject
{
    // '[ObservableProperty]' genera automáticamente una propiedad pública llamada 'Email' (con E mayúscula)
    // 'public partial string?' declara la propiedad parcial compatible con AOT (necesario para Windows Machine).
    [ObservableProperty]
    public partial string? Email { get; set; }

    [ObservableProperty]
    public partial string? Password { get; set; }

    // Controla si la app está cargando para deshabilitar botones y mostrar un indicador visual
    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    /// <summary>
    /// Comando que se ejecutará cuando el usuario presione el botón de Login.
    /// '[RelayCommand]' transforma el método C# en un comando enlazable a la UI.
    /// 'async' indica que el método es asíncrono (puede ejecutarse en segundo plano sin congelar la app).
    /// </summary>
    [RelayCommand]
    private async Task IniciarSesionAsync()
    {
        // 'string.IsNullOrWhiteSpace()' valida si el texto es nulo, vacío o tiene puros espacios
        // '||' es el operador lógico "O" (OR)
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            // 'await' espera a que el diálogo emergente termine de mostrarse antes de continuar.
            // 'DisplayAlertAsync' es un método asíncrono de MAUI para mostrar diálogos en pantalla.
            await Shell.Current.DisplayAlertAsync("Campos Vacíos", "Ingresa tu correo y contraseña.", "Aceptar");
            return; // Termina la ejecución del método aquí si faltan datos
        }

        // 'try-catch' es una estructura de control para manejar errores sin que la app se cierre inesperadamente.
        try
        {
            // Bloque de código que intentará ejecutarse
            IsBusy = true; // Activa el ActivityIndicator en la pantalla

            // 'await Task.Delay(1200)' detiene la ejecución del método durante 1200 milisegundos (1.2 seg)
            // de manera asíncrona (simulando una petición HTTP de red al servidor) sin bloquear la interfaz.
            await Task.Delay(1200);

            // 'App.UsuarioActual' guarda los datos del login en una sesión estática global
            App.UsuarioActual = new Models.UsuarioModel
            {
                NombreUsuario = Email.Split('@')[0], // Corta el correo y toma la primera parte antes del '@'
                Email = Email,
                PuntosAcumulados = 120 // Puntos iniciales ficticios
            };

            // 'Shell.Current.GoToAsync()' navega a una ruta del menú principal de la app.
            // '//' borra el historial de navegación para que el usuario no pueda "volver atrás" al Login.
            await Shell.Current.GoToAsync("//MainTabs");
        }
        catch (Exception ex)
        {
            // 'catch' atrapa cualquier error imprevisto (excepción) ocurrido dentro del bloque 'try'.
            // 'ex.Message' contiene la descripción técnica del error ocurrido.
            await Shell.Current.DisplayAlertAsync("Error", $"No se pudo ingresar: {ex.Message}", "Aceptar");
        }
        finally
        {
            // 'finally' se ejecuta SIEMPRE al final, haya ocurrido un error o no.
            IsBusy = false; // Apaga el ActivityIndicator de carga
        }
    }
}