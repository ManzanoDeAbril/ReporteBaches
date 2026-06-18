using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;
using ReporteBaches.app.Views;
using ReporteBaches.app.ViewModels;

namespace ReporteBaches.app;

/// <summary>
/// Clase de inicio del programa. Configura e inicializa el motor de la app de .NET MAUI.
/// </summary>
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // 'CreateBuilder()' inicia la configuración del cargador de servicios de .NET
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>() // Le dice al motor que nuestra clase principal es App
            .ConfigureFonts(fonts =>
            {
                // Registra las tipografías personalizadas disponibles en la app
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // =========================================================================
        // REGISTRO DE DEPENDENCIAS (Inyección de Dependencias)
        // Esto le enseña a .NET cómo instanciar y conectar automáticamente las clases.
        // 'AddTransient' crea una nueva instancia de la clase cada vez que se requiere.
        // =========================================================================

        // Registro de los ViewModels (Lógica)
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ReportesViewModel>();
        builder.Services.AddTransient<PerfilViewModel>();

        // Registro de las Vistas (Diseños)
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ReportarPage>();
        builder.Services.AddTransient<HistorialPage>();
        builder.Services.AddTransient<PerfilPage>();

        return builder.Build(); // Construye la aplicación MAUI configurada
    }
}