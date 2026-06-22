using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReporteBaches.app.Models;

namespace ReporteBaches.app.ViewModels;

public partial class PerfilViewModel : ObservableObject
{
    [ObservableProperty]
    public partial UsuarioModel? Usuario { get; set; }

    /// <summary>
    /// Sincroniza la información del ViewModel con la sesión estática global
    /// </summary>
    public void CargarDatos()
    {
        // Primero ponemos null para forzar que [ObservableProperty]
        // detecte un cambio real y dispare PropertyChanged al reasignar
        Usuario = null;
        Usuario = App.UsuarioActual;
    }

    [RelayCommand]
    private async Task CerrarSesionAsync()
    {
        // 'DisplayAlertAsync' con 4 parámetros retorna 'true' si el usuario da clic al primer botón ("Sí")
        // y 'false' si da clic al segundo ("No").
        bool confirmar = await Shell.Current.DisplayAlertAsync("Cerrar Sesión", "¿Quieres salir de tu cuenta?", "Sí", "No");

        if (confirmar)
        {
            App.UsuarioActual = null; // Limpia la sesión global en memoria
            await Shell.Current.GoToAsync("//Login"); // Redirige a la pantalla de login limpiando el historial
        }
    }
}