using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ReporteBaches.app.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    // ✅ Credenciales fijas (cámbialas a lo que quieras)
    private const string UsuarioValido = "admin";
    private const string ContrasenaValida = "1234";

    [ObservableProperty]
    public partial string? Email { get; set; }

    [ObservableProperty]
    public partial string? Password { get; set; }

    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    [RelayCommand]
    private async Task IniciarSesionAsync()
    {
        // 1️⃣ Validar que los campos no estén vacíos
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlertAsync("Campos Vacíos", "Ingresa tu usuario y contraseña.", "Aceptar");
            return;
        }

        // 2️⃣ Validar contra las credenciales fijas
        if (Email != UsuarioValido || Password != ContrasenaValida)
        {
            await Shell.Current.DisplayAlertAsync("Error", "Credenciales incorrectas.", "Aceptar");
            return;
        }

        try
        {
            IsBusy = true;
            await Task.Delay(1200); // Simula carga

            App.UsuarioActual = new Models.UsuarioModel
            {
                NombreUsuario = Email,  // Ya no necesitamos Split('@') porque no es un correo
                Email = Email,
                PuntosAcumulados = 120
            };

            await Shell.Current.GoToAsync("//MainTabs");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"No se pudo ingresar: {ex.Message}", "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }
}