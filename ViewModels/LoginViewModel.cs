using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ReporteBaches.app.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    /// <summary>
    /// Credenciales fijas para el login simulado (prototipo).
    /// En una versión real se consultaría una base de datos o API.
    /// </summary>
    private const string UsuarioValido = "admin";
    private const string ContrasenaValida = "1234";

    /// <summary>
    /// Propiedad observable enlazada al Entry "Usuario o Correo" del LoginPage.xaml.
    /// Cuando el usuario escribe, se actualiza automáticamente gracias a [ObservableProperty].
    /// </summary>
    [ObservableProperty]
    public partial string? Email { get; set; }

    /// <summary>
    /// Propiedad observable enlazada al Entry "Contraseña" del LoginPage.xaml.
    /// Almacena la contraseña escrita por el usuario.
    /// </summary>
    [ObservableProperty]
    public partial string? Password { get; set; }

    /// <summary>
    /// Propiedad observable que controla la visibilidad del ActivityIndicator (rueda de carga).
    /// Se activa (true) mientras se procesa el inicio de sesión y se desactiva (false) al terminar.
    /// Enlazada al XAML con: IsRunning="{Binding IsBusy}"
    /// </summary>
    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    /// <summary>
    /// Método que se ejecuta al presionar el botón "Entrar" en LoginPage.xaml.
    /// Se convierte automáticamente en un comando (IniciarSesionCommand) gracias al atributo [RelayCommand].
    /// Realiza 3 pasos: validar campos vacíos, validar credenciales y navegar a las pestañas principales.
    /// </summary>
    [RelayCommand]
    private async Task IniciarSesionAsync()
    {
        // =========================================================================
        // PASO 1: Validar que los campos no estén vacíos
        // =========================================================================
        // IsNullOrWhiteSpace retorna true si el string es null, está vacío ("")
        // o contiene solo espacios en blanco. Si falla, mostramos una alerta y
        // salimos del método con "return" para no continuar.
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlertAsync("Campos Vacíos", "Ingresa tu usuario y contraseña.", "Aceptar");
            return;
        }

        // =========================================================================
        // PASO 2: Validar contra las credenciales fijas
        // =========================================================================
        // Compara lo ingresado contra las constantes UsuarioValido y ContrasenaValida.
        // Si no coinciden (operador OR ||), muestra alerta de error y sale.
        if (Email != UsuarioValido || Password != ContrasenaValida)
        {
            await Shell.Current.DisplayAlertAsync("Error", "Credenciales incorrectas.", "Aceptar");
            return;
        }

        // =========================================================================
        // PASO 3: Inicio de sesión exitoso
        // =========================================================================
        try
        {
            // Activa el ActivityIndicator para que el usuario vea que algo está pasando
            IsBusy = true;

            // Task.Delay(1200) simula 1.2 segundos de carga (como si consultara un servidor).
            // El "await" permite que la UI siga respondiendo mientras tanto.
            await Task.Delay(1200);

            // Crea una instancia de UsuarioModel y la asigna a la variable estática global
            // App.UsuarioActual, accesible desde cualquier pantalla del programa.
            // Se inicializa con 120 puntos para que el usuario arranque como "Guardián de las Vías".
            App.UsuarioActual = new Models.UsuarioModel
            {
                NombreUsuario = Email,
                Email = Email,
                PuntosAcumulados = 120
            };

            // Navega al TabBar (ruta "//MainTabs") que contiene las 3 pestañas:
            // Reportar, Historial y Mi Perfil.
            await Shell.Current.GoToAsync("//MainTabs");
        }
        catch (Exception ex)
        {
            // Si ocurre cualquier error inesperado (ej. problema de navegación),
            // se muestra una alerta con el mensaje de la excepción.
            await Shell.Current.DisplayAlertAsync("Error", $"No se pudo ingresar: {ex.Message}", "Aceptar");
        }
        finally
        {
            // El bloque "finally" se ejecuta SIEMPRE, haya funcionado o no.
            // Aquí desactivamos el ActivityIndicator para que desaparezca.
            IsBusy = false;
        }
    }
}