using System;
using Microsoft.Maui.Controls;
using ReporteBaches.app.Models;

namespace ReporteBaches.app;

public partial class App : Application
{
    // 'public static' declara una variable de sesión en memoria estática
    // accesible desde cualquier otra clase o pantalla escribiendo 'App.UsuarioActual'.
    // '?' indica que puede ser nula (por ejemplo, antes de que el usuario inicie sesión).
    public static UsuarioModel? UsuarioActual { get; set; }

    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Método de ciclo de vida moderno en .NET 9/10 que se ejecuta al arrancar la app.
    /// Crea y configura la ventana principal de la interfaz visual.
    /// </summary>
    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Crea una ventana y le asigna el contenedor AppShell (que arranca en el Login)
        return new Window(new AppShell());
    }
}