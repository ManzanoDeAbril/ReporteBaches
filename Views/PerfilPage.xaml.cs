using System;
using Microsoft.Maui.Controls;

namespace ReporteBaches.app.Views;

public partial class PerfilPage : ContentPage
{
    public PerfilPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Método sobreescrito de MAUI que se dispara de forma automática cada vez que
    /// esta pestaña se vuelve visible en la pantalla del celular o PC.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing(); // Llama a la lógica base de visualización

        // Verifica si el contexto de datos (BindingContext) es nuestro PerfilViewModel
        if (BindingContext is ViewModels.PerfilViewModel vm)
        {
            vm.CargarDatos(); // Dispara la recarga e inicialización de puntos acumulados
        }
    }
}